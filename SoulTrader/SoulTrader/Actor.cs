using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SoulTrader
{
    public class Actor : GameObject
    {
        #region Enums

        private enum Side { TOP, LEFT, RIGHT, BOTTOM };

        #endregion

        #region forces

        private Vector2 gravity = new Vector2(0.0f, -20.0f);

        protected Vector2 velocity = Vector2.Zero;
        protected bool onGround = false;

        #endregion

        #region Properties

        private Vector2 spawnPoint;

        #endregion

        public Actor(string spriteName, Vector2 initialPosition, Vector2 size) 
            : base(spriteName, initialPosition, size)
        {
            spawnPoint = initialPosition;
        }

        public override void Update(TimeSpan timeSinceLastFrame)
        {
            if (!onGround)
            {
                velocity += gravity * (float)timeSinceLastFrame.TotalSeconds;
                velocity.X *= 0.8f;
            }
            else
            {
                velocity.X *= 0.9f;
            }

            onGround = false;

            UpdatePosition(worldPosition + velocity);

            base.Update(timeSinceLastFrame);
        }

        public override void HandleCollisions()
        {
            foreach (GameObject collider in Colliders)
            {
                if (collider is KillZone)
                {
                    Respawn();
                    base.HandleCollisions();
                    return;
                }
                if (collider is Obstacle)
                {
                    Obstacle obstacle = collider as Obstacle;
                    switch (CollisionSide(obstacle))
                    {
                        case Side.BOTTOM:
                            worldPosition.Y = obstacle.TopRightPosition.Y;
                            if (velocity.Y < 0.0f)
                            {
                                velocity.Y = 0.0f;
                            }
                            onGround = true;
                            break;
                        case Side.LEFT:
                            worldPosition.X = obstacle.TopRightPosition.X;
                            if (velocity.X < 0.0f)
                            {
                                velocity.X = 0.0f;
                            }
                            break;
                        case Side.RIGHT:
                            worldPosition.X = obstacle.BottomLeftPosition.X - worldScale.X;
                            if (velocity.X > 0.0f)
                            {
                                velocity.X = 0.0f;
                            }
                            break;
                        case Side.TOP:
                            worldPosition.Y = obstacle.BottomLeftPosition.Y - worldScale.Y;
                            if (velocity.Y > 0.0f)
                            {
                                velocity.Y = 0.0f;
                            }
                            break;
                    }
                }
            }
            UpdatePosition(worldPosition);

            base.HandleCollisions();
        }

        protected virtual void Respawn()
        {
            UpdatePosition(spawnPoint);
        }

        protected virtual void UpdatePosition(Vector2 newPosition)
        {
            worldPosition = newPosition;
            physicsChild.BoundingBox = new BoundingBox(new Vector3(worldPosition, 0.0f), new Vector3(worldPosition + worldScale, 0.0f));
        }

        private Side CollisionSide(Obstacle obstacle)
        {
            Vector2 size = 0.5f * (obstacle.Scale + this.Scale);
            Vector2 deltaPosition = obstacle.CenterPosition - this.CenterPosition;

            float widthY = size.X * deltaPosition.Y;
            float heightX = size.Y * deltaPosition.X;

            if (widthY > heightX)
            {
                if (widthY > -heightX)
                {
                    return Side.TOP;
                }
                else
                {
                    return Side.LEFT;
                }
            }
            else
            {
                if (widthY > -heightX)
                {
                    return Side.RIGHT;
                }
                else
                {
                    return Side.BOTTOM;
                }
            }
        }
    }
}
