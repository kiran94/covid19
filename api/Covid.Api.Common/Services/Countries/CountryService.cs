namespace Covid.Api.Common.Services.Countries
{
    using System.Linq;
    using Covid.Api.Common.DataAccess;

    /// <summary>
    /// Responsible for retrieving country information.
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// Queries Country Information
        /// </summary>
        IQueryable<Country> Query();
    }

    public class CountryService : ICountryService {
        private readonly IRepository repository;

        public CountryService (IRepository repository)
        {
            this.repository = repository;
        }

        /// <inheritdoc />
        public IQueryable<Country> Query() => this.repository.Query<Country>();
    }
}