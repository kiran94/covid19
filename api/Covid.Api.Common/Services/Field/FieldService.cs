namespace Covid.Api.Common.Services.Field
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Responsible for providing field information.
    /// </summary>
    public interface IFieldService
    {
        Task<IQueryable<Field>> GetAll();
    }

    public class FieldService : IFieldService
    {
        // TODO: Temporary layer of abstraction until we have a Field Service operational
        private static List<Field> inMemoryFields = new List<Field>()
        {
            // Reported Totals
            new Field() { ID = "REPORTED_TOTAL_CONFIRMED", Description = "Gets the Reported Total Confirmed Cases", Color = "#EF6C00" },
            new Field() { ID = "REPORTED_TOTAL_DEATHS", Description = "Gets the Reported Total Deaths Cases", Color = "#B71C1C" },
            new Field() { ID = "REPORTED_TOTAL_RECOVERED", Description = "Gets the Reported Total Recovered Cases", Color = "#1565C0" },

            // Reported Daily
            new Field() { ID = "REPORTED_DAILY_CONFIRMED", Description = "Gets the Reported Daily Confirmed", Color = "#F57C00" },
            new Field() { ID = "REPORTED_DAILY_DEATHS", Description = "Gets the Reported Daily Deaths", Color = "#C62828" },
            new Field() { ID = "REPORTED_DAILY_RECOVERED", Description = "Gets the Reported Daily Recovered", Color = "#1976D2" },

            // Daily Percent Increases
            new Field() { ID = "DAILY_PERCENT_INCREASE_CONFIRMED", Description = "Gets the Daily Percent increase of Confirmed cases", Color = "#FB8C00" },
            new Field() { ID = "DAILY_PERCENT_INCREASE_DEATHS", Description = "Gets the Daily Percent increase of Deaths", Color = "#D32F2F" },
            new Field() { ID = "DAILY_PERCENT_INCREASE_RECOVERED", Description = "Gets the Daily Percent increase of Recovered cases", Color = "#1E88E5" },

            // Population Ratios
            new Field() { ID = "POPULATION_CONFIRMED_RATIO", Description = "Gets the Confirmed/Population Ratio", Color = "#FFA726" },
            new Field() { ID = "POPULATION_DEATH_RATIO", Description = "Gets the Deaths/Population Ratio", Color = "#E53935" },
            new Field() { ID = "POPULATION_RECOVERED_RATIO", Description = "Gets the Recovered/Population Ratio", Color = "#42A5F5" },

            // Rolling Seven Day
            new Field() { ID = "ROLLING_AVERAGE_SEVENDAY_CONFIRMED", Description = "Gets the 7-Day Rolling Average of Confirmed Cases", Color = "#FF6F00" },
            new Field() { ID = "ROLLING_AVERAGE_SEVENDAY_DEATHS", Description = "Gets the 7-Day Rolling Average of Deaths Cases", Color = "#FF1744" },
            new Field() { ID = "ROLLING_AVERAGE_SEVENDAY_RECOVERED", Description = "Gets the 7-Day Rolling Average of Recovered Cases", Color = "#1A237E" },

            // Rolling Fourteen Day
            new Field() { ID = "ROLLING_AVERAGE_FOURTEENDAY_CONFIRMED", Description = "Gets the 14-Day Rolling Average of Confirmed Cases", Color = "#FF8F00" },
            new Field() { ID = "ROLLING_AVERAGE_FOURTEENDAY_DEATHS", Description = "Gets the 14-Day Rolling Average of Deaths Cases", Color = "#FF5252" },
            new Field() { ID = "ROLLING_AVERAGE_FOURTEENDAY_RECOVERED", Description = "Gets the 14-Day Rolling Average of Recovered Cases", Color = "#283593" },

            // Rolling Twenty One Day
            new Field() { ID = "ROLLING_AVERAGE_TWENTYONEDAY_CONFIRMED", Description = "Gets the 21-Day Rolling Average of Confirmed Cases", Color = "#FFA000" },
            new Field() { ID = "ROLLING_AVERAGE_TWENTYONEDAY_DEATHS", Description = "Gets the 21-Day Rolling Average of Deaths Cases", Color = "#FF8A80" },
            new Field() { ID = "ROLLING_AVERAGE_TWENTYONEDAY_RECOVERED", Description = "Gets the 21-Day Rolling Average of Recovered Cases", Color = "#303F9F" },
        };

        public async Task<IQueryable<Field>> GetAll() => await Task.FromResult(FieldService.inMemoryFields.AsQueryable());
    }
}