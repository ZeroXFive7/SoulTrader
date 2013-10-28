using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SoulTrader.SceneSystem;

namespace SoulTrader.GameObjects.Actors
{
    public class Projectile : Actor
    {
        public Player Parent { get { return parent; } }
        private Player parent;

        public Projectile(string spriteName, Player parent, Vector2 initialVelocity, Vector2 initialPosition, Vector2 size)
            : base(spriteName, initialPosition, size)
        {
            this.parent = parent;
            velocity = initialVelocity;
        }

        protected override bool OnGameObjectCollision(GameObject collider)
        {
            if (collider is KillZone)
            {
                ActiveScene.Scene.RemoveAndUninitialize(this);
                return false;
            }

 	        return base.OnGameObjectCollision(collider);
        }
    }
}
