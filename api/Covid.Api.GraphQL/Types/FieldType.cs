namespace Covid.Api.GraphQL.Types
{
    using Covid.Api.Common.Services.Field;
    using global::GraphQL.Types;

    public class FieldType : ObjectGraphType<Field>
    {
        public FieldType()
        {
            this.Field(x => x.ID);
            this.Field(x => x.Description, nullable: true);
        }
    }
}