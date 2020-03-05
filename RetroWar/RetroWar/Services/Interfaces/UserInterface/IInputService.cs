using Microsoft.Xna.Framework.Input;

namespace RetroWar.Services.Interfaces.UserInterface
{
    public interface IInputService
    {
        void LoadKeys(KeyboardState keyState);
        bool KeyJustPressed(Keys key);
    }
}
