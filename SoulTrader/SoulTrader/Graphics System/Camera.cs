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
        public Viewport Viewport { get { return viewport; } set { viewport = value; } }
        private Viewport viewport;

        private float velocity = 5000.0f;

        private float nearThreshold = 0.4f;
        private float farThreshold = 0.6f;
        private Vector2 nearLineOffset { get { return new Vector2(nearThreshold * viewport.Width, 0.0f); } }
        private Vector2 farLineOffset { get { return new Vector2(farThreshold * viewport.Width, 0.0f); } }

        public Vector2 BottomLeftPosition { get { return bottomLeftPosition; } }
        private Vector2 bottomLeftPosition = Vector2.Zero;

        public Matrix ViewTransform { get { return Matrix.CreateTranslation(new Vector3(-this.bottomLeftPosition.X, viewport.Height, 0.0f)); } }

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public void Update(Vector2 targetPosition, TimeSpan deltaTime)
        {
            Vector2 nearLine = bottomLeftPosition + nearLineOffset;
            Vector2 farLine = bottomLeftPosition + farLineOffset;

            if (targetPosition.X < nearLine.X)
            {
                updatePosition(targetPosition - nearLineOffset, deltaTime.TotalSeconds);
            }
            else if (targetPosition.X > farLine.X)
            {
                updatePosition(targetPosition - farLineOffset, deltaTime.TotalSeconds);
            }
        }

        private void updatePosition(Vector2 targetPosition, double deltaSeconds)
        {
            Vector2 offset = Vector2.Normalize(targetPosition - bottomLeftPosition);
            bottomLeftPosition.X += offset.X * velocity * (float)deltaSeconds;
        }
    }
}
