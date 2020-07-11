using System.Collections.Generic;
using Covid.Api.Common.DataAccess.Attribute;
using MongoDB.Bson.Serialization.Attributes;

namespace Covid.Api.Common.Entities
{
    [MongoCollection("countries")]
    [BsonIgnoreExtraElements]
    public class Country
    {
        [BsonElement("country_region")]
        public string CountryRegion { get; set; }

        [BsonElement("province_state")]
        public string ProvinceState { get; set; }

        [BsonElement("county")]
        public string County { get; set; }

        [BsonElement("latitude")]
        public double? Latitude { get; set; }

        [BsonElement("longitude")]
        public double? Longitude { get; set; }

        [BsonElement("population")]
        public double? Population { get; set; }

        [BsonElement("iso2")]
        public string Iso2 { get; set; }

        [BsonElement("iso3")]
        public string Iso3 { get; set; }

        [BsonElement("region")]
        public string Region { get; set; }

        [BsonElement("subregion")]
        public string SubRegion { get; set; }

        [BsonElement("borders")]
        public List<string> Borders { get; set;}

        [BsonElement("gini")]
        public double? WorldBankIndex {get; set; }

        [BsonElement("flag")]
        public string FlagUrl { get; set; }

        [BsonElement("regionalBlocs")]
        public List<RegionalBlock> RegionalBlocks { get; set; }


        [BsonElement("currencies")]
        public List<Currency> Currencies { get; set; }

        /// <inheritdoc />
        public override string ToString() => $"{this.CountryRegion} - {this.ProvinceState} - {this.County}";
    }


    [BsonIgnoreExtraElements]
    public class RegionalBlock
    {
        [BsonElement("acronym")]
        public string Acronym { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Currency
    {
        [BsonElement("code")]
        public string Code { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("symbol")]
        public string Symbol { get; set; }
    }
}