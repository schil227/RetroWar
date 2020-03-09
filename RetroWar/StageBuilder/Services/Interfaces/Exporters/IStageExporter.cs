using RetroWar.Models.Level;

namespace StageBuilder.Services.Interfaces.Exporters
{
    public interface IStageExporter
    {
        void ExportStageJson(Stage stage, string stageName);
    }
}
