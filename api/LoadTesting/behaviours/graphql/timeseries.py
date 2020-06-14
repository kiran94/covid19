from locust import TaskSet, task


class TimeseriesBehaviour(TaskSet):
    '''
    Simulates Request to the GraphQL Timeseries type.
    '''

    @task
    def get_timeseries_for_one_field_and_one_country(self):
        query = '''
            query request($country: [String], $province: [String], $field: [String]) {
                timeseries(country_region: $country, province_state: $province, fields: $field) {
                    date,
                    field,
                    value
                }
            }
        '''

        variables = {
            'country': 'United Kingdom',
            'province': '',
            'field': ['REPORTED_DAILY_DEATHS']
        }

        self.client.execute(
            self.get_timeseries_for_one_field_and_one_country.__name__, query, variables)

    @task
    def get_timeseries_single_company_multiple_fields(self):
        query = '''
            query request($country: [String], $province: [String], $field: [String]) {
                timeseries(country_region: $country, province_state: $province, fields: $field) {
                    date,
                    field,
                    value
                }
            }
        '''

        variables = {
            'country': 'Japan',
            'province': '',
            'field': ['REPORTED_DAILY_DEATHS', 'REPORTED_DAILY_CONFIRMED', 'REPORTED_DAILY_RECOVERED']
        }

        self.client.execute(
            self.get_timeseries_single_company_multiple_fields.__name__, query, variables)


    @task
    def get_timeseries_single_country_multiple_dates(self):
        query = '''
            query request($country: [String], $province: [String], $field: [String], $dates: [DateTime]) {
                timeseries(country_region: $country, province_state: $province, fields: $field, dates: $dates) {
                    date,
                    field,
                    value
                }
            }
        '''

        variables = {
            'country': 'Spain',
            'province': '',
            'field': ['REPORTED_DAILY_RECOVERED'],
            'dates': ['2020-04-15', '2020-04-10']
        }

        self.client.execute(self.get_timeseries_single_country_multiple_dates.__name__, query, variables)


    @task
    def get_timeseries_multiple_countries_fields_and_dates(self):
        query = '''
            query request($country: [String], $province: [String], $field: [String], $dates: [DateTime]) {
                timeseries(country_region: $country, province_state: $province, fields: $field, dates: $dates) {
                    countryRegion,
                    date,
                    field,
                    value
                }
            }
        '''

        variables = {
            'country': ["United Kingdom", "Albania"],
            'province': '',
            'field': ['REPORTED_DAILY_DEATHS', 'REPORTED_DAILY_CONFIRMED', 'REPORTED_DAILY_RECOVERED'],
            'dates': ["2020-06-08", "2020-06-07"]
        }

        self.client.execute(self.get_timeseries_multiple_countries_fields_and_dates.__name__, query, variables)