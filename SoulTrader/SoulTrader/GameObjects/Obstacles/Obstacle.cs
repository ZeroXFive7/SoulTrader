using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SoulTrader
{
    public class Obstacle : GameObject
    {

        public Obstacle(string spriteName, Vector2 initialPosition, Vector2 size)
            : base(spriteName, initialPosition, size)
        {
        }
    }
}
