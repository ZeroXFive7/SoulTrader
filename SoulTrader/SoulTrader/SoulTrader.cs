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
    public class SoulTrader : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SceneEditor editor = null;

        Scene scene = null;
        Camera mainCamera = null;
        Player player = null;

        public SoulTrader()
        {
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += ResizedWindow;

            Mouse.WindowHandle = Window.Handle;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

//#if (EDITOR)
            editor = new SceneEditor();
            this.IsMouseVisible = true;
//#endif
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
            scene = new Scene(mainCamera);
            scene.Load("Content/Levels/default.lvl");

            if (editor != null)
            {
                editor.AddNewScene(scene);
            }

            player = scene.Player;

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
            scene.Initialize();

            if (editor != null)
            {
                editor.Initialize(GraphicsDevice, spriteBatch);
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
            {
                //scene.Save("../../../../SoulTraderContent/Levels/default.lvl");
                this.Exit();
            }

            if (editor != null)
            {
                editor.Update();
            }

            GraphicsSystem.Update(mainCamera.ViewTransform);
            scene.Update(gameTime);
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

            scene.Render();

            GraphicsSystem.RenderGraphicsObjects();

            if (editor != null)
            {
                editor.Render();
            }

            Viewport viewport = GraphicsDevice.Viewport;
            int offset = viewport.Width / 100;
            int width = viewport.Width / 20;

            GraphicsSystem.RenderUI("dollar", new Rectangle(offset, 2 * offset, width, width));
            GraphicsSystem.RenderString(player.Money.ToString(), new Vector2(offset + width, 2 * offset));

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
            mainCamera.Viewport = newViewport;
        }
    }
}
