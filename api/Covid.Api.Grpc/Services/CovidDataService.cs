namespace Covid.Api.Grpc.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using global::Grpc.Core;
    using Microsoft.Extensions.Logging;
    using Covid.Api.Common.Services.TimeSeries;
    using Microsoft.EntityFrameworkCore;
    using Google.Protobuf.WellKnownTypes;
    using OpenTracing;

    public class CovidDataService : CovidService.CovidServiceBase
    {
        private readonly ILogger<CovidDataService> logger;
        private readonly ITimeSeriesService timeseries;
        private readonly ITracer tracer;

        public CovidDataService(ILogger<CovidDataService> logger, ITimeSeriesService timeseries, ITracer tracer)
        {
            this.logger = logger;
            this.timeseries = timeseries;
            this.tracer = tracer;
        }

        public override async Task<CovidResponseCollection> Get(CovidRequest request, ServerCallContext context)
        {
            using var span = this.tracer.BuildSpan(nameof(CovidDataService.Get))
                .WithTag(nameof(request.CountryRegion), request.CountryRegion)
                .WithTag(nameof(request.ProvinceState), request.ProvinceState)
                .WithTag(nameof(request.County), request.County)
                .WithTag(nameof(request.Fields), request.Fields?.ToString())
                .WithTag(nameof(request.DatesCase), request.DatesCase.ToString())
                .WithTag(nameof(request.AbsoluteDates), request.AbsoluteDates?.ToString())
                .WithTag(nameof(request.RelativeDates), request.RelativeDates?.ToString())
                .StartActive();

            // Construct Query
            var data = this.timeseries.Query();

            if (!string.IsNullOrWhiteSpace(request.CountryRegion))
                data = data.Where(x => x.CountryRegion == request.CountryRegion);

            request.ProvinceState = request.ProvinceState ?? "";
            data = data.Where(x => x.ProvinceState == request.ProvinceState);

            request.County = request.County ?? "";
            data = data.Where(x => x.County == request.County);

            if (request.Fields.Any())
            {
                data = data.Where(x => request.Fields.Contains(x.Field));
            }

            if (request.DatesCase == CovidRequest.DatesOneofCase.RelativeDates)
            {
                throw new NotImplementedException("Relative Dates not Implemented");
            }
            else if(request.AbsoluteDates != null)
            {
                var dates = request.AbsoluteDates.Dates.Select(x => x.ToDateTime().Date);
                data = data.Where(x => dates.Contains(x.Date));
            }

            // Get Data
            var result = await data.ToListAsync(context.CancellationToken);

            result.ForEach(x =>
            {
                x.Date = DateTime.SpecifyKind(x.Date, DateTimeKind.Utc);
            });

            // Construct Response
            var covidResponses = result.Select(x => new CovidResponse()
            {
                CountryRegion = x.CountryRegion,
                ProvinceState = x.ProvinceState,
                County = x.County,
                Field = x.Field,
                Date =  Timestamp.FromDateTime(x.Date),
                Value = x.Value ?? 0
            }).ToList();

            var covidResponsesCollection = new CovidResponseCollection();
            covidResponsesCollection.Response.AddRange(covidResponses);

            return covidResponsesCollection;
        }
    }
}