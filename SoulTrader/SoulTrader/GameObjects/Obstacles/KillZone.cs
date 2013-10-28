using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SoulTrader
{
    public class KillZone : Obstacle
    {
        public KillZone(string spriteName, Vector2 initialPosition, Vector2 scale)
            : base(spriteName, initialPosition, scale)
        {
        }

        public override void Render()
        {
            return;
        }
    }
}
