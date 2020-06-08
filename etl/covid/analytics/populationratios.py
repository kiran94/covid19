import argparse
import logging
from covid import DATABASE_CONNECTION_STRING
from covid.core.fields import reported_totals_map, reported_daily_map, population_ratio
import pandas as pd
import sqlalchemy
from typing import List
from covid.core.tracing import tracer, trace_command_line_arguments

logger = logging.getLogger(__name__)

if __name__ == "__main__":

    parser = argparse.ArgumentParser()
    parser.add_argument('--source', choices=reported_totals_map.keys(), required=True)
    parser.add_argument('--publish', action='store_true', help='Actually Publish the results to the datastore?')
    parser.add_argument('--console', action='store_true', help='Print the processed frame to console')

    args = parser.parse_args()
    source_field = reported_totals_map[args.source]
    target_field = population_ratio[args.source]

    with tracer.start_span('covid.analytics.populationratios') as span:
        trace_command_line_arguments(span, args)

        span.set_tag('source_field', source_field)
        span.set_tag('target_field', target_field)

        logger.info(f'Computing Population Ratios for {source_field} -> {target_field}')

        engine = sqlalchemy.create_engine(DATABASE_CONNECTION_STRING)

        logger.info('Getting Timeseries and Country Information')
        timeseries = pd.read_sql(f'select * from public.timeseries where field = \'{source_field}\'', con=engine)
        countries = pd.read_sql('select * from public.countries', con=engine)

        countries = countries[['country_region', 'province_state', 'population']]
        index = ['country_region', 'province_state']

        logger.info('Stitching Population to Timeseries')
        timeseries = timeseries.merge(countries, how='inner', on=index)

        logger.info(f'Computing {args.source} / Population Ratio')

        timeseries['field'] = target_field
        timeseries['value'] = timeseries['value'] / timeseries['population']

        timeseries.drop(columns='population', inplace=True)

        if args.console:
            print(timeseries)
            print(timeseries.dtypes)

        if args.publish:
            logger.info(f'Writing {timeseries.shape[0]} rows to datastore')

            with engine.begin() as connection:
                connection.execute("DELETE FROM public.timeseries WHERE field = '" + target_field + "'")
                timeseries.to_sql('timeseries', con=connection, if_exists='append', index=False)

    tracer.close()