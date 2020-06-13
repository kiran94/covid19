namespace Covid.Api.GraphQL.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using global::GraphQL.Types;
    using OpenTracing;

    public static class GraphQLExtensions
    {
        /// <summary>
        /// Checks if the given argument exists, if it exists, the value of the argument is set into value.
        /// </summary>
        public static bool TryGetArgument<TContext, TValue>(this ResolveFieldContext<TContext> context, string field, out TValue value)
        {
            if (context.HasArgument(field))
            {
                value = context.GetArgument<TValue>(field);
                return true;
            }

            value = default;
            return false;
        }

        public static ISpan WithGraphQLTags(this ISpan span, ResolveFieldContext<object> context)
        {
            foreach (var argument in context.Arguments)
            {
                if (argument.Value is List<object> list)
                {
                    span.SetTag($"collection.{argument.Key}", true);

                    if (list.Count == 1)
                    {
                        span.SetTag(argument.Key, list[0].ToString());
                    }
                    else
                    {
                        span.Log(list.Select(x => KeyValuePair.Create(argument.Key, x)));
                    }
                }
                else if (argument.Value is object[] array)
                {
                    span.SetTag($"collection.{argument.Key}", true);

                    if (array.Count() == 1)
                    {
                        span.SetTag(argument.Key, array[0].ToString());
                    }
                    else
                    {
                        span.Log(array.Select(x => KeyValuePair.Create(argument.Key, x)));
                    }
                }
                else
                {
                    span.SetTag(argument.Key, argument.Value.ToString());
                }
            }

            return span;
        }
    }
}