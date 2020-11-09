# COVID-19

![Python ETL](https://github.com/kiran94/covid19/workflows/Python%20ETL/badge.svg) ![.NET Core API](https://github.com/kiran94/covid19/workflows/.NET%20Core%20API/badge.svg) ![Web UI](https://github.com/kiran94/covid19/workflows/Web%20UI/badge.svg)

## Documentation

Samples:

- [GraphQL (Postman Collection)](api/Samples/COVID-19.postman_collection.json)
- [GRPC](api/Samples/Grpc/)

## Technologies Used

![SystemDiagram](docs/SystemDiagram.png)

This isn't an exhaustive list of all the technologies used in this project. It aims to provide a concise summary on some of the biggest ones and where they are used.

- `etl/`
  - `python`
  - `pandas`
- `api/`
  - `graphql api`
    - `ASP.NET Core`
  - `grpc api`
    - `ASP.NET Core`
  - `EF Core`
- `ui/`
  - `NuxtJS`
  - `Vuetify`
- `infrastructure`
  - `docker-compose`
  - `elasticsearch`
  - `kibana`
  - `opentracing` (`Jaeger`)
  - `postgres`
  - `mongo`
  - `redis`
  - `github actions`

## Building from Source

*Assume to have .NET, Python and Docker Installed. All commands are assumed to start from the root of the repostiory.*

1. Set the following Environment Variables

```sh
export ASPNETCORE_ENVIRONMENT=Development

export PGHOST=127.0.0.1
export PGUSER=postgres
export PGPASSWORD=password
export PGPORT=5432
export PGDATABASE=covid
export MONGO_USERNAME=root
export MONGO_PASSWORD=rootpassword

# Timeseries Database
export TimeseriesDatabase__ConnectionString="Server=${PGHOST};User Id=${PGUSER};Password=${PGPASSWORD};Database=${PGDATABASE}";
export TimeseriesDatabase__ConnectionString_ETL="postgres://${PGUSER}:${PGPASSWORD}@localhost/${PGDATABASE}"

# Country and Fields Database
export CountryDatabase__ConnectionString="mongodb://${MONGO_USERNAME}:${MONGO_PASSWORD}@127.0.0.1:27017"
export FieldsDatabase__ConnectionString="mongodb://${MONGO_USERNAME}:${MONGO_PASSWORD}@127.0.0.1:27018"
export CountryDatabase__DatabaseName=${PGDATABASE}
export FieldsDatabase__DatabaseName=${PGDATABASE}

# Cache
export Redis__Hosts__0__Host=localhost
export Redis__Hosts__0__Port=6379

# Logging
export ElasticSearch__Url=http://localhost:9200/
```

2. Run Containers

```sh
docker-compose up
```

3. Apply Migrations to Timeseries Database
   
```sh
cd api/Covid.Api.GraphQL
dotnet ef database update
```

4. Load Data

```sh
cd etl
python -m venv .env
source .env/bin/activate

python -m pip install -r requirements.txt

source run.sh # or look inside this script to only do a subset
```
