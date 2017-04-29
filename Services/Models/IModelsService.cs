using yDevs.Model.MetadataModel;

namespace yDam.Services.Models
{
    public interface IModelsService
    {
        MetadataModel[] GetModels();
        string GetModelsJson();
        void SaveModels(MetadataModel[] models);
    }
}