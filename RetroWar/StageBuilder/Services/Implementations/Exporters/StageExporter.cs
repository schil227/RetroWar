using Newtonsoft.Json;
using RetroWar.Models.Level;
using StageBuilder.Services.Interfaces.Exporters;
using System;
using System.IO;

namespace StageBuilder.Services.Implementations.Exporters
{
    public class StageExporter : IStageExporter
    {
        private readonly string ExportPath = AppDomain.CurrentDomain.BaseDirectory;

        public void ExportStageJson(Stage stage, string stageName)
        {
            var directory = Directory.GetParent(ExportPath).Parent.Parent.Parent.Parent.FullName;

            var stageJson = JsonConvert.SerializeObject(stage, Formatting.Indented);

            File.WriteAllText(directory + @"\Export\" + stageName, stageJson);
        }
    }
}
