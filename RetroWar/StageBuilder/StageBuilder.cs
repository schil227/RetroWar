using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroWar.Models.Level;
using RetroWar.Models.Repositories;
using RetroWar.Models.Screen;
using RetroWar.Models.Sprites.Illusions;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Loaders;
using RetroWar.Services.Interfaces.Repositories;
using RetroWar.Services.Interfaces.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StageBuilder
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class StageBuilder : Game
    {
        private readonly IContentLoader contentLoader;
        private readonly IContentRepository contentRepository;
        private readonly IDrawService drawService;
        private readonly IGridHandler gridHandler;
        private readonly IScreenService screenService;
        private readonly IInputService inputService;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ContentDatabase contentDatabase;
        Stage stage;
        Screen screen;
        List<Tile> tiles;
        Illusion Cursor;

        float imageScaleX = 1.0f;
        float imageScaleY = 1.0f;

        public StageBuilder(
            IContentLoader contentLoader,
            IContentRepository contentRepository,
            IDrawService drawService,
            IGridHandler gridHandler,
            IScreenService screenService,
            IInputService inputService
            )
        {
            this.contentLoader = contentLoader;
            this.contentRepository = contentRepository;
            this.drawService = drawService;
            this.gridHandler = gridHandler;
            this.screenService = screenService;
            this.inputService = inputService;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"C:\Users\Adrian\source\repos\RetroWar\RetroWar\RetroWar\Content\bin\DesktopGL\Content";
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

            contentDatabase = contentLoader.LoadAllData(
                Content,
                "./Content/LoadingScripts/PlayerLoaderScript.json",
                "./Content/LoadingScripts/EnemyLoaderScript.json",
                "./Content/LoadingScripts/ActionDataLoadingScript.json",
                "./Content/LoadingScripts/TextureLoadingScript.json",
                "./Content/LoadingScripts/TileLoaderScript.json",
                "./Content/LoadingScripts/BulletLoaderScript.json",
                "./Content/LoadingScripts/IllusionLoaderScript.json"
                );

            tiles = contentDatabase.Tiles.Where(i => i.TileId.Contains("ground"))?.Select(s => s.Tile).ToList();

            Cursor = contentDatabase.Illusions.First(i => i.IllusionId == "Cursor").Illusion;

            stage = new Stage();
            stage.Grids = gridHandler.InitializeGrid(contentDatabase.PlayerVehicles.First().Player, contentDatabase.EnemyVehicles.Select(e => e.Enemy), tiles);

            gridHandler.MoveSprite(stage.Grids, Cursor, 0, 0);

            contentRepository.Actions = contentDatabase.Actions;
            contentRepository.PlayerVehicles = contentDatabase.PlayerVehicles;
            contentRepository.EnemyVehicles = contentDatabase.EnemyVehicles;
            contentRepository.Textures = contentDatabase.Textures;
            contentRepository.Tiles = contentDatabase.Tiles;
            contentRepository.Bullets = contentDatabase.Bullets;
            contentRepository.Illusions = contentDatabase.Illusions;
            contentRepository.CurrentStage = stage;
            contentRepository.Screen = screen;

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
            var keyState = Keyboard.GetState();
            inputService.LoadKeys(keyState);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (inputService.KeyJustPressed(Keys.W))
            {
                Cursor.deltaY -= 16;
            }

            if (inputService.KeyJustPressed(Keys.S))
            {
                Cursor.deltaY += 16;
            }

            if (inputService.KeyJustPressed(Keys.A))
            {
                Cursor.deltaX -= 16;
            }

            if (inputService.KeyJustPressed(Keys.D))
            {
                Cursor.deltaX += 16;
            }

            if (Cursor.deltaX != 0 || Cursor.deltaY != 0)
            {
                var previousX = Cursor.X;
                var previousY = Cursor.Y;

                Cursor.X += Cursor.deltaX;
                Cursor.Y += Cursor.deltaY;

                Cursor.deltaX = 0;
                Cursor.deltaY = 0;

                gridHandler.MoveSprite(stage.Grids, Cursor, (int)previousX, (int)previousY);
            }

            screenService.ScrollScreen(screen, Cursor);

            // TODO: Add your update logic here

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

            // TODO: Add your drawing code here
            drawService.DrawScreen(spriteBatch, stage, screen, contentDatabase.Textures);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
