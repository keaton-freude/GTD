using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GTD.BO;
using GTD.BOL;

namespace GTD
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        MouseState prevMouseState;
        KeyboardState prevKeyboardState;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Grid grid;
        Texture2D greenSquare;
        Map map;

        Tower tower;

        EnemyManager enemyManager;

        Enemy enemy;

        public static ContentManager sharedContent;

        Vector2 startPosition = new Vector2(0, 0);
        Vector2 endPosition = new Vector2(0, 0);

        public Vector2 DrawLocation = Vector2.Zero;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1249;
            graphics.PreferredBackBufferHeight = 721;
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            
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
            sharedContent = Content;
            grid = new Grid(GraphicsDevice, Color.Black);
            greenSquare = Content.Load<Texture2D>(@"GreenSquare");
            // TODO: use this.Content to load your game content here
            enemy = new Enemy(@"whiteSquare");
            enemy.Position = new Vector2(48 * 3.0f, 48 * 3.0f);
            enemy.Speed = 550f;
            map = new Map(26, 14);
            map.squareTexture = Content.Load<Texture2D>(@"GrassTile1");
            PathFinder.map = map;
            PathFinder.RebuildMap();
            enemyManager = new EnemyManager();

            tower = new Tower("Turret", Vector2.Zero);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        List<SearchNode> result = new List<SearchNode>();
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            int x = Mouse.GetState().X / 48;
            int y = Mouse.GetState().Y / 48;

            if (x > 25)
                x = 25;
            if (y > 14)
                y = 14;

            if (prevKeyboardState.IsKeyDown(Keys.P) && Keyboard.GetState().IsKeyUp(Keys.P))
            {
                enemyManager.AddEnemyTest();
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                tower.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }

            enemyManager.Update(gameTime);
            // TODO: Add your update logic here
            prevMouseState = Mouse.GetState();
            prevKeyboardState = Keyboard.GetState();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            //spriteBatch.Draw(greenSquare, DrawLocation, Color.White);
            map.Draw(spriteBatch);
            //grid.Draw(spriteBatch);
            enemyManager.Draw(spriteBatch);
            tower.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
