using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoulTrader
{
    public class SceneEditor
    {
        private enum TempObjectState { NONE, DRAGGING };

        private TempObjectState state = TempObjectState.NONE;

        private Scene scene;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        private Vector2 firstCorner;
        private Vector2 secondCorner;

        private int blockDimension = 25;
        private int vertexRange = 10;
        private Texture2D lineTexture;

        public void AddNewScene(Scene scene)
        {
            this.scene = scene;
        }

        public void Initialize(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;

            lineTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            lineTexture.SetData(new[] {Color.DimGray});
        }

        public void Update()
        {
            MouseState mouse = Mouse.GetState();
            if (!InViewport(mouse))
            {
                return;
            }

            Vector2 mousePosition = WorldSpaceMouseClick(mouse);
            Vector2 nearestVertex = NearestVertex(mousePosition);

            if ((mousePosition - nearestVertex).Length() > vertexRange)
            {
                return;
            }

            switch (state)
            {
                case TempObjectState.NONE:
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        state = TempObjectState.DRAGGING;
                        firstCorner = nearestVertex;
                    }
                    break;
                case TempObjectState.DRAGGING:
                    if (mouse.LeftButton == ButtonState.Released)
                    {
                        state = TempObjectState.NONE;
                        secondCorner = nearestVertex;
                        AddNewObject();
                    }
                    break;
            }
        }

        public void Render()
        {
            spriteBatch.Begin();

            int initX = (int)scene.Camera.BottomLeftPosition.X / blockDimension * blockDimension;
            for (int x = 0; x < scene.Camera.Viewport.Width + blockDimension; x += blockDimension )
            {
                int xSS = initX + x - (int)scene.Camera.BottomLeftPosition.X;
                drawLine(new Vector2(xSS, scene.Camera.Viewport.Height), new Vector2(xSS + 1, 0.0f));
            }

            int initY = (int)scene.Camera.BottomLeftPosition.Y / blockDimension * blockDimension;
            for (int y = scene.Camera.Viewport.Height; y >= 0; y -= blockDimension)
            {
                int ySS = initY + y - (int)scene.Camera.BottomLeftPosition.Y;
                drawLine(new Vector2(0.0f, ySS + 1.0f), new Vector2(scene.Camera.Viewport.Width, ySS));
            }
            spriteBatch.End();
        }

        private Vector2 WorldSpaceMouseClick(MouseState mouse)
        {
            Vector2 mouseSS = new Vector2(mouse.X, scene.Camera.Viewport.Height - mouse.Y);

            return scene.Camera.BottomLeftPosition + mouseSS;
        }

        private void AddNewObject()
        {
            Vector2 bottomLeftCorner = new Vector2(Math.Min(firstCorner.X, secondCorner.X), Math.Min(firstCorner.Y, secondCorner.Y));
            Vector2 topRightCorner = new Vector2(Math.Max(firstCorner.X, secondCorner.X), Math.Max(firstCorner.Y, secondCorner.Y));

            scene.AddAndInitialize(new Obstacle("green", bottomLeftCorner, topRightCorner - bottomLeftCorner));
        }

        private bool InViewport(MouseState mouse)
        {
            return mouse.X >= 0 && mouse.X <= scene.Camera.Viewport.Width && mouse.Y >= 0 && mouse.Y <= scene.Camera.Viewport.Height;
        }

        private Vector2 NearestVertex(Vector2 mouseWS)
        {
            int nearestX = (int)mouseWS.X % blockDimension < blockDimension / 2 ? (int)mouseWS.X / blockDimension * blockDimension : (int)mouseWS.X / blockDimension * (blockDimension + 1);
            int nearestY = (int)mouseWS.Y % blockDimension < blockDimension / 2 ? (int)mouseWS.Y / blockDimension * blockDimension : (int)mouseWS.Y / blockDimension * (blockDimension + 1);
            return new Vector2(nearestX, nearestY);
        }

        private void drawLine(Vector2 bottomLeft, Vector2 topRight)
        {
            spriteBatch.Draw(lineTexture, new Rectangle((int)bottomLeft.X, (int)topRight.Y, (int)topRight.X - (int)bottomLeft.X, (int)bottomLeft.Y - (int)topRight.Y), Color.White);
        }
    }
}
