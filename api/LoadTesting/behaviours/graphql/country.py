from locust import TaskSet, task

class CountryBehaviour(TaskSet):
    '''
    Simulates Request to the GraphQL Country type.
    '''

    @task
    def get_all_countries(self):
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

        self.client.execute(self.get_all_countries.__name__, query)

    @task
    def get_all_countries_paginate(self):
        query = '''
            query request($skip: Int, $take: Int) {
                countries(skip: $skip, take: $take) {
                    countryRegion,
                    provinceState,
                    county,
                    latitude,
                    longitude,
                    population
                }
            }
        '''

        variables = {
            'skip': 0,
            'take': 10
        }

        self.client.execute(self.get_all_countries_paginate.__name__, query, variables)

    @task
    def get_single_country(self):
        query = '''
            query request($country: String) {
            countries(query: $country) {
                    countryRegion
                    provinceState
                    county
                    latitude
                    longitude
                    population
                }
            }
        '''

        variables = {
            'country': "United Kingdom"
        }

        self.client.execute(self.get_single_country.__name__, query, variables)




