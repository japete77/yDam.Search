using System;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace yDevs.Model.MetadataModel
{
    public class ObjectIdConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ObjectId);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception(
                    String.Format("Unexpected token parsing ObjectId. Expected String, got {0}.",
                                  reader.TokenType));
            }

            var value = (string)reader.Value;
            return String.IsNullOrEmpty(value) ? ObjectId.Empty : new ObjectId(value);

            // DateTime timestamp;
            // int machine;
            // int pid;
            // int increment;

            // try
            // {
            //     machine = reader.ReadAsInt32().Value;
            //     pid = reader.ReadAsInt32().Value;
            //     increment = reader.ReadAsInt32().Value;
            //     timestamp = reader.ReadAsDateTime().Value;
            // }
            // catch
            // {
            //     throw new Exception(
            //         String.Format("Unexpected token parsing ObjectId"));
            // }

            // return new ObjectId(timestamp, machine, (short)pid, increment);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is ObjectId)
            {
                var objectId = (ObjectId)value;

                writer.WriteValue(objectId != ObjectId.Empty ? objectId.ToString() : String.Empty);
            }
            else
            {
                throw new Exception("Expected ObjectId value.");
            }
        }
    }
}