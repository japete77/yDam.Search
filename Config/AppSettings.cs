using System.Collections.Generic;

namespace yDevs.Config
{
    public class AppSettings
    {
        public Mongo Mongo { get; set; }
        public Logger Logger { get; set; }
        public int MaxResults { get; set; }
        public int ResultsLimit { get; set; }
        public List<string> AllowCORS { get; set; }
        public bool EnableSwagger { get; set; }
    }

    public class Mongo
    {
        public string Url { get; set; }
        public string Database { get; set; }
    }

    public class Logger
    {
        public string Level { get; set; }
        public string Collection { get; set; }
        public string TTL { get; set; }
    }

}