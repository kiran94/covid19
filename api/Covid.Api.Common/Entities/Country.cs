namespace Covid.Api.Common.Entities
{
    public class Country
    {
        public string CountryRegion { get; set; }
        public string ProvinceState { get; set; }
        public string County { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Population { get; set; }
        public string Iso2 { get; set; }
        public string Iso3 { get; set; }

        /// <inheritdoc />
        public override string ToString() => $"{this.CountryRegion} - {this.ProvinceState} - {this.County}";
    }
}