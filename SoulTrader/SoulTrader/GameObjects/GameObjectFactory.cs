using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoulTrader
{
    public static class GameObjectFactory
    {
        public static GameObject Instantiate(GameObjectFileData objData)
        {
            switch (objData.ObjectType)
            {
                case "SoulTrader.Player":
                    return new Player(null, objData.GraphicName, objData.Position, objData.Scale);
                case "SoulTrader.Obstacle":
                    return new Obstacle(objData.GraphicName, objData.Position, objData.Scale); 
                case "SoulTrader.KillZone":
                    return new KillZone(objData.GraphicName, objData.Position, objData.Scale);
            }

            return new GameObject(objData.GraphicName, objData.Position, objData.Scale);
        }
    }
}
