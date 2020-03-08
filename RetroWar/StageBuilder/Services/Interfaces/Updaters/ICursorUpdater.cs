using RetroWar.Models.Level;
using RetroWar.Models.Sprites.Illusions;

namespace StageBuilder.Services.Interfaces.Updaters
{
    public interface ICursorUpdater
    {
        void UpdateCursor(Illusion cursor, Stage stage);
    }
}
