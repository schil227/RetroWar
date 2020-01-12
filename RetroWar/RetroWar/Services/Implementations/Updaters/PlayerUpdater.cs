using Microsoft.Xna.Framework.Input;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Actions;
using RetroWar.Models.Vehicles.Vehicles.PlayerVehicle;
using RetroWar.Services.Interfaces.Actions;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Repositories;
using RetroWar.Services.Interfaces.Updaters;
using System.Collections.Generic;
using System.Linq;

namespace RetroWar.Services.Implementations.Updaters
{
    public class PlayerUpdater : ISpriteUpdater
    {
        private readonly IActionService actionService;
        private readonly IContentRepository contentRepository;
        private readonly IGridHandler gridHandler;

        public PlayerUpdater
            (
                IActionService actionService,
                IContentRepository contentRepository,
                IGridHandler gridHandler
            )
        {
            this.actionService = actionService;
            this.contentRepository = contentRepository;
            this.gridHandler = gridHandler;
        }

        public bool UpdateSprite(Sprite sprite, float deltaTime, Dictionary<string, string> processedSprites)
        {
            PlayerVehicle playerTank = null;

            if (sprite is PlayerVehicle)
            {
                playerTank = (PlayerVehicle)sprite;
            }
            else
            {
                return false;
            }

            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.R))
            {
                var oldX = (int)playerTank.X;
                var oldY = (int)playerTank.Y;

                playerTank.X = 16;
                playerTank.Y = 180;
                playerTank.FallSum = 0;

                gridHandler.MoveSprite(contentRepository.CurrentStage.Grids, playerTank, oldX, oldY);

                var enemy = contentRepository.EnemyVehicles.First().Enemy;

                oldX = (int)enemy.X;
                oldY = (int)enemy.Y;

                enemy.X = 112;
                enemy.Y = 120;
                enemy.FallSum = 0;

                gridHandler.MoveSprite(contentRepository.CurrentStage.Grids, enemy, oldX, oldY);

                processedSprites.Add(playerTank.SpriteId, "processed");

                return true;
            }

            if (keyState.IsKeyDown(Keys.W))
            {
                playerTank.deltaY -= playerTank.VehicleSpeed * deltaTime;
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                playerTank.deltaX -= playerTank.VehicleSpeed * deltaTime;
                playerTank.CurrentDirection = Direction.Left;
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                playerTank.deltaX += playerTank.VehicleSpeed * deltaTime;
                playerTank.CurrentDirection = Direction.Right;
            }

            if (keyState.IsKeyDown(Keys.J))
            {
                if (playerTank.FallSum == 0 && playerTank.IsJumping == false)
                {
                    playerTank.FallSum = -5;
                    playerTank.IsJumping = true;
                }
            }

            if (keyState.IsKeyDown(Keys.K))
            {
                if (playerTank.CurrentAction != Action.FireStandard)
                {
                    actionService.SetAction(playerTank, Action.FireStandard);
                }
            }

            if (keyState.IsKeyDown(Keys.A) || keyState.IsKeyDown(Keys.D))
            {
                if (playerTank.CurrentAction == Action.Idle)
                {
                    actionService.SetAction(playerTank, Action.Move);
                }
            }
            else
            {
                if (playerTank.CurrentAction == Action.Move)
                {
                    actionService.SetAction(playerTank, Action.Idle);
                }
            }

            var previousPlayerX = playerTank.X;
            var previousPlayerY = playerTank.Y;

            playerTank.FallSum += System.Math.Min(playerTank.FallRate * deltaTime, 10);
            playerTank.deltaY += playerTank.FallSum;

            // Collision Handling
            playerTank.Y += (int)playerTank.deltaY;
            playerTank.deltaY = 0;

            playerTank.X += (int)playerTank.deltaX;
            playerTank.deltaX = 0;

            gridHandler.MoveSprite(contentRepository.CurrentStage.Grids, playerTank, (int)previousPlayerX, (int)previousPlayerY);

            processedSprites.Add(playerTank.SpriteId, "processed");

            return true;
        }
    }
}
