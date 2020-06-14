import gevent
from locust import HttpUser, task, between, events
from locust.env import Environment
from locust.stats import stats_printer, write_csv_files, print_stats, requests_csv
from locust.log import setup_logging
from behaviours.graphql_country import GraphQLCountryBehaviour
from client.graphql import GraphQLLocust
import os, ssl

if (not os.environ.get('PYTHONHTTPSVERIFY', '') and getattr(ssl, '_create_unverified_context', None)):
    ssl._create_default_https_context = ssl._create_unverified_context

setup_logging("DEBUG")

class User(GraphQLLocust):
    host = "https://localhost:5001/graphql"
    tasks = [GraphQLCountryBehaviour]
    wait_time = between(1, 10)



# setup Environment and Runner
env = Environment(user_classes=[User], events=events)
env.create_local_runner()

# start a WebUI instance
env.create_web_ui("127.0.0.1", 8089)

# start a greenlet that periodically outputs the current stats
gevent.spawn(stats_printer(env.stats))

# start the test
number_of_users=10
hatch_user_rate=5
env.runner.start(number_of_users, hatch_rate=hatch_user_rate)

# in 60 seconds stop the runner
gevent.spawn_later(60, lambda: env.runner.quit())

# wait for the greenlets
env.runner.greenlet.join()

# stop the web server for good measures
env.web_ui.stop()

# env.stats.write_stat_csvs('Example')

write_csv_files(env, 'load', full_history=True)
hello = requests_csv(env.stats)
print(hello)
# print_stats(env.stats)