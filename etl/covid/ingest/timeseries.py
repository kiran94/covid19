
import argparse
import logging
import pandas as pd
import os
import requests
import shutil
from sqlalchemy import create_engine
# from datetime import datetime


from covid import working_directory, DATABASE_CONNECTION_STRING
from covid.core.fields import reported_totals_map
from covid.core.tracing import tracer, trace_command_line_arguments

working_sub_directory = os.path.join(working_directory, 'timeseries')
os.makedirs(working_sub_directory, exist_ok=True)

logger = logging.getLogger('covid.ingest.timeseries')

files = {
    'deaths': 'https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_deaths_global.csv', # noqa
    'confirmed': 'https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv', # noqa
    'recovered': 'https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_recovered_global.csv' # noqa
}

if __name__ == "__main__":

    parser = argparse.ArgumentParser()
    parser.add_argument('-s', '--step', choices=['fetch', 'load', 'clean'], required=True)
    parser.add_argument('--source', choices=files.keys(), required=True)
    parser.add_argument('--publish', action='store_true', help='Actually Publish the results to the datastore?')
    parser.add_argument('--console', action='store_true', help='Print the processed frame to console')
    parser.add_argument('--working_directory', default=working_sub_directory)
    parser.add_argument('--target_database', default=DATABASE_CONNECTION_STRING, help='Target Database to write results')

    args = parser.parse_args()
    url = files.get(args.source)
    target_file = os.path.join(args.working_directory, args.source + '.csv')

    with tracer.start_span(logger.name) as span:
        trace_command_line_arguments(span, args)
        span.set_tag('url', url)
        span.set_tag('target_file', target_file)

        if (args.step == 'fetch'):
            logger.info(f'Fetch {url} -> {target_file}')

            result = requests.get(url)
            with open(target_file, 'wb') as file:
                file.write(result.content)

        elif args.step == 'load':
            logger.info(f'Loading {target_file}')

            frame: pd.DataFrame = pd.read_csv(target_file)

            logger.info('Formatting Columns')
            frame.drop(columns=['Lat', 'Long'], inplace=True)
            frame.rename(columns={'Province/State': 'province_state', 'Country/Region': 'country_region'}, inplace=True)
            frame['province_state'].fillna(value='', inplace=True)

            if frame.empty:
                logger.exception('', ValueError('frame was empty'))

            logger.info('Unpivoting Date Columns')
            pivoted = frame.melt(id_vars=['country_region', 'province_state'], var_name='date', value_name='value')

            pivoted['field'] = reported_totals_map.get(args.source)
            pivoted['date'] = pivoted['date'].astype('datetime64')
            pivoted['value'] = pivoted['value'].astype('float64')
            pivoted['county'] = ''
            pivoted = pivoted[['country_region', 'province_state', 'county', 'field', 'date', 'value']]

            # pivoted['is_updated'] = True # should be used when switching to incremental loads
            # pivoted['updated_at'] = datetime.utcnow()

            if args.console:
                print(pivoted)

            if args.publish:
                logger.info(f'Writing {pivoted.shape[0]} rows to datastore')

                engine = create_engine(args.target_database)

                with engine.begin() as connection:
                    connection.execute("DELETE FROM public.timeseries WHERE field = '" + reported_totals_map.get(args.source) + "'")
                    pivoted.to_sql('timeseries', con=connection, if_exists='append', index=False)

        elif args.step == 'clean':
            logger.info(f'Cleaning {working_sub_directory}')
            shutil.rmtree(working_sub_directory)

    tracer.close()
