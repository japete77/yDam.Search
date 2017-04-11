using System;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace yDevs.Shared.Serializers
{
    public class CustomObjectSerializer : IBsonSerializer
    {
        public Type ValueType => typeof(System.Object);

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.GetCurrentBsonType() != BsonType.Document) 
            {
                throw new Exception("Object is not a JSON document");
            }
            
            var bsonDocument = BsonSerializer.Deserialize(context.Reader, typeof(BsonDocument)) as BsonDocument;
            var cleanJson = Regex.Replace(bsonDocument.ToJson(), @"ObjectId\((.[a-f0-9]{24}.)\)", (m) => m.Groups[1].Value);
            return JsonConvert.DeserializeObject<object>(cleanJson);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            var json = (value == null) ? "{}": JsonConvert.SerializeObject(value);
		    BsonDocument document = BsonDocument.Parse(json);
            BsonSerializer.Serialize(context.Writer, typeof(BsonDocument), document, null, args);
        }
    }
}