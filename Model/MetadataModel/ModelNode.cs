using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace yDevs.Model.MetadataModel
{
    public class ModelNode 
    {
        [BsonElement("id")]
        [JsonProperty("id", Required = Required.AllowNull)]
        public long? Id { get; set; }

        [BsonElement("name")]
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [BsonElement("type")]
        [JsonProperty("type", Required = Required.Always)]
        public string Type { get; set; }

        [BsonElement("enum")]
        [JsonProperty("enum")]
        public string[] Enum { get; set; }

        [BsonElement("min_length")]
        [JsonProperty("min_length")]
        public int? MinLength { get; set; }

        [BsonElement("max_length")]
        [JsonProperty("max_length")]
        public int? MaxLength { get; set; }

        [BsonElement("pattern")]
        [JsonProperty("pattern")]
        public string Pattern { get; set; }

        [BsonElement("format")]
        [JsonProperty("format")]
        public string Format { get; set; }

        [BsonElement("multiple_of")]
        [JsonProperty("multiple_of")]
        public int? MultipleOf { get; set; }

        [BsonElement("minimum")]
        [JsonProperty("minimum")]
        public int? Minimum { get; set; }

        [BsonElement("maximum")]
        [JsonProperty("maximum")]
        public int? Maximum { get; set; }

        [BsonElement("exclusive_minimum")]
        [JsonProperty("exclusive_minimum")]
        public bool? ExclusiveMinimum { get; set; }

        [BsonElement("exclusive_maximum")]
        [JsonProperty("exclusive_maximum")]
        public bool? ExclusiveMaximum { get; set; }

        [BsonElement("minimum_items")]
        [JsonProperty("minimum_items")]
        public int? MinimumItems { get; set; }

        [BsonElement("maximum_items")]
        [JsonProperty("maximum_items")]
        public int? MaximumItems { get; set; }

        [BsonElement("uniqueness")]
        [JsonProperty("uniqueness")]
        public bool? Uniqueness { get; set; }

        [BsonElement("required")]
        [JsonProperty("required")]
        public bool? Required_ { get; set; }

        [BsonElement("readonly")]
        [JsonProperty("readonly")]
        public bool? ReadOnly { get; set; }

        [BsonElement("children")]
        [JsonProperty("children")]
        public ModelNode[] Children { get; set; }
    }
}