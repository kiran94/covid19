
import argparse
import logging
import pandas as pd
import os
import requests
import shutil
from sqlalchemy import create_engine
import snakecase
from datetime import datetime


from covid import working_directory, DATABASE_CONNECTION_STRING
from covid.core.tracing import tracer, trace_command_line_arguments

working_sub_directory = os.path.join(working_directory, 'countries')
os.makedirs(working_sub_directory, exist_ok=True)

logger = logging.getLogger(__package__)

if __name__ == "__main__":

    parser = argparse.ArgumentParser()
    parser.add_argument('-s', '--step', choices=['fetch', 'load', 'clean'], required=True)
    parser.add_argument('-u', '--url',
                        type=str,
                        default='https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/UID_ISO_FIPS_LookUp_Table.csv',
                        help='URL of the data source (in CSV format)')
    parser.add_argument('--publish', action='store_true', help='Actually Publish the results to the datastore?')
    parser.add_argument('--console', action='store_true', help='Print the processed frame to console')
    parser.add_argument('--working_directory', default=working_sub_directory)
    parser.add_argument('--working_file', default='UID_ISO_FIPS_LookUp_Table.csv')
    parser.add_argument('--target_database', default=DATABASE_CONNECTION_STRING, help='Target Database to write results')

    args = parser.parse_args()

    with tracer.start_span('covid.countries') as span:
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
                usecols=['Country_Region', 'Province_State', 'Lat', 'Long_', 'iso2', 'iso3', 'Population'])

            frame.rename(columns={'Long_': 'Long', 'Country_Region': 'CountryRegion', 'Province_State': 'ProvinceState'}, inplace=True)
            frame.columns = list(map(snakecase.convert, frame.columns))
            frame['is_updated'] = True  # should be used when switching to incremental loads
            frame['updated_at'] = datetime.utcnow()

            if frame.empty:
                logger.exception('', ValueError('frame was empty'))

            if args.console:
                print(frame)
                print(frame.dtypes)

            if args.publish:
                logger.info(f'Writing {frame.shape[0]} rows to datastore')

                engine = create_engine(args.target_database)

                with engine.begin() as connection:
                    connection.execute('TRUNCATE TABLE public.countries')
                    frame.to_sql('countries', con=connection, if_exists='append', index=False)

        elif args.step == 'clean':
            logger.info(f'Cleaning {working_sub_directory}')
            shutil.rmtree(working_sub_directory)

    logger.info('done')
    tracer.close()
