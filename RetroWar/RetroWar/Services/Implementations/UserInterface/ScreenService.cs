using RetroWar.Models.Screen;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.UserInterface;

namespace RetroWar.Services.Implementations.UserInterface
{
    public class ScreenService : IScreenService
    {
        public void ScrollScreen(Screen screen, Sprite playerSprite)
        {
            if (playerSprite.X - screen.X < screen.Width * 0.2)
            {
                screen.X -= (int)((screen.Width * 0.2) - (playerSprite.X - screen.X));
            }
            else if (playerSprite.X - screen.X > screen.Width * 0.8)
            {
                screen.X += (int)((playerSprite.X - screen.X) - screen.Width * 0.8);
            }

            if (playerSprite.Y - screen.Y < screen.Height * 0.2)
            {
                screen.Y -= (int)((screen.Height * 0.2) - (playerSprite.Y - screen.Y));
            }
            else if (playerSprite.Y - screen.Y > screen.Height * 0.8)
            {
                screen.Y += (int)((playerSprite.Y - screen.Y) - screen.Height * 0.8);
            }
        }
    }
}
