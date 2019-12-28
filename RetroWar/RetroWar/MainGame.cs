using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Level;
using RetroWar.Models.Repositories;
using RetroWar.Models.Screen;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Services.Interfaces.Collision;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.Loaders;
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

        ContentDatabase contentDatabase;
        Stage stage;
        Screen screen;
        Sprite playerSprite;
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
            IDrawService drawService
            )
        {
            this.contentLoader = contentLoader;
            this.spriteHelper = spriteHelper;
            this.collisionService = collisionService;
            this.screenService = screenService;
            this.gridHandler = gridHandler;
            this.drawService = drawService;

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
                "./Content/LoadingScripts/SpriteLoaderScript.json",
                "./Content/LoadingScripts/ActionDataLoadingScript.json",
                "./Content/LoadingScripts/TextureLoadingScript.json",
                "./Content/LoadingScripts/TileLoaderScript.json"
                );

            playerSprite = contentDatabase.Sprites.First(i => string.Equals(i.SpriteId, "tank")).Sprite;
            tiles = contentDatabase.Tiles.Where(i => i.TileId.Contains("ground"))?.Select(s => s.Tile).ToList();

            stage = new Stage();

            stage.Grids = gridHandler.InitializeGrid(playerSprite, tiles);

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
                playerSprite.X = 16;
                playerSprite.Y = 140;
                fallSum = 0;
            }

            if (keyState.IsKeyDown(Keys.W))
            {
                playerSprite.deltaY -= tankSpeed * deltaT;
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                playerSprite.deltaX -= tankSpeed * deltaT;
                playerSprite.CurrentDirection = Direction.Left;
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                playerSprite.deltaX += tankSpeed * deltaT;
                playerSprite.CurrentDirection = Direction.Right;
            }

            if (keyState.IsKeyDown(Keys.J))
            {
                if (fallSum == 0 && isJumping == false)
                {
                    fallSum = -5;
                    isJumping = true;
                }
            }

            if (keyState.IsKeyDown(Keys.A) || keyState.IsKeyDown(Keys.D))
            {
                if (playerSprite.CurrentAction == Action.Idle)
                {
                    spriteHelper.SetAction(playerSprite, Action.Move);
                }
            }
            else
            {
                if (playerSprite.CurrentAction == Action.Move)
                {
                    spriteHelper.SetAction(playerSprite, Action.Idle);
                }
            }

            var previousPlayerX = playerSprite.X;
            var previousPlayerY = playerSprite.Y;

            fallSum += Math.Min(fallRate * deltaT, 10);
            playerSprite.deltaY += fallSum;

            // Collision Handling
            playerSprite.Y += (int)playerSprite.deltaY;
            playerSprite.deltaY = 0;

            playerSprite.X += (int)playerSprite.deltaX;
            playerSprite.deltaX = 0;

            gridHandler.MoveSprite(stage.Grids, playerSprite, GridContainerSpriteType.Player, (int)previousPlayerX, (int)previousPlayerY);

            var collidedSprites = new Dictionary<string, string>();

            var boxes = gridHandler.GetGridsFromPoints(stage.Grids, screen.X, screen.Y, screen.X + screen.Width, screen.Y + screen.Height);

            foreach (var box in boxes)
            {
                if (box.PlayerSprite == null)
                {
                    continue;
                }

                foreach (var tile in (box.TileSprites.Values.ToList()))
                {
                    // Already collided, skip.
                    if (collidedSprites.ContainsKey(box.PlayerSprite.SpriteId + "_" + tile.SpriteId))
                    {
                        continue;
                    }

                    Console.WriteLine($"Num Boxes: {box.TileSprites.Values.ToList().Count()}");

                    // TODO: wrap all this up in one call to collision service
                    var collisions = collisionService.GetCollisions(playerSprite, tile);
                    if (collisions.Length > 0)
                    {
                        var beforeY = playerSprite.Y;
                        Console.WriteLine($"Found {collisions.Length} collisions {gameTime.TotalGameTime}");
                        collisionService.ResolveCollision(playerSprite, tile, collisions);

                        // resolution pushed sprite up, no longer falling
                        if (playerSprite.Y < beforeY)
                        {
                            fallSum = 0;
                            isJumping = false;
                        }
                    }

                    collidedSprites.Add(box.PlayerSprite.SpriteId + "_" + tile.SpriteId, "resolved.");
                }
            }

            screenService.ScrollScreen(screen, playerSprite);

            spriteHelper.UpdateActionSequence(playerSprite, deltaT * 1000);

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
