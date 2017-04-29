using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace yDevs.Model.MetadataModel
{
    public class MetadataModel
    {
        [BsonId]
        [JsonProperty("id")]
        [JsonConverter(typeof(ObjectIdConverter))]
        ObjectId Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("asset")]
        public ModelNode Asset { get; set; }
    }
}