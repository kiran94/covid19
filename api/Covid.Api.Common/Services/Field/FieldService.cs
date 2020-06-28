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
            new Field() { ID = "REPORTED_TOTAL_CONFIRMED", Description = "Gets the Reported Total Confirmed Cases", Color = "#FF9800" },
            new Field() { ID = "REPORTED_TOTAL_DEATHS", Description = "Gets the Reported Total Deaths Cases", Color = "#D50000" },
            new Field() { ID = "REPORTED_TOTAL_RECOVERED", Description = "Gets the Reported Total Recovered Cases", Color = "#009688" },

            // Reported Daily
            new Field() { ID = "REPORTED_DAILY_CONFIRMED", Description = "Gets the Reported Daily Confirmed", Color = "#FF9800" },
            new Field() { ID = "REPORTED_DAILY_DEATHS", Description = "Gets the Reported Daily Deaths", Color = "#D50000" },
            new Field() { ID = "REPORTED_DAILY_RECOVERED", Description = "Gets the Reported Daily Recovered", Color = "#009688" },

            // Daily Percent Increases
            new Field() { ID = "DAILY_PERCENT_INCREASE_CONFIRMED", Description = "Gets the Daily Percent increase of Confirmed cases", Color = "#FF9800" },
            new Field() { ID = "DAILY_PERCENT_INCREASE_DEATHS", Description = "Gets the Daily Percent increase of Deaths", Color = "#D50000" },
            new Field() { ID = "DAILY_PERCENT_INCREASE_RECOVERED", Description = "Gets the Daily Percent increase of Recovered cases", Color = "#009688" },

            // Population Ratios
            new Field() { ID = "POPULATION_CONFIRMED_RATIO", Description = "Gets the Confirmed/Population Ratio", Color = "#FF9800" },
            new Field() { ID = "POPULATION_DEATH_RATIO", Description = "Gets the Deaths/Population Ratio", Color = "#D50000" },
            new Field() { ID = "POPULATION_RECOVERED_RATIO", Description = "Gets the Recovered/Population Ratio", Color = "#009688" },

            // Rolling Seven Day
            new Field() { ID = "ROLLING_AVERAGE_SEVENDAY_CONFIRMED", Description = "Gets the 7-Day Rolling Average of Confirmed Cases", Color = "#FF9800" },
            new Field() { ID = "ROLLING_AVERAGE_SEVENDAY_DEATHS", Description = "Gets the 7-Day Rolling Average of Deaths Cases", Color = "#D50000" },
            new Field() { ID = "ROLLING_AVERAGE_SEVENDAY_RECOVERED", Description = "Gets the 7-Day Rolling Average of Recovered Cases", Color = "#009688" },

            // Rolling Fourteen Day
            new Field() { ID = "ROLLING_AVERAGE_FOURTEENDAY_CONFIRMED", Description = "Gets the 14-Day Rolling Average of Confirmed Cases", Color = "#FF9800" },
            new Field() { ID = "ROLLING_AVERAGE_FOURTEENDAY_DEATHS", Description = "Gets the 14-Day Rolling Average of Deaths Cases", Color = "#D50000" },
            new Field() { ID = "ROLLING_AVERAGE_FOURTEENDAY_RECOVERED", Description = "Gets the 14-Day Rolling Average of Recovered Cases", Color = "#009688" },

            // Rolling Twenty One Day
            new Field() { ID = "ROLLING_AVERAGE_TWENTYONEDAY_CONFIRMED", Description = "Gets the 21-Day Rolling Average of Confirmed Cases", Color = "#FF9800" },
            new Field() { ID = "ROLLING_AVERAGE_TWENTYONEDAY_DEATHS", Description = "Gets the 21-Day Rolling Average of Deaths Cases", Color = "#D50000" },
            new Field() { ID = "ROLLING_AVERAGE_TWENTYONEDAY_RECOVERED", Description = "Gets the 21-Day Rolling Average of Recovered Cases", Color = "#009688" },
        };

        public async Task<IQueryable<Field>> GetAll() => await Task.FromResult(FieldService.inMemoryFields.AsQueryable());
    }
}