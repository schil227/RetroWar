using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
using RetroWar.Services.Interfaces.Updaters;
using RetroWar.Services.Interfaces.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly ISpriteUpdater spriteUpdaterComposite;

        ContentDatabase contentDatabase;
        Stage stage;
        Screen screen;
        Vehicle playerTank;
        List<Tile> tiles;

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
            IBulletHelper bulletHelper,
            ISpriteUpdater spriteUpdaterComposite
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
            this.spriteUpdaterComposite = spriteUpdaterComposite;

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

            Console.WriteLine($"Width: {graphics.PreferredBackBufferWidth }, Height: {graphics.PreferredBackBufferHeight }");

            contentDatabase = contentLoader.LoadAllData(
                Content,
                "./Content/LoadingScripts/PlayerLoaderScript.json",
                "./Content/LoadingScripts/EnemyLoaderScript.json",
                "./Content/LoadingScripts/ActionDataLoadingScript.json",
                "./Content/LoadingScripts/TextureLoadingScript.json",
                "./Content/LoadingScripts/TileLoaderScript.json",
                "./Content/LoadingScripts/BulletLoaderScript.json"
                );

            playerTank = contentDatabase.PlayerVehicles.First(i => string.Equals(i.PlayerId, "tank")).Player;
            tiles = contentDatabase.Tiles.Where(i => i.TileId.Contains("ground"))?.Select(s => s.Tile).ToList();

            stage = new Stage();

            stage.Grids = gridHandler.InitializeGrid(playerTank, contentDatabase.EnemyVehicles.Select(e => e.Enemy), tiles);

            contentRepository.Actions = contentDatabase.Actions;
            contentRepository.PlayerVehicles = contentDatabase.PlayerVehicles;
            contentRepository.EnemyVehicles = contentDatabase.EnemyVehicles;
            contentRepository.Textures = contentDatabase.Textures;
            contentRepository.Tiles = contentDatabase.Tiles;
            contentRepository.Bullets = contentDatabase.Bullets;
            contentRepository.CurrentStage = stage;
            contentRepository.Screen = screen;
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

            var deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var boxes = gridHandler.GetGridsFromPoints(stage.Grids, screen.X, screen.Y, screen.X + screen.Width, screen.Y + screen.Height);

            var sprites = new HashSet<Sprite>();

            foreach (var box in boxes)
            {
                if (box.playerTank == null && box.Bullets.Count == 0)
                {
                    continue;
                }

                if (box.playerTank != null)
                {
                    sprites.Add(box.playerTank);
                }

                foreach (var bullet in box.Bullets)
                {
                    sprites.Add(bullet.Value);
                }

                foreach (var tile in box.Tiles)
                {
                    sprites.Add(tile.Value);
                }
            }

            var updatedSprites = new Dictionary<string, string>();

            foreach (var sprite in sprites)
            {
                spriteUpdaterComposite.UpdateSprite(sprite, deltaT, updatedSprites);
            }

            var collidedSprites = new Dictionary<string, string>();

            foreach (var normal in sprites)
            {
                foreach (var based in sprites)
                {
                    if (normal == based
                        || (normal is Tile && based is Tile)
                        || collidedSprites.ContainsKey(normal.SpriteId + based.SpriteId) || collidedSprites.ContainsKey(based.SpriteId + normal.SpriteId))
                    {
                        continue;
                    }

                    var collisions = collisionService.GetCollisions(normal, based);

                    if (collisions.Length > 0)
                    {
                        var beforeY = playerTank.Y;
                        collisionService.ResolveCollision(normal, based, collisions);

                        // resolution pushed player vehicle up, no longer falling
                        // Note: Open this up for all vehicles, not just player 
                        //      (need to expand vehicle class)
                        if (((normal == playerTank && based is Tile) || (based == playerTank && normal is Tile)) && playerTank.Y < beforeY)
                        {
                            playerTank.FallSum = 0;
                            playerTank.IsJumping = false;
                        }

                    }
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
