import argparse
import logging
import pandas as pd
import os
import requests
import shutil
from sqlalchemy import create_engine
import snakecase
import pymongo

from covid import working_directory, FIELDS_CONNECTION_STRING, FIELDS_COLLECTIONNAME, FIELDS_DATABASENAME
from covid.core.tracing import tracer, trace_command_line_arguments

working_sub_directory = os.path.join(working_directory, 'fields')
os.makedirs(working_sub_directory, exist_ok=True)

logger = logging.getLogger('covid.ingest.fields')

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument('-s', '--step', choices=['refresh'], required=True)
    parser.add_argument('--publish', action='store_true', help='Actually Publish the results to the datastore?')
    parser.add_argument('--console', action='store_true', help='Print the processed frame to console')
    parser.add_argument('--working_directory', default=working_sub_directory)
    parser.add_argument('--working_file', default='fields.csv')
    parser.add_argument('--target_database', default=FIELDS_CONNECTION_STRING, help='Target Database to write results')

    args = parser.parse_args()

    with tracer.start_span(logger.name) as span:
        trace_command_line_arguments(span, args)

        if args.step == 'refresh':
            target_file = os.path.join(args.working_directory, args.working_file)

            logger.info('Refreshing Fields from %s', target_file)
            frame = pd.read_csv(target_file)

            logger.info('Found Frame %s rows, %s columns', frame.shape[0], frame.shape[1])
            logger.debug('Frame: \n %s', frame)

            if args.console:
                print(frame)
                print(frame.dtypes)

            if args.publish:
                mongo = pymongo.MongoClient(FIELDS_CONNECTION_STRING)
                database = mongo[FIELDS_DATABASENAME]
                collection = database[FIELDS_COLLECTIONNAME]

                logger.info('Dropping %s.%s', FIELDS_DATABASENAME, FIELDS_COLLECTIONNAME)
                collection.drop()

                result = collection.insert_many(frame.to_dict('records'))
                logger.info('Acknowledged %s inserts documents into datastore', len(result.inserted_ids))

    logger.info('done')
    tracer.close()