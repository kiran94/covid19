namespace Covid.Api.GraphQL.V1.Types
{
    using System.Collections.Generic;
    using Covid.Api.Common.Services.Countries;
    using global::GraphQL.Types;

    public class CountryType : ObjectGraphType<Country>
    {
        public CountryType()
        {
            this.Field(x => x.CountryRegion);
            this.Field(x => x.ProvinceState, nullable: true);
            this.Field(x => x.County, nullable: true);
            this.Field(x => x.Latitude, nullable: true);
            this.Field(x => x.Longitude, nullable: true);
            this.Field(x => x.Population, nullable: true);
            this.Field(x => x.Iso2, nullable: true);
            this.Field(x => x.Iso3, nullable: true);
            this.Field(x => x.Region);
            this.Field(x => x.SubRegion, nullable: true);
            this.Field(x => x.WorldBankIndex, nullable: true);
            this.Field(x => x.Borders, nullable: true);

            this.Field<List<RegionalBlock>>(
                x => x.RegionalBlocks,
                nullable: true,
                type: typeof(ListGraphType<RegionalBlockCountryType>));

            this.Field<List<Currency>>(
                x => x.Currencies,
                nullable: true,
                type: typeof(ListGraphType<CurrencyType>));
        }
    }

    public class RegionalBlockCountryType : ObjectGraphType<RegionalBlock>
    {
        public RegionalBlockCountryType()
        {
            this.Field(x =>x.Acronym);
            this.Field(x =>x.Name);
        }
    }

    public class CurrencyType : ObjectGraphType<Currency>
    {
        public CurrencyType()
        {
            this.Field(x => x.Code, nullable: true);
            this.Field(x => x.Name, nullable: true);
            this.Field(x => x.Symbol, nullable: true);
        }
    }
}