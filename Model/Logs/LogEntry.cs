using MongoDB.Bson.Serialization.Attributes;

namespace yDevs.Model.Logs
{
    public class LogEntry
    {
        [BsonElement("execution_time")]
        public int ExecutionTime { get; set; }

        [BsonElement("log_type")]
        public string LogType { get; set; }

        [BsonElement("system_name")]
        public string SystemName { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("trace_guid")]
        public string TraceGuid { get; set; }

        [BsonElement("message")]
        public string Message { get; set; }

        [BsonElement("exception")]
        public string Exception { get; set; }

        [BsonElement("error_guid")]
        public string ErrorGuid { get; set; }
    }
}