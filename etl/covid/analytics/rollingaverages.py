import argparse
import logging
from covid import DATABASE_CONNECTION_STRING
from covid.core.fields import reported_daily_map, rolling_averages
import pandas as pd
import sqlalchemy
from typing import List

logger = logging.getLogger(__name__)

group_fields = ['country_region', 'province_state']

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

    logger.info(f'Computing {args.frequency} Averages for {field} -> {target_field}')

    engine = sqlalchemy.create_engine(DATABASE_CONNECTION_STRING)
    frame: pd.DataFrame = pd.read_sql(f'select * from public.timeseries where field = \'{field}\'', con=engine)

    logger.info(f'Loaded {frame.shape[0]} rows')

    frame['province_state'].fillna('', inplace=True)
    grouped = frame.groupby(by=group_fields)

    computed: List[pd.DataFrame] = []

    for index, group in grouped:
        logger.debug('Computing ' + str(index))

        group.sort_values(by='date', inplace=True)

        group['value'] = group['value'].rolling(rolling_map[args.frequency]).mean()
        group.loc[group['province_state'] == '', 'province_state'] = None

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