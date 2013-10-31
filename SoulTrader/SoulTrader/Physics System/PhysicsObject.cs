using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SoulTrader
{
    public class PhysicsObject
    {
        public int NumColliders { get { return parent.NumColliders; } }

        public Vector2 MinCorner { get { return minCorner; } }
        public Vector2 MaxCorner { get { return maxCorner; } }

        private Vector2 minCorner;
        private Vector2 maxCorner;

        public Vector2 Position
        { 
            get
            { 
                return minCorner; 
            } 
            set 
            { 
                minCorner = value; 
                maxCorner = minCorner + Scale; 
            } 
        }

        public Vector2 Scale { get { return scale; } set { Scale = value; maxCorner = minCorner + Scale; } }
        private Vector2 scale;

        public GameObject Parent { get { return this.parent; } }
        private GameObject parent;

        public PhysicsObject(GameObject parent, Vector2 position, Vector2 size)
        {
            this.parent = parent;
            this.scale = size;
            this.Position = position;
        }

        public void AddCollider(PhysicsObject collider)
        {
            parent.AddCollider(collider.parent);
        }

        public bool Intersects(PhysicsObject collider)
        {
            return this.minCorner.X < collider.maxCorner.X && this.maxCorner.X > collider.minCorner.X &&
                   this.minCorner.Y < collider.maxCorner.Y && this.maxCorner.Y > collider.minCorner.Y;
        }
    }
}
