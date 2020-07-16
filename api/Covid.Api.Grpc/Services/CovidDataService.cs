namespace Covid.Api.Grpc.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using global::Grpc.Core;
    using Microsoft.Extensions.Logging;

    public class CovidDataService : CovidService.CovidServiceBase
    {
        private readonly ILogger<CovidDataService> logger;

        public CovidDataService(ILogger<CovidDataService> logger)
        {
            this.logger = logger;
        }

        public override Task<CovidResponseCollection> Get(CovidRequest request, ServerCallContext context)
        {
            this.logger.LogInformation("Got Request!");
            throw new NotImplementedException();
        }
    }
}