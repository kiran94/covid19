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
            new Field() { ID = "DAILY_PERCENT_INCREASE_CONFIRMED" },
            new Field() { ID = "DAILY_PERCENT_INCREASE_DEATHS" },
            new Field() { ID = "DAILY_PERCENT_INCREASE_RECOVERED" },
            new Field() { ID = "POPULATION_CONFIRMED_RATIO" },
            new Field() { ID = "POPULATION_DEATH_RATIO" },
            new Field() { ID = "POPULATION_RECOVERED_RATIO" },
            new Field() { ID = "REPORTED_DAILY_CONFIRMED" },
            new Field() { ID = "REPORTED_DAILY_DEATHS" },
            new Field() { ID = "REPORTED_DAILY_RECOVERED" },
            new Field() { ID = "REPORTED_TOTAL_CONFIRMED" },
            new Field() { ID = "REPORTED_TOTAL_DEATHS" },
            new Field() { ID = "REPORTED_TOTAL_RECOVERED" },
            new Field() { ID = "ROLLING_AVERAGE_FOURTEENDAY_CONFIRMED" },
            new Field() { ID = "ROLLING_AVERAGE_FOURTEENDAY_DEATHS" },
            new Field() { ID = "ROLLING_AVERAGE_FOURTEENDAY_RECOVERED" },
            new Field() { ID = "ROLLING_AVERAGE_SEVENDAY_CONFIRMED" },
            new Field() { ID = "ROLLING_AVERAGE_SEVENDAY_DEATHS" },
            new Field() { ID = "ROLLING_AVERAGE_SEVENDAY_RECOVERED" },
            new Field() { ID = "ROLLING_AVERAGE_TWENTYONEDAY_CONFIRMED" },
            new Field() { ID = "ROLLING_AVERAGE_TWENTYONEDAY_DEATHS" },
            new Field() { ID = "ROLLING_AVERAGE_TWENTYONEDAY_RECOVERED" }
        };

        public async Task<IQueryable<Field>> GetAll() => await Task.FromResult(FieldService.inMemoryFields.AsQueryable());
    }
}