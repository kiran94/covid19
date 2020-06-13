using System.Linq;

namespace Covid.Api.GraphQL.Services.DataLoader
{
    using System.Threading;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Covid.Api.Common.Entities;
    using global::GraphQL.Types;
    using Covid.Api.Common.DataAccess;
    using Covid.Api.GraphQL.Query;
    using Covid.Api.GraphQL.Extensions;
    using System;

    public interface ITimeSeriesDataLoader
    {
        Task<ILookup<Country, IEnumerable<TimeSeries>>> GetTimeSeries(
            IEnumerable<Country> countries,
            ResolveFieldContext<Country> context,
            CancellationToken token);
    }

    public class TimeSeriesDataLoader : ITimeSeriesDataLoader
    {
        private readonly ApiContext sql;
        private readonly ITimeSeriesService timeseries;

        public TimeSeriesDataLoader(ApiContext sql, ITimeSeriesService timeseries)
        {
            this.sql = sql;
            this.timeseries = timeseries;
        }

        public async Task<ILookup<Country, IEnumerable<TimeSeries>>> GetTimeSeries(
            IEnumerable<Country> countries,
            ResolveFieldContext<Country> context,
            CancellationToken token)
        {

            var countriesRegions = countries.Select(x => x.CountryRegion);
            var provinceState = countries.Select(x => x.ProvinceState);
            var counties = countries.Select(x => x.County);

            var timeseries = this.sql.Set<TimeSeries>()
                .Where(x => countriesRegions.Contains(x.CountryRegion)
                    || provinceState.Contains(x.ProvinceState)
                    || counties.Contains(x.County));

            timeseries = await this.timeseries.Query(timeseries, context);

            var result = countries.GroupJoin(
                timeseries,
                x => new { x.CountryRegion, x.ProvinceState, x.County },
                x => new { x.CountryRegion, x.ProvinceState, x.County },
                (countries, timeslices) => (countries, timeseries))
                .ToDictionary(x => x.countries, x => x.timeseries)
                .ToLookup(x => x.Key, x => x.Value.AsEnumerable());

            return result;
        }
    }
}