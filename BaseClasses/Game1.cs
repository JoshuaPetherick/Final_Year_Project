using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

// All of the structure below is provided by the Monogame Framework

namespace FinalYearProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;

        public static int GAMEWIDTH = 800;
        public static int GAMEHEIGHT = 600;
        // Controls Menu states to allow for navigation
        private enum gameStates {
            MAINMENU,   // Controls Main Menu
            HOSTING,    // Enter Port Num
            JOINING,    // Enter IP and Port Num
            TESTING,    // Creates an Empty Lab Session - Similar to Playing
            PLAYING     // Playing Multipler Session
        };
        gameStates state = gameStates.TESTING;

        private Server server;
        private Camera camera;
        private World world = new World();
        private Player player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Change window size
            graphics.PreferredBackBufferWidth = GAMEWIDTH;
            graphics.PreferredBackBufferHeight = GAMEHEIGHT;
            graphics.ApplyChanges();
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
            // Load new World (Load floor and goal Textures)
            world.loadTextures(Content.Load<Texture2D>("floor"), Content.Load<Texture2D>("floor"));
            world.loadLevel();
            // Load player image across
            Tuple<int, int> playerPos = world.getPlayerPos();
            player = new Player(playerPos.Item1, playerPos.Item2);
            player.setTexture(Content.Load<Texture2D>("bot"));
            // Camera class
            camera = new Camera(GraphicsDevice.Viewport);
            // Activate Server
            if (state == gameStates.PLAYING)
            {
                server = new Server(14242, world);
                player.connectClient("127.0.0.1", 14242);
            }
            else if(state == gameStates.TESTING)
            {
                player.connectClient(true); // Running locally
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
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

            if (state == gameStates.PLAYING || state == gameStates.TESTING)
            {
                player.playerUpdate(world); // Player input handled in Update
                if (state == gameStates.PLAYING)
                {
                    server.checkMessages();
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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (state == gameStates.PLAYING || state == gameStates.TESTING)
            {
                // Camera movement
                camera.update(player.getX(), player.getY(), (float)gameTime.ElapsedGameTime.TotalSeconds, world);

                // Draw player on-screen
                spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
                world.draw(spriteBatch);
                if (state == gameStates.TESTING)
                {
                    player.drawClient(spriteBatch);
                }
                // Draw actual player on top of Client player
                player.drawPlayer(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
