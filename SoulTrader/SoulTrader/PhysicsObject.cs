using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SoulTrader
{
    public class PhysicsObject
    {
        public GameObject Parent { get { return this.parent; } }
        private GameObject parent;

        private BoundingBox boundingBox;
        public BoundingBox BoundingBox { get { return boundingBox; } set { boundingBox = value; } }

        public PhysicsObject(GameObject parent, Vector2 lowerLeft, Vector2 size)
        {
            this.parent = parent;
            boundingBox = new BoundingBox(new Vector3(lowerLeft, 0.0f), new Vector3(lowerLeft + size, 0.0f));
        }

        public void AddCollider(PhysicsObject collider)
        {
            parent.AddCollider(collider.parent);
        }
    }
}
