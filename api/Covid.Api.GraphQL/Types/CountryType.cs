namespace Covid.Api.GraphQL.Types
{
    using Covid.Api.Common.Entities;
    using global::GraphQL.Types;

    public class CountryType : ObjectGraphType<Country>
    {
        public CountryType()
        {
            this.Field(x => x.CountryRegion);
            this.Field(x => x.ProvinceState);
            this.Field(x => x.County);
            this.Field(x => x.Latitude, nullable: true);
            this.Field(x => x.Longitude, nullable: true);
            this.Field(x => x.Population, nullable: true);
            this.Field(x => x.Iso2, nullable: true);
            this.Field(x => x.Iso3, nullable: true);
        }
    }
}