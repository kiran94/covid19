from jaeger_client import Config
from argparse import Namespace
from opentracing_instrumentation.client_hooks import install_all_patches
import logging
import os

TRACING_LOGGING_LEVEL = os.environ.get('TRACING_LOGGING_LEVEL', logging.WARNING)

logging.getLogger('opentracing_instrumentation.client_hooks.sqlalchemy').setLevel(TRACING_LOGGING_LEVEL)
logging.getLogger('opentracing_instrumentation.client_hooks.urllib2').setLevel(TRACING_LOGGING_LEVEL)
logging.getLogger('jaeger_tracing').setLevel(TRACING_LOGGING_LEVEL)

install_all_patches()

TRACING_SERVICE_NAME = "COVID-19 ETL"

config = Config(
    config={
        'sampler': {
            'type': 'const',
            'param': 1
        },
        'logging': True,
    },
    service_name=TRACING_SERVICE_NAME,
    validate=True)

tracer = config.initialize_tracer()


def trace_command_line_arguments(span, args: Namespace):
    for key, value in vars(args).items():
        span.set_tag(key, value)
