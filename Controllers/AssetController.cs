using Microsoft.AspNetCore.Mvc;
using yDam.Dam.Model.Assets;
using yDevs.Dam.Services.Assets;

namespace yDam.Dam.Controllers
{
    [Route("api/[action]")]
    public class AssetController : Controller
    {
        private readonly IAssetService _assetService;
        
        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpPost]
        public AssetCreateResponse Asset([FromBody] AssetCreateRequest request)
        {
            return _assetService.Create(request);
        }

        [HttpGet]
        public AssetGetResponse Asset([FromQuery] AssetGetRequest request)
        {
            return _assetService.Get(request);
        }

        [HttpDelete]
        public void Asset([FromQuery] string id)
        {
            _assetService.Delete(id);
        }
    }    
}