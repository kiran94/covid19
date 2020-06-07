import os
import covid.logger

working_directory = 'data'

os.makedirs(working_directory, exist_ok=True)

DATABASE_USER=os.environ.get('POSTGRES_USER')
DATABASE_PASS=os.environ.get('POSTGRES_PASSWORD')
DATABASE_HOST=os.environ.get('POSTGRES_HOST')
DATABASE_PORT=os.environ.get('POSTGRES_PORT', '5432')
DATABASE_NAME=os.environ.get('COVID_DATABASE_NAME')

DATABASE_CONNECTION_STRING = os.environ.get(
    'DATABASE_CONNECTION_STRING',
    f'postgresql+pg8000://{DATABASE_USER}:{DATABASE_PASS}@{DATABASE_HOST}:{DATABASE_PORT}/{DATABASE_NAME}')