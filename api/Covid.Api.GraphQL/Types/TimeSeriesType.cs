namespace Covid.Api.GraphQL.Types
{
    using Covid.Api.Common.Services.TimeSeries;
    using global::GraphQL.Types;

    public class TimeSeriesType : ObjectGraphType<TimeSeries>
    {
        public TimeSeriesType()
        {
            this.Field(x => x.CountryRegion);

            this.Field(x => x.ProvinceState);
            this.Field(x => x.County);
            this.Field(x => x.Field);
            this.Field(x => x.Date);
            this.Field(x => x.Value, nullable: true);
        }
    }
}