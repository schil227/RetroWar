using RetroWar.Models.Screen;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Vehicles;

namespace RetroWar.Services.Interfaces.UserInterface
{
    public interface IScreenService
    {
        void ScrollScreen(Screen screen, Vehicle playerTank);
        bool IsOnScreen(Screen screen, Sprite sprite);
    }
}
