﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Level;
using RetroWar.Models.Repositories;
using RetroWar.Models.Screen;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Services.Interfaces.Actions;
using RetroWar.Services.Interfaces.Collision;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.Loaders;
using RetroWar.Services.Interfaces.Repositories;
using RetroWar.Services.Interfaces.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = RetroWar.Models.Sprites.Actions.Action;

namespace RetroWar
{
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private readonly ISpriteHelper spriteHelper;
        private readonly ICollisionService collisionService;
        private readonly IScreenService screenService;
        private readonly IGridHandler gridHandler;
        private readonly IContentLoader contentLoader;
        private readonly IDrawService drawService;
        private readonly ISequenceService sequenceService;
        private readonly IActionService actionService;
        private readonly IContentRepository contentRepository;
        private readonly IBulletHelper bulletHelper;

        ContentDatabase contentDatabase;
        Stage stage;
        Screen screen;
        Vehicle playerTank;
        List<Tile> tiles;

        float tankSpeed;
        float fallSum = 0;
        float fallRate = 10;
        bool isJumping = false;

        float imageScaleX = 1.0f;
        float imageScaleY = 1.0f;

        public MainGame(
            IContentLoader contentLoader,
            ISpriteHelper spriteHelper,
            ICollisionService collisionService,
            IScreenService screenService,
            IGridHandler gridHandler,
            IDrawService drawService,
            ISequenceService sequenceService,
            IActionService actionService,
            IContentRepository contentRepository,
            IBulletHelper bulletHelper
            )
        {
            this.contentLoader = contentLoader;
            this.spriteHelper = spriteHelper;
            this.collisionService = collisionService;
            this.screenService = screenService;
            this.gridHandler = gridHandler;
            this.drawService = drawService;
            this.sequenceService = sequenceService;
            this.actionService = actionService;
            this.contentRepository = contentRepository;
            this.bulletHelper = bulletHelper;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
        }

        private void OnResize(Object sender, EventArgs e)
        {
            Console.WriteLine("In the resize event");

            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;

            graphics.ApplyChanges();

            imageScaleX = (Window.ClientBounds.Width * 1.0f) / 256.0f;
            imageScaleY = (Window.ClientBounds.Height * 1.0f) / 240.0f;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            screen = new Screen
            {
                X = 0,
                Y = 0
            };

            graphics.PreferredBackBufferWidth = screen.Width;
            graphics.PreferredBackBufferHeight = screen.Height;
            graphics.ApplyChanges();

            imageScaleX = (Window.ClientBounds.Width * 1.0f) / screen.Width * 1.0f;
            imageScaleY = (Window.ClientBounds.Height * 1.0f) / screen.Height * 1f;

            tankSpeed = 100f;

            Console.WriteLine($"Width: {graphics.PreferredBackBufferWidth }, Height: {graphics.PreferredBackBufferHeight }");

            contentDatabase = contentLoader.LoadAllData(
                Content,
                "./Content/LoadingScripts/VehicleLoaderScript.json",
                "./Content/LoadingScripts/ActionDataLoadingScript.json",
                "./Content/LoadingScripts/TextureLoadingScript.json",
                "./Content/LoadingScripts/TileLoaderScript.json",
                "./Content/LoadingScripts/BulletLoaderScript.json"
                );

            playerTank = contentDatabase.Vehicles.First(i => string.Equals(i.VehicleId, "tank")).Vehicle;
            tiles = contentDatabase.Tiles.Where(i => i.TileId.Contains("ground"))?.Select(s => s.Tile).ToList();

            stage = new Stage();

            stage.Grids = gridHandler.InitializeGrid(playerTank, tiles);

            contentRepository.Actions = contentDatabase.Actions;
            contentRepository.Vehicles = contentDatabase.Vehicles;
            contentRepository.Textures = contentDatabase.Textures;
            contentRepository.Tiles = contentDatabase.Tiles;
            contentRepository.Bullets = contentDatabase.Bullets;
            contentRepository.CurrentStage = stage;
            contentRepository.PlayerTank = playerTank;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //tankTexture = Content.Load<Texture2D>("Sprites/tankv1");
            //groundTexture = Content.Load<Texture2D>("Sprites/ground1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var keyState = Keyboard.GetState();

            var deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyState.IsKeyDown(Keys.R))
            {
                playerTank.X = 16;
                playerTank.Y = 140;
                fallSum = 0;
            }

            if (keyState.IsKeyDown(Keys.W))
            {
                playerTank.deltaY -= tankSpeed * deltaT;
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                playerTank.deltaX -= tankSpeed * deltaT;
                playerTank.CurrentDirection = Direction.Left;
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                playerTank.deltaX += tankSpeed * deltaT;
                playerTank.CurrentDirection = Direction.Right;
            }

