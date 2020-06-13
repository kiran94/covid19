namespace Covid.Api.GraphQL.Services
{
    using System;
    using global::GraphQL.Types;
    using System.Linq;
    using System.Collections.Generic;
    using Covid.Api.GraphQL.Extensions;
    using Covid.Api.GraphQL.Query;
    using Covid.Api.Common.Entities;
    using System.Threading.Tasks;

    public interface ITimeSeriesService : IGraphService<TimeSeries>
    {
    }

    public class TimeSeriesService : ITimeSeriesService
    {
        public Task<IQueryable<TimeSeries>> Query<TContext>(IQueryable<TimeSeries> collection, ResolveFieldContext<TContext> context)
        {
            if (context.TryGetArgument<TContext, List<string>>(Parameters.CountryRegion, out var countries))
            {
                collection = collection.Where(x => countries.Contains(x.CountryRegion));
            }

            if (context.TryGetArgument<TContext, List<string>>(Parameters.ProvinceState, out var provinces))
            {
                collection = collection.Where(x => provinces.Contains(x.ProvinceState));
            }

            if (context.TryGetArgument<TContext, List<string>>(Parameters.Counties, out var counties))
            {
                collection = collection.Where(x => counties.Contains(x.County));
            }

            if (context.TryGetArgument<TContext, List<string>>(Parameters.Fields, out var fields))
            {
                collection = collection.Where(x => fields.Contains(x.Field));
            }

            if (context.TryGetArgument<TContext, List<DateTime>>(Parameters.Dates, out var dates))
            {
                collection = collection.Where(x => dates.Contains(x.Date));
            }

            if (context.TryGetArgument<TContext, int>(Parameters.Skip, out var skip)) collection = collection.Skip(skip);
            if (context.TryGetArgument<TContext, int>(Parameters.Take, out var take)) collection = collection.Take(take);

            return Task.FromResult(collection);
        }
    }
}