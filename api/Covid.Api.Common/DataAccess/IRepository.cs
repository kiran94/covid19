using System.Linq;
namespace Covid.Api.Common.DataAccess
{
    public interface IRepository
    {
        IQueryable<T> Query<T>() where T : class;
    }
}