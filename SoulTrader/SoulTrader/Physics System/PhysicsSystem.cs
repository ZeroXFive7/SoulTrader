using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SoulTrader.Data_Structures;
using Microsoft.Xna.Framework.Graphics;
using SoulTrader.GameObjects.Actors.Projectiles;

namespace SoulTrader
{
    static public class PhysicsSystem
    {
        static public Vector2 SceneMinCorner = Vector2.Zero;
        static public Vector2 SceneMaxCorner = Vector2.Zero;

        static private List<PhysicsObject> sceneList = new List<PhysicsObject>();
        static private QuadTree sceneTree = new QuadTree();
        static private List<GameObject> colliders = new List<GameObject>();

        static public void Initialize()
        {
            sceneTree.Initialize(SceneMinCorner, SceneMaxCorner);//new Vector2(0, -100), new Vector2(1000, 1000));
        }

        static public void RegisterPhysicsObject(PhysicsObject physicsObject)
        {
            if (!sceneList.Contains(physicsObject))
            {
                sceneList.Add(physicsObject);
                SceneMinCorner = new Vector2(Math.Min(physicsObject.MinCorner.X, SceneMinCorner.X), Math.Min(physicsObject.MinCorner.Y, SceneMinCorner.Y));
                SceneMaxCorner = new Vector2(Math.Max(physicsObject.MaxCorner.X, SceneMaxCorner.X), Math.Max(physicsObject.MaxCorner.Y, SceneMaxCorner.Y));
            }
        }

        static public void RemovePhyicsObject(PhysicsObject physicsObject)
        {
            if (sceneList.Contains(physicsObject))
            {
                sceneList.Remove(physicsObject);
            }
        }

        static public void Update()
        {
            sceneTree.Clear();
            sceneTree.Initialize(SceneMinCorner, SceneMaxCorner);
            foreach (PhysicsObject element in sceneList)
            {
                sceneTree.Insert(element);
            }

            foreach (PhysicsObject element in sceneList)
            {
                List<PhysicsObject> potentialColliders = sceneTree.Query(element.MinCorner, element.MaxCorner);

                foreach (PhysicsObject collider in potentialColliders)
                {
                    if (element != collider && element.Intersects(collider))
                    {
                        element.AddCollider(collider);
                    }
                }

                if (element.NumColliders > 0)
                {
                    colliders.Add(element.Parent);
                }
            }

            foreach (GameObject obj in colliders)
            {
                obj.HandleCollisions();
            }

            colliders.Clear();
        }

        static public void Render(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            sceneTree.Render(device, spriteBatch);
        }
    }
}
