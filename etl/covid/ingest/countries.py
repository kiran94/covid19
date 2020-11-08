
import argparse
import logging
import pandas as pd
import os
import requests
import shutil
import snakecase
import pymongo
import numpy as np

from covid import working_directory, DATABASE_CONNECTION_STRING, COUNTRY_CONNECTION_STRING, COUNTRY_COLLECTIONNAME, COUNTRY_DATBASENAME
from covid.core.tracing import tracer, trace_command_line_arguments

working_sub_directory = os.path.join(working_directory, 'countries')
os.makedirs(working_sub_directory, exist_ok=True)


logger = logging.getLogger('covid.ingest.countries')


if __name__ == "__main__":

    parser = argparse.ArgumentParser()
    parser.add_argument('-s', '--step', choices=['fetch', 'load', 'clean'], required=True)
    parser.add_argument('-u', '--url',
                        type=str,
                        default='https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/UID_ISO_FIPS_LookUp_Table.csv',
                        help='URL of the data source (in CSV format)')
    parser.add_argument('--url_additional',
                        type=str,
                        default='https://restcountries.eu/rest/v2/all',
                        help='URL of the additional data source')
    parser.add_argument('--publish', action='store_true', help='Actually Publish the results to the datastore?')
    parser.add_argument('--console', action='store_true', help='Print the processed frame to console')
    parser.add_argument('--working_directory', default=working_sub_directory)
    parser.add_argument('--working_file', default='UID_ISO_FIPS_LookUp_Table.csv')

    args = parser.parse_args()

    with tracer.start_span(logger.name) as span:
        trace_command_line_arguments(span, args)

        target_file = os.path.join(args.working_directory, args.working_file)

        if args.step == 'fetch':
            logger.info(f'Fetching {args.url} -> {target_file}')

            result = requests.get(args.url)
            with open(os.path.join(target_file), 'wb') as file:
                file.write(result.content)

        elif args.step == 'load':
            logger.info(f'Loading Countries from {target_file}')

            frame: pd.DataFrame = pd.read_csv(
                target_file,
                usecols=['Country_Region', 'Province_State', 'Admin2', 'Lat', 'Long_', 'iso2', 'iso3', 'Population'])

            frame.rename(columns={
                'Lat': 'Latitude',
                'Long_': 'Longitude',
                'Admin2': 'County',
                'Country_Region': 'CountryRegion',
                'Province_State': 'ProvinceState'}, inplace=True)

            frame['ProvinceState'].fillna(value='', inplace=True)
            frame['County'].fillna(value='', inplace=True)
            frame['iso2'].fillna(value='', inplace=True)
            frame['iso3'].fillna(value='', inplace=True)
            frame.columns = list(map(snakecase.convert, frame.columns))

            logger.info('Getting Additional Country Details')

            result = requests.get(args.url_additional).json()
            additional_frame = pd.DataFrame.from_dict(result)
            additional_frame.rename(columns={ 'alpha3Code': 'iso3'}, inplace=True)
            additional_frame.drop(columns=['alpha2Code', 'name', 'population', 'latlng'], inplace=True)

            frame = frame.merge(additional_frame, how='left', on='iso3')
            frame.fillna('', inplace=True)

            if frame.empty:
                logger.exception('', ValueError('frame was empty'))

            if args.console:
                print(frame)
                print(frame.dtypes)

            if args.publish:
                logger.info(f'Writing {frame.shape[0]} rows to datastore')

                mongo = pymongo.MongoClient(COUNTRY_CONNECTION_STRING)

                database = mongo[COUNTRY_DATBASENAME]
                collection = database[COUNTRY_COLLECTIONNAME]

                logger.info('Removing Empty Fields')
                countries = frame.to_dict('records')
                for country in countries:
                    for key in list(country):
                        value = country[key]
                        if value == '':
                            del country[key]

                logger.info('Dropping %s.%s', COUNTRY_DATBASENAME, COUNTRY_COLLECTIONNAME)
                collection.drop()

                logger.info('Writing to data store')
                result = collection.insert_many(countries)

                logger.info('Acknowledged %s inserts documents into datastore', len(result.inserted_ids))

        elif args.step == 'clean':
            logger.info(f'Cleaning {working_sub_directory}')
            shutil.rmtree(working_sub_directory)

    logger.info('done')
    tracer.close()
