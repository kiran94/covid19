namespace Covid.Api.Common.DataAccess.Attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class MongoCollectionAttribute : Attribute
    {
        public MongoCollectionAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}