            if (keyState.IsKeyDown(Keys.J))
            {
                if (fallSum == 0 && isJumping == false)
                {
                    fallSum = -5;
                    isJumping = true;
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

            if (keyState.IsKeyDown(Keys.Right))
            {
                screen.X += 10;
            }

            var previousPlayerX = playerTank.X;
            var previousPlayerY = playerTank.Y;

            fallSum += Math.Min(fallRate * deltaT, 10);
            playerTank.deltaY += fallSum;

            // Collision Handling
            playerTank.Y += (int)playerTank.deltaY;
            playerTank.deltaY = 0;

            playerTank.X += (int)playerTank.deltaX;
            playerTank.deltaX = 0;

            gridHandler.MoveSprite(stage.Grids, playerTank, GridContainerSpriteType.Player, (int)previousPlayerX, (int)previousPlayerY);

            var collidedSprites = new Dictionary<string, string>();
            var processedBullets = new Dictionary<string, string>();

            var boxes = gridHandler.GetGridsFromPoints(stage.Grids, screen.X, screen.Y, screen.X + screen.Width, screen.Y + screen.Height);

            foreach (var box in boxes)
            {
                var bullets = box.Bullets.Values.ToList();

                foreach (var bullet in bullets)
                {
                    if (processedBullets.ContainsKey(bullet.SpriteId))
                    {
                        continue;
                    }

                    var oldX = (int)bullet.X;
                    var oldY = (int)bullet.Y;

                    var newPoint = bulletHelper.FindNextPointInTrajectory(bullet, deltaT);

                    bullet.X = newPoint.X;
                    bullet.Y = newPoint.Y;

                    if (!screenService.IsOnScreen(screen, bullet))
                    {
                        gridHandler.RemoveSpriteFromGrid(stage.Grids, bullet, GridContainerSpriteType.Bullet, oldX, oldY);
                        continue;
                    }

                    gridHandler.MoveSprite(stage.Grids, bullet, GridContainerSpriteType.Bullet, oldX, oldY);

                    processedBullets.Add(bullet.SpriteId, "Processed");
                }
            }

            //foreach (var bullet in bullets)
            //{
            //    if (!screenService.IsOnScreen(screen, bullet))
            //    {
            //        Console.WriteLine($"Num bullets: {bulletsList.Count}");
            //        gridHandler.RemoveSpriteFromGrid(stage.Grids, bullet, GridContainerSpriteType.Bullet);
            //        continue;
            //    }

            //    var oldX = bullet.X;
            //    var oldY = bullet.Y;

            //    var newPoint = bulletHelper.FindNextPointInTrajectory(bullet, deltaT);

            //    bullet.X = newPoint.X;
            //    bullet.Y = newPoint.Y;

            //    if (!screenService.IsOnScreen(screen, bullet))
            //    {
            //        Console.WriteLine($"Num bullets: {bulletsList.Count}");
            //        bullet.X = oldX;
            //        bullet.Y = oldY;
            //        gridHandler.RemoveSpriteFromGrid(stage.Grids, bullet, GridContainerSpriteType.Bullet);
            //        continue;
            //    }

            //    gridHandler.MoveSprite(stage.Grids, bullet, GridContainerSpriteType.Bullet, (int)oldX, (int)oldY);
            //}

            foreach (var box in boxes)
            {
                if (box.playerTank == null)
                {
                    continue;
                }

                foreach (var tile in (box.Tiles.Values.ToList()))
                {
                    // Already collided, skip.
                    if (collidedSprites.ContainsKey(box.playerTank.SpriteId + "_" + tile.SpriteId))
                    {
                        continue;
                    }

                    Console.WriteLine($"Num Boxes: {box.Tiles.Values.ToList().Count()}");

                    // TODO: wrap all this up in one call to collision service
                    var collisions = collisionService.GetCollisions(playerTank, tile);
                    if (collisions.Length > 0)
                    {
                        var beforeY = playerTank.Y;
                        Console.WriteLine($"Found {collisions.Length} collisions {gameTime.TotalGameTime}");
                        collisionService.ResolveCollision(playerTank, tile, collisions);

                        // resolution pushed sprite up, no longer falling
                        if (playerTank.Y < beforeY)
                        {
                            fallSum = 0;
                            isJumping = false;
                        }
                    }

                    collidedSprites.Add(box.playerTank.SpriteId + "_" + tile.SpriteId, "resolved.");
                }
            }

            screenService.ScrollScreen(screen, playerTank);

            sequenceService.UpdateActionSequence(playerTank, deltaT * 1000);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(164, 228, 252));

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(imageScaleX, imageScaleY, 1.0f));

            drawService.DrawScreen(spriteBatch, stage, screen, contentDatabase.Textures);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
