using RetroWar.Models.Screen;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Textures;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.UserInterface;

namespace RetroWar.Services.Implementations.UserInterface
{
    public class ScreenService : IScreenService
    {

        private const double ScreenLowerBound = 0.3;
        private const double ScreenUpperBound = 0.7;

        private readonly ISpriteHelper spriteHelper;

        public ScreenService(ISpriteHelper spriteHelper)
        {
            this.spriteHelper = spriteHelper;
        }

        public void ScrollScreen(Screen screen, Sprite focusedSprite)
        {
            if (focusedSprite.X - screen.X < screen.Width * ScreenLowerBound)
            {
                screen.X -= (int)((screen.Width * ScreenLowerBound) - (focusedSprite.X - screen.X));
            }
            else if (focusedSprite.X - screen.X > screen.Width * ScreenUpperBound)
            {
                screen.X += (int)((focusedSprite.X - screen.X) - screen.Width * ScreenUpperBound);
            }

            if (focusedSprite.Y - screen.Y < screen.Height * ScreenLowerBound)
            {
                screen.Y -= (int)((screen.Height * ScreenLowerBound) - (focusedSprite.Y - screen.Y));
            }
            else if (focusedSprite.Y - screen.Y > screen.Height * ScreenUpperBound)
            {
                screen.Y += (int)((focusedSprite.Y - screen.Y) - screen.Height * ScreenUpperBound);
            }
        }

        public bool IsOnScreen(Screen screen, Sprite sprite)
        {
            var textureData = spriteHelper.GetCurrentTextureData(sprite);

            foreach (var texture in textureData)
            {
                if (TextureOnScreen(screen, sprite, texture.Key))
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
