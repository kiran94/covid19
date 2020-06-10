namespace Covid.Api.GraphQL.Extensions
{
    using global::GraphQL.Types;

    public static class GraphQLExtensions
    {
        /// <summary>
        /// Checks if the given argument exists, if it exists, the value of the argument is set into value.
        /// </summary>
        public static bool TryGetArgument<T>(this ResolveFieldContext<object> context, string field, out T value)
        {
            if (context.HasArgument(field))
            {
                value = context.GetArgument<T>(field);
                return true;
            }

            value = default;
            return false;
        }
    }
}