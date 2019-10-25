using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroWar.Models.Repositories;
using RetroWar.Models.Repositories.Sprites;
using RetroWar.Models.Repositories.Textures;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Collision;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroWar
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private readonly ISpriteLoader spriteLoader;
        private readonly IActionDataLoader actionDataLoader;
        private readonly ITextureLoader textureLoader;
        private readonly ISpriteHelper spriteHelper;
        private readonly ICollisionService collisionService;

        SpriteDatabase spriteDatabase;
        ActionDataDatabase actionDataDatabase;
        TextureDatabase textureDatabase;

        Sprite playerSprite;
        List<Sprite> groundSprites;

        //Texture2D tankTexture;
        //Texture2D groundTexture;
        //Vector2 tankPosition;
        //List<Vector2> groundPositions;
        float tankSpeed;

        float imageScaleX = 1.0f;
        float imageScaleY = 1.0f;

        public MainGame(
            ISpriteLoader spriteLoader,
            IActionDataLoader actionDataLoader,
            ITextureLoader textureLoader,
            ISpriteHelper spriteHelper,
            ICollisionService collisionService
            )
        {
            this.spriteLoader = spriteLoader;
            this.actionDataLoader = actionDataLoader;
            this.textureLoader = textureLoader;
            this.spriteHelper = spriteHelper;
            this.collisionService = collisionService;

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
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 256;
            graphics.PreferredBackBufferHeight = 240;
            graphics.ApplyChanges();


            //tankPosition = new Vector2(graphics.PreferredBackBufferWidth / 4, graphics.PreferredBackBufferHeight / 4);
            tankSpeed = 100f;

            Console.WriteLine($"Width: {graphics.PreferredBackBufferWidth }, Height: {graphics.PreferredBackBufferHeight }");

            //groundPositions = new List<Vector2>
            //{
            //    new Vector2(0,226),
            //    new Vector2(16,226),
            //    new Vector2(32,226),
            //    new Vector2(48,226),
            //    new Vector2(64,226),
            //    new Vector2(80,194)
            //};

            spriteDatabase = new SpriteDatabase();
            actionDataDatabase = new ActionDataDatabase();
            textureDatabase = new TextureDatabase();

            spriteDatabase.SpriteDatabaseItems = spriteLoader.LoadSprites("./Content/LoadingScripts/SpriteLoaderScript.json");
            actionDataDatabase.ActionDataDatabaseItems = actionDataLoader.LoadActionData("./Content/LoadingScripts/ActionDataLoadingScript.json");
            textureDatabase.TextureDatabaseItems = textureLoader.LoadTextures("./Content/LoadingScripts/TextureLoadingScript.json", Content);

            foreach (var spriteData in spriteDatabase.SpriteDatabaseItems)
            {
                spriteData.Sprite.ActionDataSet = actionDataDatabase.ActionDataDatabaseItems.First(a => string.Equals(spriteData.Sprite.ActionDataSetId, a.ActionDataId)).ActionData;
            }

            playerSprite = spriteDatabase.SpriteDatabaseItems.First(i => string.Equals(i.SpriteId, "tank")).Sprite;
            groundSprites = spriteDatabase.SpriteDatabaseItems.Where(i => i.SpriteId.Contains("ground"))?.Select(s => s.Sprite).ToList();

            if (groundSprites.Count == 0)
            {
                throw new Exception("No ground sprites found.");
            }

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

            if (keyState.IsKeyDown(Keys.W))
            {
                playerSprite.Y -= tankSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (keyState.IsKeyDown(Keys.S))
            {
                playerSprite.Y += tankSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                playerSprite.X -= tankSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                playerSprite.X += tankSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Collision Handling

            foreach (var ground in groundSprites)
            {
                // TODO: wrap all this up in one call to collision service
                var collisions = collisionService.GetCollisions(playerSprite, ground);
                if (collisions.Length > 0)
                {
                    Console.WriteLine($"Found {collisions.Length} collisions");
                    collisionService.ResolveCollision(playerSprite, ground, collisions);
                }
            }

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

            //foreach (var position in groundPositions)
            //{
            //    spriteBatch.Draw(groundTexture, position, Color.White);
            //}

            //spriteBatch.Draw(tankTexture, tankPosition, Color.White);

            foreach (var ground in groundSprites)
            {
                var textures = spriteHelper.GetCurrentTextureData(ground);

                foreach (var texture in textures)
                {
                    var position = new Vector2(ground.X + 16 * texture.RelativeX, ground.Y + 16 * texture.RelativeY);
                    spriteBatch.Draw(textureDatabase.TextureDatabaseItems.First(t => string.Equals(t.TextureId, texture.TextureId)).Texture, position, Color.White);
                }
            }

            var playerTextures = spriteHelper.GetCurrentTextureData(playerSprite);

            foreach (var texture in playerTextures)
            {
                var position = new Vector2(playerSprite.X + 16 * texture.RelativeX, playerSprite.Y + 16 * texture.RelativeY);
                spriteBatch.Draw(textureDatabase.TextureDatabaseItems.First(t => string.Equals(t.TextureId, texture.TextureId)).Texture, position, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
