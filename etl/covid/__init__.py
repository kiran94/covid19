import os

import logging
from covid.core.logger import CovidLogger

logging.setLoggerClass(CovidLogger)

working_directory = 'data'

os.makedirs(working_directory, exist_ok=True)

DATABASE_USER = os.environ.get('PGUSER')
DATABASE_PASS = os.environ.get('PGPASSWORD')
DATABASE_HOST = os.environ.get('PGHOST')
DATABASE_PORT = os.environ.get('PGPORT', '5432')
DATABASE_NAME = os.environ.get('PGDATABASE')

DATABASE_CONNECTION_STRING = os.environ.get(
    'TimeseriesDatabase__ConnectionString_ETL',
    f'postgresql+pg8000://{DATABASE_USER}:{DATABASE_PASS}@{DATABASE_HOST}:{DATABASE_PORT}/{DATABASE_NAME}')

COUNTRY_CONNECTION_STRING=os.environ.get('CountryDatabase__ConnectionString')
COUNTRY_DATBASENAME=os.environ.get('CountryDatabase__DatabaseName', 'covid')
COUNTRY_COLLECTIONNAME=os.environ.get('CountryDatabase__Collection', 'countries')

FIELDS_CONNECTION_STRING=os.environ.get('FieldsDatabase__ConnectionString')
FIELDS_DATABASENAME=os.environ.get('FieldsDatabase__DatabaseName', 'covid')
FIELDS_COLLECTIONNAME=os.environ.get('FieldsDatabase__Collection', 'fields')
