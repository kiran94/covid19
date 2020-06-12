namespace Covid.Api.Common.Entities
{
    using System;

    public class TimeSeries
    {
        public string CountryRegion { get; set; }
        public string ProvinceState { get; set; }
        public string County { get; set; }
        public string Field { get; set; }
        public DateTime Date { get; set; }
        public double? Value { get; set; }

        /// <inheritdoc />
        public override string ToString() => $"{this.CountryRegion} - {this.ProvinceState} - {this.County}";
    }
}