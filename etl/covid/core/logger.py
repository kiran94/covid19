import logging
from rainbow_logging_handler import RainbowLoggingHandler
import sys


class CovidLogger(logging.Logger):

    def __init__(self,
                 name: str,
                 level: int = logging.INFO,
                 format='%(asctime)s - %(name)s - %(levelname)s - %(message)s'):
        super().__init__(name, level)

        formatter = logging.Formatter(format)

        handler = RainbowLoggingHandler(sys.stderr, color_funcName=('black', 'yellow', True))
        handler.setFormatter(formatter)
        self.addHandler(handler)


logging.getLogger('sqlalchemy.engine.base.Engine').setLevel(logging.WARNING)
