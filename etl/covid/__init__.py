import os

import logging
from covid.core.logger import CovidLogger

logging.setLoggerClass(CovidLogger)

working_directory = 'data'

os.makedirs(working_directory, exist_ok=True)

DATABASE_USER = os.environ.get('POSTGRES_USER')
DATABASE_PASS = os.environ.get('POSTGRES_PASSWORD')
DATABASE_HOST = os.environ.get('POSTGRES_HOST')
DATABASE_PORT = os.environ.get('POSTGRES_PORT', '5432')
DATABASE_NAME = os.environ.get('COVID_DATABASE_NAME')

DATABASE_CONNECTION_STRING = os.environ.get(
    'DATABASE_CONNECTION_STRING',
    f'postgresql+pg8000://{DATABASE_USER}:{DATABASE_PASS}@{DATABASE_HOST}:{DATABASE_PORT}/{DATABASE_NAME}')

MONGO_HOST=os.environ.get('MONGO_HOST')
MONGO_PORT=int(os.environ.get('MONGO_PORT'))
MONGO_USERNAME=os.environ.get('MONGO_INITDB_ROOT_USERNAME')
MONGO_PASSWORD=os.environ.get('MONGO_INITDB_ROOT_PASSWORD')

COUNTRY_DATBASENAME=os.environ.get('COVID_COUNTRY_DATABASE_NAME', 'covid')
COUNTRY_COLLECTIONNAME=os.environ.get('COVID_COUNTRY_COLLECTIONNAME', 'countries')

COUNTRY_CONNECTION_STRING=os.environ.get(
    'COVID_COUNTRY_CONNECTION_STRING',
    f'mongodb://{MONGO_USERNAME}:{MONGO_PASSWORD}@{MONGO_HOST}:{MONGO_PORT}/?authSource=admin&readPreference=primary&ssl=false'
)

