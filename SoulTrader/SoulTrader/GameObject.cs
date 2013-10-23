using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SoulTrader
{
    public class GameObject
    {
        #region System Children

        protected PhysicsObject physicsChild;
        protected GraphicsObject graphicsChild;

        public GraphicsObject GraphicsObject { get { return graphicsChild; } }
        public PhysicsObject PhysicsObject { get { return physicsChild; } }

        #endregion

        #region World Properties

        public Vector2 BottomLeftPosition { get { return worldPosition; } }
        public Vector2 CenterPosition { get { return worldPosition + 0.5f * worldScale; } }
        public Vector2 TopRightPosition { get { return worldPosition + worldScale; } }
        public Vector2 Scale { get { return worldScale; } }

        protected Vector2 worldPosition = Vector2.Zero;
        protected Vector2 worldScale = new Vector2(50, 100);

        #endregion

        #region Collections

        protected List<GameObject> Colliders = new List<GameObject>();

        #endregion

        public GameObject(string spriteName, Vector2 initialPosition, Vector2 size)
        {
            graphicsChild = new GraphicsObject(this, spriteName);
            physicsChild = new PhysicsObject(this, initialPosition, size);

            worldPosition = initialPosition;
            worldScale = size;
        }

        public void AddCollider(GameObject collider)
        {
            Colliders.Add(collider);
        }

        public virtual void Update(TimeSpan timeSinceLastFrame)
        {
        }

        public virtual void HandleCollisions()
        {
            Colliders.Clear();
        }

        public virtual void Render()
        {
            Matrix worldTransform = Matrix.CreateScale(worldScale.X, worldScale.Y, 1.0f) * Matrix.CreateTranslation(worldPosition.X, -worldPosition.Y, 0.0f);
            GraphicsSystem.EnqueueGraphicsObject(graphicsChild, worldTransform);
        }
    }
}
