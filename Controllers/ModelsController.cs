using System;
using System.IO;
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
        public FileStreamResult Export()
        {
            string models = _modelsService.GetModelsJson();
            var date = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            return new FileStreamResult(_modelsService.GetModelsZip(), "application/octet-stream")
            {
                FileDownloadName = $"models_{date}.zip"
            };
        }

        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}