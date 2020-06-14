import argparse
import logging
import os
import ssl

import gevent
from locust import HttpUser, between, events, task
from locust.env import Environment
from locust.log import setup_logging
from locust.stats import (print_stats, requests_csv, stats_printer,
                          write_csv_files, RequestStats)

from behaviours.graphql.country import CountryBehaviour
from behaviours.graphql.timeseries import TimeseriesBehaviour
from client.graphql import GraphQLLocust

if (not os.environ.get('PYTHONHTTPSVERIFY', '') and getattr(ssl, '_create_unverified_context', None)):
    ssl._create_default_https_context = ssl._create_unverified_context


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument('-ho', '--host', default='https://localhost:5001/graphql')
    parser.add_argument('--wait-time-min', default=1, type=int, help='Default minimum wait time between tasks')
    parser.add_argument('--wait-time-max', default=10, type=int, help='Default maximum wait time between tasks')
    parser.add_argument('-w', '--web', action='store_true', help='Enable Web UI')
    parser.add_argument('-wh', '--web-host', default='127.0.0.1', help='Address to host the Web UI')
    parser.add_argument('-wp', '--web-port', default=8089, type=int, help='Port to host the Web UI')
    parser.add_argument('-u', '--number-of-users', default=10, type=int, help='Number of Users to start')
    parser.add_argument('-uh', '--user-hatch-rate', default=5, type=int, help='Number of Users to spawn per second')
    parser.add_argument('-d', '--duration', default=60, type=int, help='Duration of the load test (in seconds)')
    parser.add_argument('-o', '--output_directory', default='results', type=str, help='Output directory to put results')
    parser.add_argument('-f', '--print_final_statistics', action='store_true', default=True, help='Print the final statistics before exit')
    parser.add_argument('-v', '--verbosity', default="INFO", type=str, help='Set the logging level', choices=['DEBUG', 'INFO', 'WARN', 'ERROR'])

    args = parser.parse_args()

    setup_logging(args.verbosity)
    logger = logging.getLogger('covid.load.testing')

    logger.info(args)

    print(args)
    logger.info('Initialising Load Testing')

    class User(GraphQLLocust):
        host = args.host
        tasks = [CountryBehaviour, TimeseriesBehaviour]
        wait_time = between(args.wait_time_min, args.wait_time_max)

    logger.info('Setting up Environment and Runner')
    env = Environment(user_classes=[User], events=events)
    env.create_local_runner()

    if args.web:
        logger.info(f'Launching Web UI in {args.web_host}:{args.web_port}')
        env.create_web_ui(args.web_host, args.web_port)

    gevent.spawn(stats_printer(env.stats))

    logger.info(f'Starting Run with {args.number_of_users} users with hatch rate {args.user_hatch_rate}')
    env.runner.start(args.number_of_users, hatch_rate=args.user_hatch_rate)

    gevent.spawn_later(args.duration, lambda: env.runner.quit())
    env.runner.greenlet.join()

    if args.web:
        logger.debug('Stopping Web UI')
        env.web_ui.stop()

    logger.info(f'Writing results to output directory {args.output_directory}')
    os.makedirs(args.output_directory, exist_ok=True)
    write_csv_files(env, os.path.join(args.output_directory, 'result'), full_history=True)

    if args.print_final_statistics:
        stats_printer(env.stats)