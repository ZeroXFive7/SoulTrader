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
using SoulTrader.Input_System;
using SoulTrader.SceneSystem;
using SoulTrader.Souls;

namespace SoulTrader
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SoulTrader : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public SoulTrader()
        {
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += ResizedWindow;

            Mouse.WindowHandle = Window.Handle;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ActiveScene.Editor = new SceneEditor();
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
            ActiveScene.Camera = new Camera(GraphicsDevice.Viewport);
            ActiveScene.Scene = new Scene(ActiveScene.Camera);
            ActiveScene.Scene.Load("Content/Levels/default.lvl");

            if (ActiveScene.Editor != null)
            {
                ActiveScene.Editor.AddNewScene(ActiveScene.Scene);
            }

            ActiveScene.Player = ActiveScene.Scene.Player;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            InputSystem.Initialize();
            GraphicsSystem.Initialize(spriteBatch, Content);
            PhysicsSystem.Initialize();
            ActiveScene.Scene.Initialize();

            if (ActiveScene.Editor != null)
            {
                ActiveScene.Editor.Initialize(GraphicsDevice, spriteBatch);
            }

            ActiveScene.Scene.AddAndInitialize(new NPC("white", new DoubleJumpSoul(null), 200, new Vector2(500, 50), new Vector2(50, 50)));
            ActiveScene.Scene.AddAndInitialize(new NPC("white", new GroundPoundSoul(null), 200, new Vector2(700, 50), new Vector2(50, 50)));

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
            if (ActiveScene.Player.Money <= 0 || Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                //scene.Save("../../../../SoulTraderContent/Levels/default.lvl");
                this.Exit();
            }

            InputSystem.Update();

            if (ActiveScene.Editor != null)
            {
                ActiveScene.Editor.Update();
            }

            GraphicsSystem.Update(ActiveScene.Camera.ViewTransform);
            ActiveScene.Scene.Update(gameTime);
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

            ActiveScene.Scene.Render();

            GraphicsSystem.RenderGraphicsObjects();

            if (ActiveScene.Editor != null)
            {
                ActiveScene.Editor.Render();
            }

            Viewport viewport = GraphicsDevice.Viewport;
            int offset = viewport.Width / 100;
            int width = viewport.Width / 20;
            int stringWidth = viewport.Width / 5;
            int stringHeight = viewport.Height / 10;

            GraphicsSystem.RenderUI("dollar", new Rectangle(offset, 2 * offset, width, width));
            GraphicsSystem.RenderString(ActiveScene.Player.Money.ToString(), new Vector2(offset + width, 2 * offset));

            Vector2 soulPosition = new Vector2(viewport.Width - 2 * stringWidth - offset, 2 * offset);
            foreach (Soul soul in ActiveScene.Player.Portfolio)
            {
                string[] nameParts = soul.ToString().Split('.');
                GraphicsSystem.RenderString(nameParts[2], soulPosition);
                soulPosition += new Vector2(0.0f, stringHeight);
            }

            base.Draw(gameTime);
        }

        protected void ResizedWindow(object sender, EventArgs e)
        {
            var safeWidth = Math.Max(this.Window.ClientBounds.Width, 1);
            var safeHeight = Math.Max(this.Window.ClientBounds.Height, 1);
            var newViewport = new Viewport(0, 0, safeWidth, safeHeight) { MinDepth = 0.0f, MaxDepth = 1.0f };

            var presentationParams = GraphicsDevice.PresentationParameters;
            presentationParams.BackBufferWidth = safeWidth;
            presentationParams.BackBufferHeight = safeHeight;
            presentationParams.DeviceWindowHandle = this.Window.Handle;
            GraphicsDevice.Reset(presentationParams);

            GraphicsDevice.Viewport = newViewport;
            ActiveScene.Camera.Viewport = newViewport;
        }
    }
}
