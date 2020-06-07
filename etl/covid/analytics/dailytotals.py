import argparse
import logging
from covid import DATABASE_CONNECTION_STRING
from covid.core.fields import reported_totals_map, reported_daily_map
import pandas as pd
import sqlalchemy
from typing import List

logger = logging.getLogger(__name__)

group_fields = ['country_region', 'province_state']

if __name__ == "__main__":

    parser = argparse.ArgumentParser()
    parser.add_argument('--source', choices=reported_totals_map.keys(), required=True)
    parser.add_argument('--publish', action='store_true', help='Actually Publish the results to the datastore?')
    parser.add_argument('--console', action='store_true', help='Print the processed frame to console')

    args = parser.parse_args()

    field = reported_totals_map.get(args.source)
    target_field = reported_daily_map.get(args.source)

    logger.info(f'Computing Daily Totals for {field} -> {target_field}')

    engine = sqlalchemy.create_engine(DATABASE_CONNECTION_STRING)

    frame: pd.DataFrame = pd.read_sql(f'''
        select *
        from public.timeseries
        where field = \'{field}\'
    ''', con=engine)


    logger.info(f'Loaded {frame.shape[0]} rows')

    frame.fillna('', inplace=True)
    grouped = frame.groupby(by=group_fields)

    computed: List[pd.DataFrame] = []

    for index, grouped in grouped:
        logger.info('Computing ' + str(index))
        grouped['value'] = grouped['value'] - grouped['value'].shift(+1)
        grouped.loc[grouped['province_state'] == '', 'province_state'] = None

        computed.append(grouped)

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