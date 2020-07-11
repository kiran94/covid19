namespace Covid.Api.Common.Services.Field
{
    using System.Linq;
    using Covid.Api.Common.DataAccess;

    /// <summary>
    /// Responsible for providing field information.
    /// </summary>
    public interface IFieldService
    {
        IQueryable<Field> Query();
    }

    public class FieldService : IFieldService
    {
        private readonly IRepository repository;

        public FieldService(IRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<Field> Query() => this.repository.Query<Field>();
    }
}