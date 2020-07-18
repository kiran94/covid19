import grpc

from covid_pb2 import CovidRequest, AbsoluteDateRequest
from covid_pb2_grpc import CovidServiceStub
from datetime import datetime

channel = grpc.insecure_channel('0.0.0.0:6000')
client = CovidServiceStub(channel)

request = CovidRequest(
    CountryRegion='United Kingdom',
    ProvinceState='',
    County='',
    Fields=['REPORTED_DAILY_DEATHS'],
    absoluteDates=AbsoluteDateRequest(
        Dates=[
            {"seconds": int(datetime(2020, 6, 1, 0, 0).timestamp())},
            {"seconds": int(datetime(2020, 6, 2, 0, 0).timestamp())}
        ]
    ))

response = client.Get(request)
print(response)