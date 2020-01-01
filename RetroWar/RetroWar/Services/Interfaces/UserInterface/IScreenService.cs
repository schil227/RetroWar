using RetroWar.Models.Screen;
using RetroWar.Models.Sprites;

namespace RetroWar.Services.Interfaces.UserInterface
{
    public interface IScreenService
    {
        void ScrollScreen(Screen screen, Sprite playerSprite);
        bool IsOnScreen(Screen screen, Sprite sprite);
    }
}
