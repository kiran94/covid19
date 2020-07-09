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

        /// <inheritdoc />
        public override string ToString() => $"{this.CountryRegion} - {this.ProvinceState} - {this.County}";
    }
}