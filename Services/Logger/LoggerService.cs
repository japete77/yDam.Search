using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using yDevs.Config;
using yDevs.Dam.Services.Database;
using yDevs.Shared.Utils;

namespace yDevs.Services.Logger
{
    public class LoggerService : ILoggerService
    {
        private readonly Serilog.Core.Logger _logger;
        private readonly string _timestampIndexName = "Timestamp_1";
        private readonly string _timestampIndexField = "{ Timestamp: 1 }";
        public LoggerService(IOptions<AppSettings> settings, IMongoDbContext mongoDbContext)
        {            
            var loggerConfiguration = new LoggerConfiguration();
            switch (settings.Value.Logger.Level.ToLower())
            {
                case "verbose":
                    loggerConfiguration = loggerConfiguration.MinimumLevel.Verbose();
                    break;
                case "debug":
                    loggerConfiguration = loggerConfiguration.MinimumLevel.Debug();
                    break;
                case "information":
                    loggerConfiguration = loggerConfiguration.MinimumLevel.Information();
                    break;
                case "warning":
                    loggerConfiguration = loggerConfiguration.MinimumLevel.Warning();
                    break;
                case "error":
                    loggerConfiguration = loggerConfiguration.MinimumLevel.Error();   
                    break;
                case "fatal":
                    loggerConfiguration = loggerConfiguration.MinimumLevel.Fatal();
                    break;
                default:
                    loggerConfiguration = loggerConfiguration.MinimumLevel.Error();
                    break;
            }
            
            loggerConfiguration = loggerConfiguration
                    .WriteTo.MongoDB(
                        settings.Value.Mongo.Url + "/" + settings.Value.Mongo.Database, 
                        settings.Value.Logger.Collection)
                    .Enrich.With(new ThreadIdEnricher())
                    .Enrich.With(new MachineNameEnricher());
            
            this._logger = loggerConfiguration.CreateLogger();
            
            // Setup logs TTL
            var ttlTimeSpan = Utils.ConvertToTimeSpan(settings.Value.Logger.TTL);
            var logCollection = mongoDbContext.Database.GetCollection<BsonDocument>(settings.Value.Logger.Collection);
            var indexes = logCollection.Indexes.List();
            while (indexes.MoveNext())
            {
                foreach (var i in indexes.Current)
                {
                    if (i.GetValue("name").ToString().Equals(_timestampIndexName) &&
                        !i.GetValue("expireAfterSeconds").ToDecimal().Equals(ttlTimeSpan))
                        {
                            logCollection.Indexes.DropOne(_timestampIndexName);
                        }                    
                }
            }

            logCollection
                .Indexes
                .CreateOne(_timestampIndexField, new CreateIndexOptions() { ExpireAfter =  ttlTimeSpan });
        }

        public ILogger Logger()
        {
            return this._logger;
        }
    }
}