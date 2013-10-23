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

namespace SoulTrader
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<GameObject> gameObjectCollection = new List<GameObject>();
        Camera mainCamera;
        Player player;

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
            mainCamera = new Camera(GraphicsDevice.Viewport);

            player = new Player(mainCamera, "red", new Vector2(75.0f, 75.0f), new Vector2(50.0f, 50.0f));
            gameObjectCollection.Add(player);

            gameObjectCollection.Add(new Obstacle("green", new Vector2(0.0f, 0.0f), new Vector2(1000.0f, 50.0f)));
            gameObjectCollection.Add(new Obstacle("green", new Vector2(200.0f, 50.0f), new Vector2(50.0f, 100.0f)));

            gameObjectCollection.Add(new KillZone("green", new Vector2(-5000.0f, -200.0f), new Vector2(10000.0f, 100.0f)));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {   
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GraphicsSystem.Initialize(spriteBatch, Content);
            PhysicsSystem.Initialize();

            foreach (GameObject obj in gameObjectCollection)
            {
                GraphicsSystem.RegisterGraphicsObject(obj.GraphicsObject);
                PhysicsSystem.RegisterPhysicsObject(obj.PhysicsObject);
            }

            GraphicsSystem.RegisterSpriteByName("dollar");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            if (player.Money <= 0 || Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            GraphicsSystem.Update(mainCamera.ViewTransform);

            foreach (GameObject obj in gameObjectCollection)
            {
                obj.Update(gameTime.ElapsedGameTime);
            }

            PhysicsSystem.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (GameObject obj in gameObjectCollection)
            {
                obj.Render();
            }

            GraphicsSystem.RenderGraphicsObjects();
            
            Viewport viewport = GraphicsDevice.Viewport;
            int offset = viewport.Width / 100;
            int width = viewport.Width / 20;

            GraphicsSystem.RenderUI("dollar", new Rectangle(offset, 2 * offset, width, width));
            GraphicsSystem.RenderString(player.Money.ToString(), new Vector2(offset + width, 2 * offset));

            base.Draw(gameTime);
        }
    }
}
