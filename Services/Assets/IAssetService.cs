using yDam.Dam.Model.Assets;

namespace yDevs.Dam.Services.Assets
{
    public interface IAssetService
    {
        AssetCreateResponse Create(AssetCreateRequest request);
        AssetGetResponse Get(AssetGetRequest request);
        void Delete(string id);
    }
}