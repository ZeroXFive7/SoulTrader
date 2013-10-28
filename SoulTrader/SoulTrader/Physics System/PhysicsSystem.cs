using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SoulTrader
{
    static public class PhysicsSystem
    {
        static private List<PhysicsObject> physicsScene = new List<PhysicsObject>();
        static private List<GameObject> colliders = new List<GameObject>();

        static public void Initialize()
        {

        }

        static public void RegisterPhysicsObject(PhysicsObject physicsObject)
        {
            if (!physicsScene.Contains(physicsObject))
            {
                physicsScene.Add(physicsObject);
            }
        }

        static public void RemovePhyicsObject(PhysicsObject physicsObject)
        {
            if (physicsScene.Contains(physicsObject))
            {
                physicsScene.Remove(physicsObject);
            }
        }

        static public void Update()
        {
            for (int i = 0; i < physicsScene.Count; ++i)
            {
                for (int j = 0; j < physicsScene.Count; ++j)
                {
                    PhysicsObject collider = physicsScene[i];
                    PhysicsObject collidee = physicsScene[j];

                    if (collider != collidee && collider.BoundingBox.Intersects(collidee.BoundingBox))
                    {
                        collider.AddCollider(collidee);

                        colliders.Add(collidee.Parent);
                    }
                }
            }

            foreach (GameObject obj in colliders)
            {
                obj.HandleCollisions();
            }

            colliders.Clear();
        }
    }
}
