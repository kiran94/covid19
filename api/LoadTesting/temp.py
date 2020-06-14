from graphqlclient import GraphQLClient


import os, ssl

if (not os.environ.get('PYTHONHTTPSVERIFY', '') and
getattr(ssl, '_create_unverified_context', None)):
    ssl._create_default_https_context = ssl._create_unverified_context


query = '''
    query request {
        countries {
            countryRegion,
            provinceState,
            county,
            latitude,
            longitude,
            population
        }
    }
'''

g = GraphQLClient('https://localhost:5001/graphql')

print(g.execute(query))