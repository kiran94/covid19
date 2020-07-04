import argparse
import logging
from covid import DATABASE_CONNECTION_STRING
from covid.core.fields import reported_totals_map, reported_daily_map
import pandas as pd
import sqlalchemy
from typing import List
from covid.core.tracing import tracer, trace_command_line_arguments
from covid.core import COUNTRY_INDEX

from covid.analytics.common import run

logger = logging.getLogger('covid.analytics.dailytotals')

def logic(frame: pd.DataFrame, target_field: str):
    '''
    Computes the Daily Totals for the incoming DataFrame.
    '''
    grouped = frame.groupby(by=COUNTRY_INDEX)
    computed: List[pd.DataFrame] = []

    for index, group in grouped:
        logger.debug('Computing ' + str(index))

        group.set_index('date', inplace=True)
        group = group.sort_index().asfreq('d')

        group['value'] = group['value'] - group['value'].shift(+1, fill_value=0)
        group.reset_index(inplace=True)

        computed.append(group)

    result = pd.concat(computed)
    result['field'] = target_field
    return result

if __name__ == "__main__":

    parser = argparse.ArgumentParser()
    parser.add_argument('--source', choices=reported_totals_map.keys(), required=True)
    parser.add_argument('--publish', action='store_true', help='Actually Publish the results to the datastore?')
    parser.add_argument('--console', action='store_true', help='Print the processed frame to console')

    args = parser.parse_args()

    field = reported_totals_map.get(args.source)
    target_field = reported_daily_map.get(args.source)

    with tracer.start_span(logger.name) as span:
        trace_command_line_arguments(span, args)
        span.set_tag('source_field', field)
        span.set_tag('target_field', target_field)

        run(field, target_field, logic, console=args.console, publish=args.publish)

    tracer.close()
