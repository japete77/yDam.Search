using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using yDam.Services.Models;
using yDevs.Model.MetadataModel;

namespace yDam.Dam.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ModelsController: Controller
    {
        private IModelsService _modelsService;
        public ModelsController(IModelsService modelsService)
        {
            this._modelsService = modelsService;
        }

        [HttpGet]
        public MetadataModel[] Get()
        {
            return _modelsService.GetModels();
        }

        [HttpPost]
        public void Save(IFormFile file)
        {
            //_modelsService.SaveModels(models);
            if (file == null) throw new Exception("File is null");
            if (file.Length == 0) throw new Exception("File is empty");

            using (Stream stream = file.OpenReadStream())
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    var fileContent = binaryReader.ReadBytes((int)file.Length);
                    _modelsService.SaveModels(fileContent);
                }
            }
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