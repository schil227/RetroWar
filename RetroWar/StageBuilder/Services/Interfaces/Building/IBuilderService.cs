using StageBuilder.Model.UI;

namespace StageBuilder.Services.Interfaces.Building
{
    public interface IBuilderService
    {
        void AddTileToStage(ConstructionData constructionData);
        void RemoveTileFromStage(ConstructionData constructionData);
    }
}
