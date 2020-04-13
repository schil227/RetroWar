using Microsoft.Xna.Framework.Input;
using RetroWar.Models.Level;
using RetroWar.Models.Sprites.Illusions;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.UserInterface;
using StageBuilder.Services.Interfaces.Updaters;

namespace StageBuilder.Services.Implementations.Updaters
{
    public class CursorUpdater : ICursorUpdater
    {
        private readonly IInputService inputService;
        private readonly IGridHandler gridHandler;

        public CursorUpdater(
            IInputService inputService,
            IGridHandler gridHandler
            )
        {
            this.inputService = inputService;
            this.gridHandler = gridHandler;
        }

        public void UpdateCursor(Illusion cursor, Stage stage)
        {
            if (inputService.KeyJustPressed(Keys.W))
            {
                cursor.deltaY -= 16;
            }

            if (inputService.KeyJustPressed(Keys.S))
            {
                cursor.deltaY += 16;
            }

            if (inputService.KeyJustPressed(Keys.A))
            {
                cursor.deltaX -= 16;
            }

            if (inputService.KeyJustPressed(Keys.D))
            {
                cursor.deltaX += 16;
            }

            if (cursor.deltaX != 0 || cursor.deltaY != 0)
            {
                cursor.X += cursor.deltaX;
                cursor.Y += cursor.deltaY;

                cursor.deltaX = 0;
                cursor.deltaY = 0;

                gridHandler.MoveSprite(stage.Grids, cursor);
            }
        }
    }
}
