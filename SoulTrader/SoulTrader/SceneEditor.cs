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

        private Vector2 firstCorner;
        private Vector2 secondCorner;

        public void AddNewScene(Scene scene)
        {
            this.scene = scene;
        }

        public void Update()
        {
            MouseState mouse = Mouse.GetState();
            switch (state)
            {
                case TempObjectState.NONE:
                    if (InViewport(mouse) && mouse.LeftButton == ButtonState.Pressed)
                    {
                        state = TempObjectState.DRAGGING;
                        firstCorner = WorldSpaceMouseClick(mouse);
                        Console.WriteLine("MOUSE DOWN AT: " + mouse.X + " " + (scene.Camera.Viewport.Height - mouse.Y));
                    }
                    break;
                case TempObjectState.DRAGGING:
                    if (InViewport(mouse) && mouse.LeftButton == ButtonState.Released)
                    {
                        state = TempObjectState.NONE;
                        secondCorner = WorldSpaceMouseClick(mouse);
                        Console.WriteLine("MOUSE UP AT: " + mouse.X + " " + (scene.Camera.Viewport.Height - mouse.Y));
                        AddNewObject();
                    }
                    break;
            }
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
    }
}
