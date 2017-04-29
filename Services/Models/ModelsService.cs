using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using yDevs.Config;
using yDevs.Dam.Services.Database;
using yDevs.Model.MetadataModel;

namespace yDam.Services.Models
{
    public class ModelsService : IModelsService
    {
        private readonly string ModelsCollectionName = "models";
        private readonly IMongoDbContext _mongoDbContext;
        private IMongoCollection<MetadataModel> _modelsCollection;
        private readonly AppSettings _settings;

        public ModelsService(IMongoDbContext mongoDbContext, IOptions<AppSettings> settings)
        {
            _mongoDbContext = mongoDbContext;
            _modelsCollection = _mongoDbContext.Database.GetCollection<MetadataModel>(ModelsCollectionName);
            _settings = settings.Value;
        }

        public MetadataModel[] GetModels()
        {            
            if (!ExistsModelCollection())
            {
                this.SaveModels(DefaultModels());
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

        public void SaveModels(MetadataModel[] models)
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