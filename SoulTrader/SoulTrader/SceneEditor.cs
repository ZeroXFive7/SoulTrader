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

        private Vector2 selectedVertex;
        private Vector2 firstCorner;
        private Vector2 secondCorner;

        private bool isHighlighting = false;

        private int blockDimension = 25;
        private int vertexRange = 7;
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
            lineTexture.SetData(new[] {Color.White});
        }

        public void Update()
        {
            MouseState mouse = Mouse.GetState();
            if (!InViewport(mouse))
            {
                return;
            }

            Vector2 mousePosition = WorldSpaceMouseClick(mouse);
            selectedVertex = NearestVertex(mousePosition);

            isHighlighting = (mousePosition - selectedVertex).Length() <= vertexRange;

            switch (state)
            {
                case TempObjectState.NONE:
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        state = TempObjectState.DRAGGING;
                        firstCorner = selectedVertex;
                    }
                    break;
                case TempObjectState.DRAGGING:
                    if (mouse.LeftButton == ButtonState.Released)
                    {
                        state = TempObjectState.NONE;
                        secondCorner = selectedVertex;
                        AddNewObject();
                    }
                    break;
            }
        }

        public void Render()
        {
            spriteBatch.Begin();

            Vector2 ssSelectedVertex = ScreenSpaceVector(selectedVertex);
            if (isHighlighting)
            {
                drawHighlight(ssSelectedVertex, Color.Yellow);
            }

            if (state == TempObjectState.DRAGGING)
            {
                Vector2 ssFirstCorner = ScreenSpaceVector(firstCorner);
                drawHighlight(ssFirstCorner, Color.Red);
                drawTempObject(ssFirstCorner, ssSelectedVertex);
            }

            Color color = Color.DimGray;

            int initX = (int)scene.Camera.BottomLeftPosition.X / blockDimension * blockDimension;
            for (int x = 0; x < scene.Camera.Viewport.Width + blockDimension; x += blockDimension )
            {
                int xWS = initX + x;
                int xSS = xWS - (int)scene.Camera.BottomLeftPosition.X;

                color.A = (xWS % 2 == 0) ? (byte)255 : (byte)128;
                drawLine(new Vector2(xSS, scene.Camera.Viewport.Height), new Vector2(xSS + 2, 0.0f), color);
            }

            int initY = (int)scene.Camera.BottomLeftPosition.Y / blockDimension * blockDimension;
            for (int y = scene.Camera.Viewport.Height; y >= 0; y -= blockDimension)
            {
                int yWS = initY + y;
                int ySS = yWS - (int)scene.Camera.BottomLeftPosition.Y;

                color.A = (yWS % 2 == 0) ? (byte)255 : (byte)128;
                drawLine(new Vector2(0.0f, ySS + 2.0f), new Vector2(scene.Camera.Viewport.Width, ySS), color);
            }
            spriteBatch.End();
        }

        private Vector2 WorldSpaceMouseClick(MouseState mouse)
        {
            Vector2 mouseSS = new Vector2(mouse.X, scene.Camera.Viewport.Height - mouse.Y);

            return scene.Camera.BottomLeftPosition + mouseSS;
        }

        private Vector2 ScreenSpaceVector(Vector2 wsVector)
        {
            Vector2 ssVector = wsVector - scene.Camera.BottomLeftPosition;
            ssVector.Y = scene.Camera.Viewport.Height - ssVector.Y;

            return ssVector;
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
            Vector2 offset = new Vector2(blockDimension, blockDimension);

            Vector2 lowBound = new Vector2((int)mouseWS.X / blockDimension * blockDimension, (int)mouseWS.Y / blockDimension * blockDimension);
            Vector2 mid = lowBound + offset / 2;
            Vector2 upperBound = lowBound + offset;

            return new Vector2(mouseWS.X < mid.X ? lowBound.X : upperBound.X, mouseWS.Y < mid.Y ? lowBound.Y : upperBound.Y);
        }

        private void drawLine(Vector2 bottomLeft, Vector2 topRight, Color color)
        {
            spriteBatch.Draw(lineTexture, new Rectangle((int)bottomLeft.X, (int)topRight.Y, (int)topRight.X - (int)bottomLeft.X, (int)bottomLeft.Y - (int)topRight.Y), color);
        }

        private void drawHighlight(Vector2 vertexPosition, Color color)
        {
            spriteBatch.Draw(lineTexture, new Rectangle((int)vertexPosition.X - vertexRange, (int)vertexPosition.Y - vertexRange, 2 * vertexRange, 2 * vertexRange), color);
        }

        private void drawTempObject(Vector2 firstSS, Vector2 secondSS)
        {
            Vector2 topLeftCorner = new Vector2(Math.Min(firstSS.X, secondSS.X), Math.Min(firstSS.Y, secondSS.Y));
            Vector2 bottomRightCorner = new Vector2(Math.Max(firstSS.X, secondSS.X), Math.Max(firstSS.Y, secondSS.Y));

            spriteBatch.Draw(lineTexture, new Rectangle((int)topLeftCorner.X, (int)topLeftCorner.Y, (int)bottomRightCorner.X - (int)topLeftCorner.X, (int)bottomRightCorner.Y - (int)topLeftCorner.Y), Color.Yellow);
        }
    }
}
