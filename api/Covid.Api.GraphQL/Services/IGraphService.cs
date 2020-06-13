
namespace Covid.Api.GraphQL.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using global::GraphQL.Types;

    /// <summary>
    /// Represents abstract application of graphql
    /// parameters on an entity
    /// </summary>
    /// <typeparam name="T">Type of entity we are operating on (actual entity, not the graphtype)</typeparam>
    public interface IGraphService<T> where T : class
    {
        /// <summary>
        /// Builds a Query on the incoming <see cref="IQueryable{T}" /> and applies the arguemnts
        /// in the incoming <see cref="ResolveFieldContext{TContext}" />
        /// </summary>
        /// <param name="collection">collection of nodes we are operating on</param>
        /// <param name="context">graphql context to retrieve argument parameters from</param>
        /// <typeparam name="TContext">type of context we are looking at</typeparam>
        /// <returns>queryable with the arguments applied</returns>
        Task<IQueryable<T>> Query<TContext>(IQueryable<T> collection, ResolveFieldContext<TContext> context);
    }
}