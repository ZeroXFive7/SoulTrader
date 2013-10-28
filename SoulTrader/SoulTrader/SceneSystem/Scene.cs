using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SoulTrader
{
    [Serializable]
    public struct GameObjectFileData
    {
        public string ObjectType;
        public string GraphicName;
        public Vector2 Position;
        public Vector2 Scale;
    }

    public class Scene
    {
        public Camera Camera { get { return camera; } }
        private Camera camera = null;

        public Player Player { get { return player; } }
        private Player player = null;

        protected List<GameObject> scene = new List<GameObject>();

        private List<GameObject> insertionQueue = new List<GameObject>();
        private List<GameObject> removalQueue = new List<GameObject>();

        public Scene(Camera camera)
        {
            this.camera = camera;
        }

        public void AddAndInitialize(GameObject gameObject)
        {
            Add(gameObject);
            GraphicsSystem.RegisterGraphicsObject(gameObject.GraphicsObject);
            PhysicsSystem.RegisterPhysicsObject(gameObject.PhysicsObject);
        }

        public void Add(GameObject gameObject)
        {
            insertionQueue.Add(gameObject);
        }

        public void RemoveAndUninitialize(GameObject gameObject)
        {
            Remove(gameObject);
            PhysicsSystem.RemovePhyicsObject(gameObject.PhysicsObject);
        }

        public void Remove(GameObject gameObject)
        {
            removalQueue.Add(gameObject);
        }

        public void Initialize()
        {
            foreach (GameObject obj in scene)
            {
                GraphicsSystem.RegisterGraphicsObject(obj.GraphicsObject);
                PhysicsSystem.RegisterPhysicsObject(obj.PhysicsObject);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (GameObject obj in scene)
            {
                obj.Update(gameTime.ElapsedGameTime);
            }

            if (insertionQueue.Count > 0)
            {
                scene.AddRange(insertionQueue);
                insertionQueue.Clear();
            }

            if (removalQueue.Count > 0)
            {
                foreach (GameObject obj in removalQueue)
                {
                    scene.Remove(obj);
                }
                removalQueue.Clear();
            }
        }

        public void Render()
        {
            foreach (GameObject obj in scene)
            {
                obj.Render();
            }
        }

        public void Save(string destinationFilePath)
        {
            List<GameObjectFileData> objects = new List<GameObjectFileData>();
            foreach (GameObject obj in scene)
            {
                GameObjectFileData objectData = new GameObjectFileData();
                objectData.ObjectType = obj.GetType().ToString();
                objectData.GraphicName = obj.GraphicsObject.TextureString;
                objectData.Position = obj.BottomLeftPosition;
                objectData.Scale = obj.Scale;

                objects.Add(objectData);
            }

            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "object";
            root.IsNullable = true;

            FileStream stream = File.Open(destinationFilePath, FileMode.Create);

            XmlSerializer serializer = new XmlSerializer(typeof(List<GameObjectFileData>), root);
            serializer.Serialize(stream, objects);

            stream.Close();
        }

        public void Load(string filePath)
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "object";
            root.IsNullable = true;

            FileStream stream = File.Open(filePath, FileMode.Open);

            XmlSerializer deserializer = new XmlSerializer(typeof(List<GameObjectFileData>), root);
            List<GameObjectFileData> objects = (List<GameObjectFileData>)deserializer.Deserialize(stream);

            stream.Close();

            foreach (GameObjectFileData obj in objects)
            {
                scene.Add(Instantiate(obj));
            }
        }

        private GameObject Instantiate(GameObjectFileData objData)
        {
            switch (objData.ObjectType)
            {
                case "SoulTrader.Player":
                    player = new Player(camera, objData.GraphicName, objData.Position, objData.Scale);
                    return player;
                case "SoulTrader.Obstacle":
                    return new Obstacle(objData.GraphicName, objData.Position, objData.Scale);
                case "SoulTrader.KillZone":
                    return new KillZone(objData.GraphicName, objData.Position, objData.Scale);
            }

            return new GameObject(objData.GraphicName, objData.Position, objData.Scale);
        }
    }
}
