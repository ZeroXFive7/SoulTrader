﻿using System;
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

        private Vector2 gravity = new Vector2(0.0f, -40.0f);

        protected Vector2 velocity = Vector2.Zero;
        protected bool onGround = false;

        private float groundFriction = 10.0f;
        private float airFriction = 8.5f;

        #endregion

        #region Properties


        #endregion

        public Actor(string spriteName, Vector2 initialPosition, Vector2 size) 
            : base(spriteName, initialPosition, size) { }

        public override void Update(TimeSpan timeSinceLastFrame)
        {
            //UpdatePosition(worldPosition);

            float deltaSeconds = (float)timeSinceLastFrame.TotalSeconds;
  
            if (!onGround)
            {
                velocity += gravity * deltaSeconds;
                velocity.X *= (1.0f - airFriction * deltaSeconds);
            }
            else
            {
                velocity.X *= (1.0f - groundFriction * deltaSeconds);
            }

            onGround = false;

            UpdatePosition(worldPosition + velocity);

            base.Update(timeSinceLastFrame);
        }

        protected override bool OnGameObjectCollision(GameObject collider)
        {
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
                return true;
            }
            
            return base.OnGameObjectCollision(collider);
        }

        protected virtual void UpdatePosition(Vector2 newPosition)
        {
            worldPosition = newPosition;
            physicsChild.Position = worldPosition;
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
