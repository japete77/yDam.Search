using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using yDam.Services.Models;
using yDevs.Model.MetadataModel;

namespace yDam.Dam.Controllers
{
    [Route("api/[action]")]
    public class ModelsController: Controller
    {
        private IModelsService _modelsService;
        public ModelsController(IModelsService modelsService)
        {
            this._modelsService = modelsService;
        }

        [HttpGet]
        public MetadataModel[] Models()
        {
            return _modelsService.GetModels();
        }

        [HttpPost]
        public void Models([FromBody] MetadataModel[] models)
        {
            // var a = models.ToString();
            // var m = JsonConvert.DeserializeObject<MetadataModel[]>(a);
            _modelsService.SaveModels(models);
        }

        [HttpGet]
        public void Export() 
        {
            string models = _modelsService.GetModelsJson();
            var byteArray = Encoding.UTF8.GetBytes(models);
            var date = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            HttpContext.Response.ContentType = "application/octet-stream";
            HttpContext.Response.ContentLength = byteArray.Length;
            HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename=\"model_{date}.json\"");
            HttpContext.Response.Body.Write(byteArray, 0, byteArray.Length);
        }
    }
}