using RetroWar.Models.Screen;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Textures;
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
