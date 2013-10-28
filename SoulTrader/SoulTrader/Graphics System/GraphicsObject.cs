using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SoulTrader
{
    public class GraphicsObject
    {
        private GameObject parent;

        public string TextureString { get { return this.texString; } }
        private string texString = "";

        public Vector2 WorldPosition { get { return this.worldPosition; } set { this.worldPosition = value; } }
        private Vector2 worldPosition = Vector2.Zero;

        public GraphicsObject(GameObject parent, string texString)
        {
            this.parent = parent;
            this.texString = texString;
        }

    }
}
