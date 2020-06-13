using System;
namespace Covid.Api.GraphQL.Types
{
    using Covid.Api.Common.DataAccess;
    using Covid.Api.Common.Entities;
    using Covid.Api.GraphQL.Query;
    using Covid.Api.GraphQL.Services;
    using Covid.Api.GraphQL.Services.DataLoader;
    using global::GraphQL.DataLoader;
    using global::GraphQL.Types;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;

    public class CountryType : ObjectGraphType<Country>
    {
        public CountryType(
            ApiContext sql,
            ITimeSeriesService timeSeriesService,
            ITimeSeriesDataLoader timeSeriesDataLoader,
            IDataLoaderContextAccessor dataLoader)
        {
            this.Field(x => x.CountryRegion);
            this.Field(x => x.ProvinceState);
            this.Field(x => x.County);
            this.Field(x => x.Latitude, nullable: true);
            this.Field(x => x.Longitude, nullable: true);
            this.Field(x => x.Population, nullable: true);
            this.Field(x => x.Iso2, nullable: true);
            this.Field(x => x.Iso3, nullable: true);

            this.FieldAsync<ListGraphType<TimeSeriesType>>(
                name: "timeseries",
                description: "Get the Timeseries for this Country",
                arguments: new QueryArguments()
                {
                    Parameters.Argument<ListGraphType<StringGraphType>>(Parameters.Fields),
                    Parameters.Argument<ListGraphType<DateTimeGraphType>>(Parameters.Dates),
                    Parameters.Argument<IntGraphType>(Parameters.Take),
                    Parameters.Argument<IntGraphType>(Parameters.Skip)
                },
                resolve: async context =>
                {
                    var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<Country, IEnumerable<TimeSeries>>(
                        nameof(timeSeriesDataLoader.GetTimeSeries),
                        countries => timeSeriesDataLoader.GetTimeSeries(countries, context, context.CancellationToken));

                    var result = await loader.LoadAsync(context.Source);

                    return result.SelectMany(x => x);


                    // var timeseries = sql.Set<TimeSeries>().OrderByDescending(x => x.Date).AsQueryable();

                    // timeseries = timeseries.Where(x => x.CountryRegion == context.Source.CountryRegion);
                    // timeseries = timeseries.Where(x => x.ProvinceState == context.Source.ProvinceState);
                    // timeseries = timeseries.Where(x => x.County == context.Source.County);

                    // var result = await timeSeriesService.Query(timeseries, context);

                    // return result.ToListAsync(context.CancellationToken);
                }
            );
        }
    }
}