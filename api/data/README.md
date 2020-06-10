# Covid API

- [Covid API](#covid-api)
  - [GraphQL](#graphql)
    - [Example Queries](#example-queries)
      - [What countries do you cover?](#what-countries-do-you-cover)
      - [What details do you have on Vietnam?](#what-details-do-you-have-on-vietnam)
      - [What Provinces do you cover in China?](#what-provinces-do-you-cover-in-china)
      - [What are the reported deaths for the UK throughout the pandemic?](#what-are-the-reported-deaths-for-the-uk-throughout-the-pandemic)
      - [What are the reported deaths, confirmed, covered in the Japan throughout the pandemic?](#what-are-the-reported-deaths-confirmed-covered-in-the-japan-throughout-the-pandemic)
      - [What are the reported recovered cases in Spain for these dates?](#what-are-the-reported-recovered-cases-in-spain-for-these-dates)
      - [Multiple Countries, Fields and Dates](#multiple-countries-fields-and-dates)
      - [Pagination](#pagination)

## GraphQL

### Example Queries

#### What countries do you cover?

```graphql
query {
  countries(skip:0, take:5) {
    countryRegion,
    provinceState,
    county,
    latitude,
    longitude,
    population
  }
}
```

#### What details do you have on Vietnam?

```graphql
query {
  countries(query: "Vietnam") {
    latitude,
    longitude,
    population
  }
}
```

#### What Provinces do you cover in China?

```graphql
query {
  countries(query: "China") {
		provinceState
  }
}
```

#### What are the reported deaths for the UK throughout the pandemic?

```graphql
query {
  timeseries(
    country_region: "United Kingdom"
    province_state: ""
    fields: "REPORTED_DAILY_DEATHS"
  ) {
    date
    field
    value
  }
}
```

#### What are the reported deaths, confirmed, covered in the Japan throughout the pandemic?

```graphql
query {
  timeseries(
    country_region: "Japan"
    province_state: "",
    fields: [
      "REPORTED_DAILY_CONFIRMED"
      "REPORTED_DAILY_DEATHS"
      "REPORTED_DAILY_RECOVERED"
    ]
  ) {
    date
    field
    value
  }
}
```

#### What are the reported recovered cases in Spain for these dates?

```graphql
query {
  timeseries(
    country_region: "Spain"
    province_state: "",
    fields: [
      "REPORTED_DAILY_RECOVERED"
    ],
    dates: [
      "2020-04-15",
      "2020-04-10"
    ]
  ) {
    date
    field
    value
  }
}
```

#### Multiple Countries, Fields and Dates

```graphql
query {
  timeseries(
    country_region: ["United Kingdom", "Albania"]
    province_state: "",
    fields: [
      "REPORTED_DAILY_CONFIRMED"
      "REPORTED_DAILY_DEATHS"
      "REPORTED_DAILY_RECOVERED"
    ],
    dates: ["2020-06-08", "2020-06-07"]
  ) {
    countryRegion
    date
    field
    value
  }
}
```

#### Pagination

```graphql
query {
  timeseries(
    country_region: "US",
    province_state: "",
    fields: [
      "REPORTED_DAILY_RECOVERED"
    ],
    take: 5
    skip: 10
  ) {
    countryRegion
    date
    field
    value
  }
}
```