using Microsoft.Xna.Framework.Input;
using RetroWar.Services.Interfaces.UserInterface;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.UserInterface
{
    public class InputService : IInputService
    {
        ISet<Keys> PreviousState { get; set; }
        ISet<Keys> CurrentState { get; set; }

        public void LoadKeys(KeyboardState keyboardState)
        {
            var keys = keyboardState.GetPressedKeys();

            PreviousState = CurrentState;
            CurrentState = new HashSet<Keys>();

            foreach (var key in keys)
            {
                CurrentState.Add(key);
            }
        }

        public bool KeyJustPressed(Keys key)
        {
            return CurrentState.Contains(key) && !PreviousState.Contains(key);
        }

        public bool KeyPressed(Keys key)
        {
            return CurrentState.Contains(key) || (PreviousState != null && PreviousState.Contains(key));
        }
    }
}
