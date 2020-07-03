namespace Covid.Api.GraphQL.Query
{
    using System.Collections.Generic;
    using global::GraphQL.Types;

    public static class Parameters
    {
        public const string Queries = "query";
        public const string CountryRegion = "country_region";
        public const string ProvinceState = "province_state";
        public const string Counties = "counties";
        public const string Fields = "fields";
        public const string Dates = "dates";
        public const string Take = "take";
        public const string Skip = "skip";
        public const string Chronological  = "chronological";

        /// <summary>
        /// Maps Parameters to Descriptions
        /// </summary>
        private static Dictionary<string, string> descriptionMapping = new Dictionary<string, string>()
        {
            [Queries] = "Fuzzy Search Country/Region/Province/State/County",
            [CountryRegion] = "The Country/Region(s) to Filter on",
            [ProvinceState] = "The Province/State(s) to Filter on",
            [Counties] = "The Counties to Filter on",
            [Fields] = "The Field to Filter on",
            [Dates] = "The Date to Filter on",
            [Chronological] = "Order Events in chronological order",
            [Take] = "(Pagination) only take specified number of results",
            [Skip] = "(Pagination) skip the specified number of results"
        };

        /// <summary>
        /// Constructs a QueryArgument.
        /// </summary>
        public static QueryArgument<T> Argument<T>(string name) where T : IGraphType
        {
            return new QueryArgument<T>()
            {
                Name = name,
                Description = descriptionMapping.GetValueOrDefault(name, string.Empty)
            };
        }
    }
}