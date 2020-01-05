using RetroWar.Models.Screen;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Textures;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.UserInterface;

namespace RetroWar.Services.Implementations.UserInterface
{
    public class ScreenService : IScreenService
    {
        private readonly ISpriteHelper spriteHelper;

        public ScreenService(ISpriteHelper spriteHelper)
        {
            this.spriteHelper = spriteHelper;
        }

        public void ScrollScreen(Screen screen, Vehicle playerTank)
        {
            if (playerTank.X - screen.X < screen.Width * 0.2)
            {
                screen.X -= (int)((screen.Width * 0.2) - (playerTank.X - screen.X));
            }
            else if (playerTank.X - screen.X > screen.Width * 0.8)
            {
                screen.X += (int)((playerTank.X - screen.X) - screen.Width * 0.8);
            }

            if (playerTank.Y - screen.Y < screen.Height * 0.2)
            {
                screen.Y -= (int)((screen.Height * 0.2) - (playerTank.Y - screen.Y));
            }
            else if (playerTank.Y - screen.Y > screen.Height * 0.8)
            {
                screen.Y += (int)((playerTank.Y - screen.Y) - screen.Height * 0.8);
            }
        }

        public bool IsOnScreen(Screen screen, Sprite sprite)
        {
            var textureData = spriteHelper.GetCurrentTextureData(sprite);

            foreach (var texture in textureData)
            {
                if (TextureOnScreen(screen, sprite, texture))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TextureOnScreen(Screen screen, Sprite sprite, TextureData texture)
        {
            if (screen.X > sprite.X + texture.Width)
            {
                return false;
            }

            if (sprite.X > screen.X + screen.Width)
            {
                return false;
            }

            if (screen.Y > sprite.Y + texture.Height)
            {
                return false;
            }

            if (sprite.Y > screen.Y + screen.Height)
            {
                return false;
            }

            return true;
        }
    }
}
