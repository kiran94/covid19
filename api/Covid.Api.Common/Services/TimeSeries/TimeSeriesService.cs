namespace Covid.Api.Common.Services.TimeSeries
{
    using System.Linq;
    using Covid.Api.Common.DataAccess;

    /// <summary>
    /// Responsible for providing field information.
    /// </summary>
    public interface ITimeSeriesService
    {
        IQueryable<TimeSeries> Query();
    }

    public class TimeSeriesService : ITimeSeriesService
    {
        private readonly IRepository repository;

        public TimeSeriesService(IRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<TimeSeries> Query() => this.repository.Query<TimeSeries>();
    }
}