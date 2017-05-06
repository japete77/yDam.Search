using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using yDevs.Config;
using yDevs.Dam.Services.Database;
using yDevs.Model.MetadataModel;
using yDevs.Services.Logger;
using yDevs.Shared.Exceptions;

namespace yDam.Services.Models
{
    public class ModelsService : IModelsService
    {
        private readonly string ModelsCollectionName = "models";
        private readonly IMongoDbContext _mongoDbContext;
        private IMongoCollection<MetadataModel> _modelsCollection;
        private readonly AppSettings _settings;
        private readonly ILoggerService _loggerService;

        public ModelsService(IMongoDbContext mongoDbContext, IOptions<AppSettings> settings, ILoggerService loggerService)
        {
            _mongoDbContext = mongoDbContext;
            _modelsCollection = _mongoDbContext.Database.GetCollection<MetadataModel>(ModelsCollectionName);
            _settings = settings.Value;
            _loggerService = loggerService;
        }

        public MetadataModel[] GetModels()
        {            
            if (!ExistsModelCollection())
            {
                this.UpdateModels(DefaultModels());
            }

            return _modelsCollection.Find<MetadataModel>(x => true).ToList().ToArray();
        }

        public string GetModelsJson()
        {
            return JsonConvert.SerializeObject(_modelsCollection.Find<MetadataModel>(x => true).ToList(), 
                Formatting.Indented, 
                new JsonSerializerSettings { 
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        public Stream GetModelsZip()
        {
            var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var models = _modelsCollection.Find<MetadataModel>(x => true).ToList();
                foreach(var m in models)
                {
                    var jsonModel = JsonConvert.SerializeObject(m, 
                        Formatting.Indented,
                        new JsonSerializerSettings { 
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    var file = archive.CreateEntry($"{m.Type}.json");
                    using (var entryStream = file.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        streamWriter.Write(jsonModel);
                    }
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public void UpdateModels(MetadataModel[] models)
        {
            if (models!=null)
            {
                foreach (var model in models)
                {
                    this._modelsCollection.UpdateOne(u => u.Type == model.Type, 
                        Builders<MetadataModel>.Update.Set(u => u.Asset, model.Asset), 
                        new UpdateOptions { IsUpsert = true });
                }
            }
        }

        public void UpdateModels(byte[] models)
        {
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.WriteAsync(models, 0, models.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
                try
                {
                    using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                    {
                        foreach(var entry in zip.Entries)
                        {
                            using(var stream = entry.Open())
                            {
                                using (StreamReader streamReader = new StreamReader(stream))
                                {
                                    var jsonModel = streamReader.ReadToEnd();
                                    
                                    try
                                    {
                                        MetadataModel model = JsonConvert.DeserializeObject<MetadataModel>(jsonModel);
                                        UpdateModels(new MetadataModel[] { model });
                                    }
                                    catch (Exception e)
                                    {
                                        this._loggerService.Logger().Error(e.Message);
                                        throw new HttpException(HttpStatusCode.InternalServerError, "Invalid format. Please review JSON model syntax in the imported file.");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (InvalidDataException e)
                {
                    this._loggerService.Logger().Error(e.Message);
                    throw new HttpException(HttpStatusCode.InternalServerError, "Invalid file format.");
                }
            }
        }

        private MetadataModel[] DefaultModels()
        {
            string[] assetTypes = { "Audio", "Collection", "Document", "Image", "Raw", "Video" };
            List<MetadataModel> models = new List<MetadataModel>();
            foreach (var aType in assetTypes)
            {
                models.Add(new MetadataModel {
                    Type = aType,
                    Asset = BaseModel(),
                });
            }

            return models.ToArray();
        }

        private bool ExistsModelCollection()
        {
            var collections = _mongoDbContext.Database.ListCollections().ToList();
            foreach (var collection in collections)
            {
                if (collection.GetValue("name", new BsonString(string.Empty))
                    .AsString
                    .Equals(ModelsCollectionName))
                    {
                        return true;
                    }
            }
            return false;
        }

        private ModelNode BaseModel()
        {
            return 
                new  ModelNode {
                    Name = "asset",
                    Type = "object",
                    Required_ = true,
                    ReadOnly = true,
                    Children = new ModelNode[] {
                        new ModelNode {
                            Name = "id",
                            Type = "object",
                            Required_ = true,
                            ReadOnly = true,
                            Children = new ModelNode[] {
                                new ModelNode() {
                                    Name = "timestamp",
                                    Type = "numeric",
                                    ReadOnly = true,
                                },
                                new ModelNode() {
                                    Name = "machine",
                                    Type = "numeric",
                                    ReadOnly = true,
                                },
                                new ModelNode() {
                                    Name = "pid",
                                    Type = "numeric",
                                    ReadOnly = true,
                                },
                                new ModelNode() {
                                    Name = "increment",
                                    Type = "numeric",
                                    ReadOnly = true,
                                },
                                new ModelNode() {
                                    Name = "creationTime",
                                    Type = "string",
                                    ReadOnly = true,
                                },
                            }
                        },
                        new ModelNode {
                            Name = "owner",
                            Type = "string",
                            Required_ = true,
                            ReadOnly = true,
                        },
                        new ModelNode {
                            Name = "group",
                            Type = "string",
                            Required_ = true,
                            ReadOnly = true,
                        },
                        new ModelNode {
                            Name = "permissions",
                            Type = "numeric",
                            Required_ = true,
                            ReadOnly = true,
                        },
                        new ModelNode {
                            Name = "last_update_date",
                            Type = "string",
                            Required_ = true,
                            ReadOnly = true,
                        },
                        new ModelNode {
                            Name = "last_update_user",
                            Type = "string",
                            Required_ = true,
                            ReadOnly = true,
                        },
                        new ModelNode {
                            Name = "asset_type",
                            Type = "string",
                            Required_ = true,
                            ReadOnly = true,
                        },
                        new ModelNode {
                            Name = "parent_folder",
                            Type = "array",
                            ReadOnly = true,
                            Uniqueness = true,
                            Children = new ModelNode[] {
                                new ModelNode {
                                    Name = "folder",
                                    Type = "string",
                                    ReadOnly = true,
                                }
                            }
                        },
                        new ModelNode {
                            Name = "model",
                            Type = "object",
                            Required_ = true,
                            ReadOnly = true,
                        },
                    }
                };
        }
    }
}