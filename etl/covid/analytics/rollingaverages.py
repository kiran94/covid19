import argparse
import logging
from covid import DATABASE_CONNECTION_STRING
from covid.core.fields import reported_daily_map, rolling_averages
import pandas as pd
import sqlalchemy
from typing import List
from covid.core.tracing import tracer, trace_command_line_arguments
from covid.core import COUNTRY_INDEX
from covid.analytics.common import run

logger = logging.getLogger('covid.analytics.rollingaverages')

rolling_map = {
    'SEVENDAY': 7,
    'FOURTEENDAY': 14,
    'TWENTYONEDAY': 21
}


def logic(frame: pd.DataFrame, target_field: str, **kwargs):
    '''
    Computes the Rolling Averages for the incoming DataFrame.
    '''
    grouped = frame.groupby(by=COUNTRY_INDEX)

    rolling_number = rolling_map[kwargs.get('frequency')]
    logger.info('Rolling by %s', rolling_number)

    computed: List[pd.DataFrame] = []

    for index, group in grouped:
        logger.debug('Computing ' + str(index))

        group.set_index('date', inplace=True)
        group = group.sort_index().asfreq('d')

        group['value'] = group['value'].rolling(rolling_number).mean()
        group.reset_index(inplace=True)

        computed.append(group)

    result = pd.concat(computed)
    result['field'] = target_field
    return result


if __name__ == "__main__":

    parser = argparse.ArgumentParser()
    parser.add_argument('-s', '--source', choices=reported_daily_map.keys(), required=True)
    parser.add_argument('-f', '--frequency', choices=rolling_averages.keys(), required=True)
    parser.add_argument('-p', '--publish', action='store_true', help='Actually Publish the results to the datastore?')
    parser.add_argument('-c', '--console', action='store_true', help='Print the processed frame to console')

    args = parser.parse_args()

    field = reported_daily_map.get(args.source)
    target_field = rolling_averages[args.frequency][args.source]

    with tracer.start_span(logger.name) as span:
        trace_command_line_arguments(span, args)
        span.set_tag('source_field', field)
        span.set_tag('target_field', target_field)

        logger.info(f'Computing {args.frequency} Averages for {field} -> {target_field}')

        run(field, target_field, logic, frequency=args.frequency, console=args.console, publish=args.publish)

    tracer.close()
