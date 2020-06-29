import argparse
import logging
from covid import DATABASE_CONNECTION_STRING
from covid.core.fields import reported_daily_map, rolling_averages
import pandas as pd
import sqlalchemy
from typing import List
from covid.core.tracing import tracer, trace_command_line_arguments
from covid.core import COUNTRY_INDEX

logger = logging.getLogger('covid.analytics.rollingaverages')

rolling_map = {
    'SEVENDAY': 7,
    'FOURTEENDAY': 14,
    'TWENTYONEDAY': 21
}

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

        engine = sqlalchemy.create_engine(DATABASE_CONNECTION_STRING)
        frame: pd.DataFrame = pd.read_sql(f'select * from public.timeseries where field = \'{field}\'', con=engine)

        logger.info(f'Loaded {frame.shape[0]} rows')

        grouped = frame.groupby(by=COUNTRY_INDEX)

        computed: List[pd.DataFrame] = []

        for index, group in grouped:
            logger.debug('Computing ' + str(index))

            group.set_index('date', inplace=True)
            group = group.asfreq('d')
            group['value'] = group['value'].rolling(rolling_map[args.frequency]).mean()
            group.reset_index(inplace=True)

            computed.append(group)

        result = pd.concat(computed)
        result['field'] = target_field

        if args.console:
            print(result)
            print(result.dtypes)

        if args.publish:
            logger.info(f'Writing {result.shape[0]} rows to datastore')

            with engine.begin() as connection:
                connection.execute("DELETE FROM public.timeseries WHERE field = '" + target_field + "'")
                result.to_sql('timeseries', con=connection, if_exists='append', index=False)

    tracer.close()
