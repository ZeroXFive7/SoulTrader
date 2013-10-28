using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SoulTrader.GameObjects.Actors.Projectiles
{
    public class MoneyBall : Projectile
    {
        public int Value { get { return value; } }
        private int value = 0;

        public bool HasLeftPlayer = false;
        private bool collidedWithPlayer = false;

        public MoneyBall(string spriteName, int value, Player parent, Vector2 initialVelocity, Vector2 initialPosition, Vector2 size)
            : base(spriteName, parent, initialVelocity, initialPosition, size)
        {
            this.value = value;
        }

        protected override bool OnGameObjectCollision(GameObject collider)
        {
            if (collider is Player)
            {
                collidedWithPlayer = true;
                return true;
            }

            return base.OnGameObjectCollision(collider);
        }

        protected override void PostCollisionOperation()
        {
            if (!collidedWithPlayer)
            {
                HasLeftPlayer = true;
            }
            collidedWithPlayer = false;
        }
    }
}
