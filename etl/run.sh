#!/bin/bash
set -e
# Normally these steps would sit inside a scheduler,
# but it's useful to have these steps here so I can quickly
# run an entire refresh
# It also gives a good high level picture on the proceseses involved

# Fetch Latest Countries
python -m covid.ingest.countries -s fetch
python -m covid.ingest.countries -s load --publish
python -m covid.ingest.countries -s clean

# Fetch Latest Timeseries
python -m covid.ingest.timeseries -s fetch --source deaths
python -m covid.ingest.timeseries -s load --source deaths --publish

python -m covid.ingest.timeseries -s fetch --source confirmed
python -m covid.ingest.timeseries -s load --source confirmed --publish

python -m covid.ingest.timeseries -s fetch --source recovered
python -m covid.ingest.timeseries -s load --source recovered --publish
python -m covid.ingest.timeseries -s clean --source recovered

# Daily Totals
python -m covid.analytics.dailytotals --source deaths --publish
python -m covid.analytics.dailytotals --source confirmed --publish
python -m covid.analytics.dailytotals --source recovered --publish

# Rolling Averages
python -m covid.analytics.rollingaverages -s deaths -f SEVENDAY --publish
python -m covid.analytics.rollingaverages -s deaths -f FOURTEENDAY --publish
python -m covid.analytics.rollingaverages -s deaths -f TWENTYONEDAY --publish

python -m covid.analytics.rollingaverages -s confirmed -f SEVENDAY --publish
python -m covid.analytics.rollingaverages -s confirmed -f FOURTEENDAY --publish
python -m covid.analytics.rollingaverages -s confirmed -f TWENTYONEDAY --publish

python -m covid.analytics.rollingaverages -s recovered -f SEVENDAY --publish
python -m covid.analytics.rollingaverages -s recovered -f FOURTEENDAY --publish
python -m covid.analytics.rollingaverages -s recovered -f TWENTYONEDAY --publish

# Population Ratios
python -m covid.analytics.populationratios --source deaths --publish
python -m covid.analytics.populationratios --source confirmed --publish
python -m covid.analytics.populationratios --source recovered --publish