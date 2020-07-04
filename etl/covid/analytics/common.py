import argparse
import logging
from covid import DATABASE_CONNECTION_STRING
from covid.core.fields import reported_totals_map, reported_daily_map
import pandas as pd
import sqlalchemy
from typing import List, Callable
from covid.core.tracing import tracer, trace_command_line_arguments
from covid.core import COUNTRY_INDEX

logger = logging.getLogger(__name__)


def run(
        source_field: str,
        target_field: str,
        logic: Callable[[pd.DataFrame, str], pd.DataFrame],
        source_table_schema="public",
        source_table_name: str = "timeseries",
        target_table_schema="public",
        target_table_name: str = "timeseries",
        **kwargs):
    '''
    Runs Common Flow for Analytics logic

    :param kwargs
        source_table_schema(str): The schema of the table to pull data from
        source_table_name(str): The name of the table to pull data from
        target_table_schema(str): The schema of the table to write data back to
        target_table_name(str): The name of the table to write data back to
        console(bool): Print the computed data to console
        publish(bool): Publish the data back to the target table
    '''
    logger.info('Computing %s -> %s', source_field, target_field)

    logger.debug('Parsing kwargs')
    source_table_schema = kwargs.get("source_table_schema", "public")
    target_table_schema = kwargs.get("target_table_schema", "public")
    source_table_name = kwargs.get("source_table_name", "timeseries")
    target_table_name = kwargs.get("target_table_name", "timeseries")
    source_table = f'{source_table_schema}.{source_table_name}'
    target_table = f'{target_table_schema}.{target_table_name}'
    console = kwargs.get('console', False)
    publish = kwargs.get('publish', False)

    logger.debug('Create Sql Alchemy Engine')
    engine = sqlalchemy.create_engine(DATABASE_CONNECTION_STRING)

    logger.info('Retrieving Data from %s', source_table)
    frame: pd.DataFrame = pd.read_sql(f'SELECT * FROM {source_table} WHERE field = \'{source_field}\'', con=engine)

    logger.info('Applying Logic on %s datapoints', frame.shape[0])

    result: pd.DataFrame = logic(frame, target_field)

    if console:
        print(result)
        print(result.dtypes)

    if kwargs.get('publish', False):
        logger.info(f'Writing {result.shape[0]} datapoints to datastore')

        with engine.begin() as connection:
            connection.execute("DELETE FROM " + target_table + " WHERE field = '" + target_field + "'")
            result.to_sql(target_table_name, schema=target_table_schema, con=connection, if_exists='append', index=False)
    else:
        logger.debug('Publish flag was not set')
