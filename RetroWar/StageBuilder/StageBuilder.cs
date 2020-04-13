using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroWar.Models.Level;
using RetroWar.Models.Repositories;
using RetroWar.Models.Screen;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Loaders;
using RetroWar.Services.Interfaces.Repositories;
using RetroWar.Services.Interfaces.UserInterface;
using StageBuilder.Model.UI;
using StageBuilder.Services.Interfaces.Building;
using StageBuilder.Services.Interfaces.Exporters;
using StageBuilder.Services.Interfaces.UI;
using StageBuilder.Services.Interfaces.Updaters;
using System;
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
        private readonly ICursorUpdater cursorUpdater;
        private readonly IStageBuilderDrawingService stageBuilderDrawingService;
        private readonly IBuilderService builderService;
        public readonly IStageExporter stageExporter;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ContentDatabase contentDatabase;
        Screen screen;
        ConstructionData constructionData;

        float imageScaleX = 1.0f;
        float imageScaleY = 1.0f;

        public StageBuilder(
            IContentLoader contentLoader,
            IContentRepository contentRepository,
            IDrawService drawService,
            IGridHandler gridHandler,
            IScreenService screenService,
            IStageBuilderDrawingService stageBuilderDrawingService,
            IInputService inputService,
            ICursorUpdater cursorUpdater,
            IBuilderService builderService,
            IStageExporter stageExporter
            )
        {
            this.contentLoader = contentLoader;
            this.contentRepository = contentRepository;
            this.drawService = drawService;
            this.gridHandler = gridHandler;
            this.screenService = screenService;
            this.stageBuilderDrawingService = stageBuilderDrawingService;
            this.cursorUpdater = cursorUpdater;
            this.inputService = inputService;
            this.builderService = builderService;
            this.stageExporter = stageExporter;

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
                "./Content/LoadingScripts/UniqueTileLoaderScript.json",
                "./Content/LoadingScripts/BulletLoaderScript.json",
                "./Content/LoadingScripts/IllusionLoaderScript.json",
                "./Content/LoadingScripts/Stages/StageLoaderScript.json",
                "./Content/LoadingScripts/Stages/"
                );

            constructionData = new ConstructionData();
            constructionData.Tiles = contentDatabase.Tiles.Where(i => i.TileId.Contains("ground"))?.Select(s => s.Tile).ToList();
            constructionData.TileIndex = 0;

            constructionData.Cursor = contentDatabase.Illusions.First(i => i.IllusionId == "Cursor").Illusion;

            constructionData.Stage = new Stage();
            gridHandler.InitializeGrid(constructionData.Stage, contentDatabase.PlayerVehicles.First().Player, contentDatabase.EnemyVehicles.Select(e => e.Enemy));

            gridHandler.MoveSprite(constructionData.Stage.Grids, constructionData.Cursor);

            contentRepository.Actions = contentDatabase.Actions;
            contentRepository.PlayerVehicles = contentDatabase.PlayerVehicles;
            contentRepository.EnemyVehicles = contentDatabase.EnemyVehicles;
            contentRepository.Textures = contentDatabase.Textures;
            contentRepository.Tiles = contentDatabase.Tiles;
            contentRepository.Bullets = contentDatabase.Bullets;
            contentRepository.Illusions = contentDatabase.Illusions;
            contentRepository.CurrentStage = constructionData.Stage;
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

            cursorUpdater.UpdateCursor(constructionData.Cursor, constructionData.Stage);

            if (inputService.KeyJustPressed(Keys.Left))
            {
                constructionData.TileIndex = (constructionData.TileIndex == 0 ? constructionData.Tiles.Count - 1 : constructionData.TileIndex - 1);
            }

            if (inputService.KeyJustPressed(Keys.Right))
            {
                constructionData.TileIndex = (constructionData.TileIndex == constructionData.Tiles.Count - 1 ? 0 : constructionData.TileIndex + 1);
            }

            if (inputService.KeyJustPressed(Keys.Down) &&
                (inputService.KeyPressed(Keys.LeftShift) || inputService.KeyPressed(Keys.RightShift)))
            {
                builderService.RemoveTileFromStage(constructionData);
            }
            else if (inputService.KeyJustPressed(Keys.Down))
            {
                builderService.AddTileToStage(constructionData);
            }

            if (inputService.KeyJustPressed(Keys.Up))
            {
                builderService.RemoveTileFromStage(constructionData);
            }

            if (inputService.KeyJustPressed(Keys.E))
            {
                stageExporter.ExportStageJson(constructionData.Stage, "STAGE_OUTPUT.json");
            }

            screenService.ScrollScreen(screen, constructionData.Cursor);

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
            drawService.DrawScreen(spriteBatch, constructionData.Stage, screen, contentDatabase.Textures);

            stageBuilderDrawingService.DrawStageBuilderUI(spriteBatch, constructionData.Stage, screen, contentDatabase.Textures, constructionData);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
