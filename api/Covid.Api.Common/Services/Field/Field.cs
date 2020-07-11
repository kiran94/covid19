namespace Covid.Api.Common.Services.Field
{
    using Covid.Api.Common.DataAccess.Attribute;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    [MongoCollection("fields")]
    [BsonIgnoreExtraElements]
    public class Field
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("color")]
        public string Color { get; set; }
    }
}