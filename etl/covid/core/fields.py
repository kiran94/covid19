from typing import Dict

reported_totals_map: Dict[str, str] = {
    'deaths': 'REPORTED_TOTAL_DEATHS',
    'confirmed': 'REPORTED_TOTAL_CONFIRMED',
    'recovered': 'REPORTED_TOTAL_RECOVERED'
}

reported_daily_map: Dict[str, str] = {
    'deaths': 'REPORTED_DAILY_DEATHS',
    'confirmed': 'REPORTED_DAILY_CONFIRMED',
    'recovered': 'REPORTED_DAILY_RECOVERED'
}

rolling_averages: Dict[str, Dict[str, str]] = {
    'SEVENDAY': {
        'deaths': 'ROLLING_AVERAGE_SEVENDAY_DEATHS',
        'confirmed': 'ROLLING_AVERAGE_SEVENDAY_CONFIRMED',
        'recovered': 'ROLLING_AVERAGE_SEVENDAY_RECOVERED'
    },
    'FOURTEENDAY': {
        'deaths': 'ROLLING_AVERAGE_FOURTEENDAY_DEATHS',
        'confirmed': 'ROLLING_AVERAGE_FOURTEENDAY_CONFIRMED',
        'recovered': 'ROLLING_AVERAGE_FOURTEENDAY_RECOVERED'
    },
    'TWENTYONEDAY': {
        'deaths': 'ROLLING_AVERAGE_TWENTYONEDAY_DEATHS',
        'confirmed': 'ROLLING_AVERAGE_TWENTYONEDAY_CONFIRMED',
        'recovered': 'ROLLING_AVERAGE_TWENTYONEDAY_RECOVERED'
    }
}