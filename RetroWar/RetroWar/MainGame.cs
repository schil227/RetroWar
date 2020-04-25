using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroWar.Extensions;
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
        private readonly IGridContainerHelper gridContainerHelper;
        private readonly IActionService actionService;
        private readonly IContentRepository contentRepository;
        private readonly ISpriteUpdater spriteUpdaterComposite;
        private readonly IInputService inputService;

        private ContentDatabase contentDatabase;
        private Stage stage;
        private Screen screen;
        private Vehicle playerTank;

        private bool FrameMode = false;
        private bool AdvanceFrame = false;

        float imageScaleX = 1.0f;
        float imageScaleY = 1.0f;

        public MainGame(
            IContentLoader contentLoader,
            ISpriteHelper spriteHelper,
            ICollisionService collisionService,
            IScreenService screenService,
            IGridHandler gridHandler,
            IDrawService drawService,
            IActionService actionService,
            IContentRepository contentRepository,
            IBulletHelper bulletHelper,
            ISpriteUpdater spriteUpdaterComposite,
            IGridContainerHelper gridContainerHelper,
            IInputService inputService
            )
        {
            this.contentLoader = contentLoader;
            this.spriteHelper = spriteHelper;
            this.collisionService = collisionService;
            this.screenService = screenService;
            this.gridHandler = gridHandler;
            this.drawService = drawService;
            this.actionService = actionService;
            this.contentRepository = contentRepository;
            this.spriteUpdaterComposite = spriteUpdaterComposite;
            this.gridContainerHelper = gridContainerHelper;
            this.inputService = inputService;

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
                "./Content/LoadingScripts/BulletLoaderScript.json",
                "./Content/LoadingScripts/IllusionLoaderScript.json",
                "./Content/LoadingScripts/Stages/StageLoaderScript.json",
                "./Content/LoadingScripts/Stages/"
                );

            playerTank = contentDatabase.PlayerVehicles.First(i => string.Equals(i.PlayerId, "tank")).Player;

            stage = contentDatabase.Stages.First().Stage;

            gridHandler.InitializeGrid(stage, playerTank, contentDatabase.EnemyVehicles.Select(e => e.Enemy));

            contentRepository.Actions = contentDatabase.Actions;
            contentRepository.PlayerVehicles = contentDatabase.PlayerVehicles;
            contentRepository.EnemyVehicles = contentDatabase.EnemyVehicles;
            contentRepository.Textures = contentDatabase.Textures;
            contentRepository.Tiles = contentDatabase.Tiles;
            contentRepository.Bullets = contentDatabase.Bullets;
            contentRepository.Illusions = contentDatabase.Illusions;
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
            {
                Exit();
            }

            var keyState = Keyboard.GetState();
            inputService.LoadKeys(keyState);

            if (inputService.KeyJustPressed(Keys.P))
            {
                FrameMode = !FrameMode;
            }

            if (FrameMode && !AdvanceFrame)
            {
                if (inputService.KeyJustPressed(Keys.Enter))
                {
                    AdvanceFrame = true;
                }

                base.Update(gameTime);
                return;
            }

            AdvanceFrame = false;

            var deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var boxes = gridHandler.GetGridsFromPoints(stage.Grids, screen.X, screen.Y, screen.X + screen.Width, screen.Y + screen.Height);

            UpdateSprites(boxes, deltaT);

            var spritesToMove = CollideSprites(boxes, deltaT);

            // if sprites are updated by collisions, their boxes must be updated.
            foreach (var sprite in spritesToMove)
            {
                //gridHandler.MoveSprite(stage.Grids, sprite, (int)sprite.oldX, (int)sprite.oldY);
            }

            screenService.ScrollScreen(screen, playerTank);

            base.Update(gameTime);
        }

        private HashSet<Sprite> CollideSprites(IEnumerable<GridContainer> boxes, float deltaT)
        {
            var collidedSprites = new Dictionary<string, string>();
            var spritesToMove = new HashSet<Sprite>();
            foreach (var box in boxes)
            {
                var sprites = gridContainerHelper.GetActionableGridSprites(box);
                var movingSprites = sprites.Where(s => !(s is Tile));

                foreach (var normal in movingSprites)
                {
                    foreach (var based in sprites)
                    {
                        if (normal == based
                            || (normal is Tile && based is Tile)
                            || collidedSprites.ContainsKey(normal.SpriteId + based.SpriteId)
                            || collidedSprites.ContainsKey(based.SpriteId + normal.SpriteId))
                        {
                            continue;
                        }

                        if (collisionService.HandleCollision(normal, based, deltaT))
                        {
                            spritesToMove.Add(normal);
                            collidedSprites.Add(normal.SpriteId + based.SpriteId, "collided");
                        }
                    }
                }
            }

            return spritesToMove;
        }

        private void UpdateSprites(IEnumerable<GridContainer> boxes, float deltaT)
        {
            var sprites = new HashSet<Sprite>();
            var movingSprites = new HashSet<Sprite>();

            foreach (var box in boxes)
            {
                sprites.AddRange(gridContainerHelper.GetActionableGridSprites(box));
            }

            var updatedSprites = new Dictionary<string, string>();

            foreach (var sprite in sprites)
            {
                spriteUpdaterComposite.UpdateSprite(sprite, deltaT, updatedSprites);
            }
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
