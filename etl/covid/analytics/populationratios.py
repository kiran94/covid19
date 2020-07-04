import argparse
import logging
from covid import DATABASE_CONNECTION_STRING
from covid.core.fields import reported_totals_map, population_ratio
import pandas as pd
import sqlalchemy
from covid.core.tracing import tracer, trace_command_line_arguments
from covid.core import COUNTRY_INDEX
from covid.analytics.common import run

logger = logging.getLogger('covid.analytics.populationratios')


def logic(frame: pd.DataFrame, target_field: str, **kwargs):
    '''
    Computes the Rolling Averages for the incoming DataFrame.
    '''
    timeseries = frame
    countries = kwargs.get('countries')

    countries = countries[['country_region', 'province_state', 'county', 'population']]

    logger.info('Stitching Population to Timeseries')
    timeseries = timeseries.merge(countries, how='inner', on=COUNTRY_INDEX)

    logger.info(f'Computing {args.source} / Population Ratio')

    timeseries['field'] = target_field
    timeseries['value'] = timeseries['value'] / timeseries['population']

    timeseries.drop(columns='population', inplace=True)
    return timeseries


if __name__ == "__main__":

    parser = argparse.ArgumentParser()
    parser.add_argument('--source', choices=reported_totals_map.keys(), required=True)
    parser.add_argument('--publish', action='store_true', help='Actually Publish the results to the datastore?')
    parser.add_argument('--console', action='store_true', help='Print the processed frame to console')

    args = parser.parse_args()
    source_field = reported_totals_map[args.source]
    target_field = population_ratio[args.source]

    with tracer.start_span(logger.name) as span:
        trace_command_line_arguments(span, args)

        span.set_tag('source_field', source_field)
        span.set_tag('target_field', target_field)

        run(source_field, target_field, logic, load_country=True, console=args.console, publish=args.publish)

    tracer.close()
