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
                playerTank.X = 16;
                playerTank.Y = -16;
                playerTank.OldX = 16;
                playerTank.OldY = -16;
                playerTank.FallSum = 0;

                foreach (var actions in new List<Action>(playerTank.CurrentActions))
                {
                    actionService.RemoveAction(playerTank, actions);
                }

                actionService.SetAction(playerTank, Action.Armed);
                actionService.SetAction(playerTank, Action.Stationary);

                gridHandler.MoveSprite(contentRepository.CurrentStage.Grids, playerTank);

                var enemy = contentRepository.EnemyVehicles.First().Enemy;

                enemy.X = 112;
                enemy.Y = 120;
                enemy.OldX = 112;
                enemy.OldY = 120;
                enemy.FallSum = 0;

                foreach (var actions in new List<Action>(enemy.CurrentActions))
                {
                    actionService.RemoveAction(enemy, actions);
                }

                actionService.SetAction(enemy, Action.Armed);
                actionService.SetAction(enemy, Action.Stationary);

                gridHandler.MoveSprite(contentRepository.CurrentStage.Grids, enemy);

                processedSprites.Add(playerTank.SpriteId, "processed");

                return true;
            }

            if (keyState.IsKeyDown(Keys.W) && !playerTank.CurrentActions.Contains(Action.Destroyed))
            {
                playerTank.deltaY -= playerTank.VehicleSpeed * deltaTime;
            }

            if (keyState.IsKeyDown(Keys.A) && !playerTank.CurrentActions.Contains(Action.Destroyed))
            {
                playerTank.deltaX -= playerTank.VehicleSpeed * deltaTime;
                playerTank.CurrentDirection = Direction.Left;
            }

            if (keyState.IsKeyDown(Keys.D) && !playerTank.CurrentActions.Contains(Action.Destroyed))
            {
                playerTank.deltaX += playerTank.VehicleSpeed * deltaTime;
                playerTank.CurrentDirection = Direction.Right;
            }

            if (keyState.IsKeyDown(Keys.J) && !playerTank.CurrentActions.Contains(Action.Destroyed))
            {
                if (playerTank.FallSum == 0 && playerTank.IsJumping == false)
                {
                    playerTank.FallSum = -5;
                    playerTank.IsJumping = true;
                }
            }

            if (keyState.IsKeyDown(Keys.K) && !playerTank.CurrentActions.Contains(Action.Destroyed) && !playerTank.CurrentActions.Contains(Action.Charging))
            {
                if (playerTank.CurrentActions.Contains(Action.Armed))
                {
                    actionService.SetAction(playerTank, Action.FireStandard, Action.Armed);
                }
            }

            if (keyState.IsKeyUp(Keys.K) && playerTank.CurrentActions.Contains(Action.Charging) && !playerTank.CurrentActions.Contains(Action.Destroyed))
            {
                actionService.SetAction(playerTank, Action.FireStandard, Action.Charging);
            }

            if (keyState.IsKeyUp(Keys.K) && playerTank.CurrentActions.Contains(Action.Charged) && !playerTank.CurrentActions.Contains(Action.Destroyed))
            {
                actionService.SetAction(playerTank, Action.FireCharged, Action.Charged);
            }

            if (keyState.IsKeyDown(Keys.A) || keyState.IsKeyDown(Keys.D))
            {
                if (playerTank.CurrentActions.Contains(Action.Stationary))
                {
                    actionService.SetAction(playerTank, Action.Move, Action.Stationary);
                }
            }
            else
            {
                if (playerTank.CurrentActions.Contains(Action.Move))
                {
                    actionService.SetAction(playerTank, Action.Stationary, Action.Move);
                }
            }

            playerTank.OldX = playerTank.X;
            playerTank.OldY = playerTank.Y;

            playerTank.FallSum += System.Math.Min(playerTank.FallRate * deltaTime, 10);
            playerTank.deltaY += playerTank.FallSum;

            // Collision Handling
            playerTank.Y += playerTank.deltaY;
            playerTank.deltaY = 0;

            playerTank.X += playerTank.deltaX;
            playerTank.deltaX = 0;

            gridHandler.MoveSprite(contentRepository.CurrentStage.Grids, playerTank);

            processedSprites.Add(playerTank.SpriteId, "processed");

            return true;
        }
    }
}
