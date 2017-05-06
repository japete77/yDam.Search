using System.IO;
using yDevs.Model.MetadataModel;

namespace yDam.Services.Models
{
    public interface IModelsService
    {
        MetadataModel[] GetModels();
        string GetModelsJson();
        Stream GetModelsZip();
        void UpdateModels(MetadataModel[] models);
        void UpdateModels(byte[] models);
    }
}