using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using yDam.Dam.Model.Assets;
using yDevs.Config;
using yDevs.Dam.Services.Database;
using yDevs.Shared.Exceptions;

namespace yDevs.Dam.Services.Assets
{
    public class AssetService : IAssetService
    {
        private readonly string AssetCollectionName = "assets";
        private readonly IMongoDbContext _mongoDbContext;
        private IMongoCollection<Asset> _assetCollection;
        private readonly AppSettings _settings;

        public AssetService(IMongoDbContext mongoDbContext, IOptions<AppSettings> settings)
        {
            _mongoDbContext = mongoDbContext;
            _assetCollection = _mongoDbContext.Database.GetCollection<Asset>(AssetCollectionName);
            _settings = settings.Value;
        }

        public AssetGetResponse Get(AssetGetRequest request)
        {
            // Limit max results
            if (request.MaxResults<1)
            {
                request.MaxResults = _settings.MaxResults;
            }
            else if (request.MaxResults> _settings.ResultsLimit)
            {
                request.MaxResults = _settings.ResultsLimit;
            }

            List<Asset> assets = new List<Asset>();
            if (string.IsNullOrEmpty(request.SearchText))
            {
                assets = _assetCollection.Find<Asset>(x => true)
                    // .Sort(Builders<Asset>.Sort.MetaTextScore("textscore"))
                    .Limit(request.MaxResults)
                    .Skip(request.Skip)
                    .ToList();
            }
            else
            {
                assets = _assetCollection.Find<Asset>(Builders<Asset>.Filter.Text(request.SearchText))
                    // .Sort(Builders<Asset>.Sort.MetaTextScore("textscore"))
                    .Limit(request.MaxResults)
                    .Skip(request.Skip)
                    .ToList();
            }
                   
            return new AssetGetResponse()
            {                
                Results = assets
            };
        }

        public AssetCreateResponse Create(AssetCreateRequest request)
        {
            _assetCollection.InsertOne(request.NewAsset);            
            
            return new AssetCreateResponse()
            {
                Result = request.NewAsset
            };
        }

        public void Delete(string id)
        {
            ObjectId oId;
            if (!ObjectId.TryParse(id, out oId))
            {
                throw new HttpException(HttpStatusCode.BadRequest, String.Format("Invalid Asset Id '{0}'", id));
            }

            Asset deletedAsset = _assetCollection.FindOneAndDelete<Asset>(x => x.Id.Equals(oId));
            if (deletedAsset==null)
            {
                throw new HttpException(HttpStatusCode.NotFound, String.Format("Asset Id '{0}' not found", id));
            }
        }
    }
}