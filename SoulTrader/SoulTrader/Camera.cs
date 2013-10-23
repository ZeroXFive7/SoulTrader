using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoulTrader
{
    public class Camera
    {
        private Viewport viewport;

        private float nearThreshold = 0.4f;
        private float farThreshold = 0.6f;
        private Vector2 bottomLeft = Vector2.Zero;

        public Matrix ViewTransform { get { return Matrix.CreateTranslation(new Vector3(-this.bottomLeft.X, this.bottomLeft.Y, 0.0f)); } }

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
            this.bottomLeft = new Vector2(0.0f, this.viewport.Height);
        }

        public void Update(Vector2 targetPosition)
        {
            float nearDistance = nearThreshold * viewport.Width + bottomLeft.X;
            float farDistance = farThreshold * viewport.Width + bottomLeft.X;

            if (targetPosition.X < nearDistance)
            {
                bottomLeft.X = targetPosition.X - nearThreshold * viewport.Width;
            }
            else if (targetPosition.X > farDistance)
            {
                bottomLeft.X = targetPosition.X - farThreshold * viewport.Width;
            }
        }
    }
}
