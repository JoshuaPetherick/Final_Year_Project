using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

// All of the structure below is provided by the Monogame Framework

namespace Anti_Latency
{
    /// The primary class for game. Holds and manages all the crucial elements of the game.
    public class Game1 : Game
    {
        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;

        public static int GAMEWIDTH = 800;
        public static int GAMEHEIGHT = 600;
        private string ip = "127.0.0.1";
        private int port = 14242;
        // Controls Menu states to allow for navigation
        private enum gameStates {
            MAINMENU,    // Controls Main Menu
            HOSTING,     // Hosting Multiplayer Session
            JOINING,     // Join Multiplayer Session
            TESTINGMENU, // Configure parameters for Test room
            TESTING,     // Creates an Empty Lab Session - Similar to Playing
        };
        gameStates state = gameStates.MAINMENU;

        private List<Button> buttons = new List<Button>();
        private World world = new World();
        private Thread serverThread;
        private SpriteFont font;
        private Server server;
        private Camera camera;
        private Player player;
        private Texture2D background;
        private int tech = 0;
        private int delay = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// Sets the graphics to the prefered window size (800x600)
        protected override void Initialize()
        {
            // Change window size
            graphics.PreferredBackBufferWidth = GAMEWIDTH;
            graphics.PreferredBackBufferHeight = GAMEHEIGHT;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// LoadContent will be called to load content based on State.
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            buttons.Clear();
            //if (state != gameStates.PLAYING && state != gameStates.TESTING)
            if (state == gameStates.MAINMENU)
            {
                IsMouseVisible = true;
                // Load font
                font = Content.Load<SpriteFont>("my_font");
                // Load buttons
                buttons.Add(new Button(350, 200, "HOST"));
                buttons.Add(new Button(350, 250, "JOIN"));
                buttons.Add(new Button(350, 300, "TESTING"));
            }
            else if (state == gameStates.TESTINGMENU)
            {
                IsMouseVisible = true;
                // Load buttons
                buttons.Add(new Button(300, 200, "CLIENT-SIDE PREDICTION"));
                buttons.Add(new Button(300, 250, "SERVER RECONCILIATION"));
                //  Delay Parameters
                buttons.Add(new Button(300, 300, "DELAY (Milliseconds)"));
                buttons.Add(new Button(310, 340, "<"));
                buttons.Add(new Button(330, 340, "0"));
                buttons.Add(new Button(410, 340, ">"));
                buttons.Add(new Button(670, 550, "START"));
                buttons.Add(new Button(20, 550, "BACK"));
            }
            else
            {
                IsMouseVisible = false;
                // Load static background
                background = Content.Load<Texture2D>("background");
                // Load new World (Load floor and goal Textures)
                world.loadTextures(Content.Load<Texture2D>("ground"), Content.Load<Texture2D>("goal"), Content.Load<Texture2D>("flag"));
                world.loadLevel();
                // Load player image across
                Tuple<int, int> playerPos = world.getPlayerPos();
                player = new Player(playerPos.Item1, playerPos.Item2);
                player.setTexture(Content.Load<Texture2D>("bot"));
                // Camera class
                camera = new Camera(GraphicsDevice.Viewport);
                // Activate Server
                if (state != gameStates.TESTING)
                {
                    tech = 2; // Auto set to Server Reconcilliation
                    player.setTechnique(tech);
                    if (state == gameStates.HOSTING)
                    {
                        server = new Server(port, world);
                        serverThread = new Thread(new ThreadStart(server.update));
                        serverThread.Start(); // Start independent thread 
                    }
                    player.connectClient(ip, port);
                }
                else if (state == gameStates.TESTING)
                {
                    player.setTechnique(tech);
                    player.connectClient(true, world, delay); // Running locally
                }
            }
        }

        /// Called last, will close server and server thread if running as Host.
        protected override void UnloadContent()
        {
            if (state == gameStates.HOSTING)
            {
                server.closeServer(); // Close server
                serverThread.Abort(); // Stop Thread
            }
        }

        /// Allows the game to run logic such as updating the player, buttons being clicked, etc.
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            // If button has been clicked
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                int mouseX = Mouse.GetState().X;
                int mouseY = Mouse.GetState().Y;
                if (state == gameStates.MAINMENU)
                {
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        Button tempButton = buttons[i];
                        int result = Logic.axisAlignedBoundingBox(mouseX, mouseY, 1, 1, tempButton.getX(), tempButton.getY(), tempButton.getHeight(), tempButton.getWidth());
                        if (result > 0)
                        {
                            switch (i)
                            {
                                case 0:
                                    state = gameStates.HOSTING;
                                    tech = 2;
                                    break;
                                case 1:
                                    state = gameStates.JOINING;
                                    tech = 2;
                                    break;
                                case 2:
                                    state = gameStates.TESTINGMENU;
                                    break;
                            }
                            LoadContent();
                            break;
                        }
                    }
                }
                else if (state == gameStates.TESTINGMENU)
                {
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        Button tempButton = buttons[i];
                        int result = Logic.axisAlignedBoundingBox(mouseX, mouseY, 1, 1, tempButton.getX(), tempButton.getY(), tempButton.getHeight(), tempButton.getWidth());
                        if (result > 0)
                        {
                            switch (i)
                            {
                                // Client Side Prediction
                                case 0:
                                    tech = 1;
                                    buttons[1].buttonClicked(false);
                                    buttons[i].buttonClicked(true);
                                    break;
                                // Server Reconcilliation
                                case 1:
                                    tech = 2;
                                    buttons[0].buttonClicked(false);
                                    buttons[i].buttonClicked(true);
                                    break;
                                // <
                                case 3:
                                    if (delay > 0)
                                    {
                                        delay--;
                                    }
                                    buttons[4].setText(delay.ToString());
                                    break;                
                                // >
                                case 5:
                                    delay++;
                                    buttons[4].setText(delay.ToString());
                                    break;
                                // START
                                case 6:
                                    state = gameStates.TESTING;
                                    LoadContent();
                                    break;
                                // BACK
                                case 7:
                                    state = gameStates.MAINMENU;
                                    LoadContent();
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
            if (state != gameStates.MAINMENU && state != gameStates.TESTINGMENU)
            {
                player.playerUpdate(world); // Player input handled in Update
            }
            
            base.Update(gameTime);
        }

        /// Called every frame to draw the contents of the objects (Based on state)
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (state != gameStates.MAINMENU && state != gameStates.TESTINGMENU)
            {
                // Camera movement
                camera.update(player.getX(), player.getY(), (float)gameTime.ElapsedGameTime.TotalSeconds, world);
                // Begin drawing
                spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
                // Draw static background
                spriteBatch.Draw(background, new Rectangle(camera.getX(world), 0, GAMEWIDTH, GAMEHEIGHT), Color.GhostWhite);
                // Draw world then player on-screen
                world.draw(spriteBatch);
                player.drawClient(spriteBatch);
                // Draw actual player on top of other players
                player.drawPlayer(spriteBatch);
            }
            else
            {
                GraphicsDevice.Clear(Color.DarkBlue);
                spriteBatch.Begin();
                for(int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].draw(spriteBatch, font);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
