using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoulTrader.SceneSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoulTrader.Data_Structures
{
    public class QuadTree
    {
        #region Private Members

        private QuadTreeNode root = null;

        #endregion

        #region Constants

        private const int MAX_DEPTH = 10;
        private const int MAX_NODE_COUNT = 2;

        #endregion

        #region Public Interface (QuadTreeNode Wrapper)

        public int Count { get { return root.Count; } }

        public void Initialize(Vector2 minCorner, Vector2 maxCorner)
        {
            root = null;
            root = new QuadTreeNode(minCorner, maxCorner, 0);
        }

        public void Insert(PhysicsObject element)
        {
            root.Insert(element);
        }

        public void Clear()
        {
            root.Clear();
        }

        public List<PhysicsObject> Query(Vector2 minCorner, Vector2 maxCorner)
        {
            List<PhysicsObject> results = new List<PhysicsObject>();
            return root.Query(ref results, minCorner, maxCorner);
        }

        public void Render(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            Texture2D lineTexture = new Texture2D(device, 1, 1);
            lineTexture.SetData(new Color[] { Color.White });

            spriteBatch.Begin();
            root.Render(spriteBatch, lineTexture);
            spriteBatch.End();
        }

        #endregion

        #region QuadTreeNode class

        private class QuadTreeNode
        {
            #region Private Members

            private Vector2 minCorner;
            private Vector2 maxCorner;
            private Vector2 center;
            private Vector2 dimensions;

            private List<QuadTreeNode> children = null;
            private List<PhysicsObject> contents = new List<PhysicsObject>();

            private int count = 0;
            private int depth;

            #endregion

            #region Enum

            enum QuadrantRegion { NORTHEAST, NORTHWEST, SOUTHWEST, SOUTHEAST, NONE };

            #endregion

            #region Public Interface

            public int Count { get { return Count; } }

            public QuadTreeNode(Vector2 minCorner, Vector2 maxCorner, int depth)
            {
                this.minCorner  = minCorner;
                this.maxCorner  = maxCorner;
                this.dimensions = this.maxCorner - this.minCorner;
                this.center     = this.minCorner + 0.5f * this.dimensions;
                this.depth = depth;
            }

            public void Insert(PhysicsObject element)
            {
                if (children != null)
                {
                    QuadrantRegion region = GetQuadrantIndex(element.MinCorner, element.MaxCorner);
                    if (region != QuadrantRegion.NONE)
                    {
                        children[(int)region].Insert(element);
                        return;
                    }
                }

                contents.Add(element);

                if (contents.Count > MAX_NODE_COUNT && depth < MAX_DEPTH)
                {
                    if (children == null)
                    {
                        Split();
                    }

                    int i = 0;
                    while (i < contents.Count)
                    {
                        QuadrantRegion region = GetQuadrantIndex(contents[i].MinCorner, contents[i].MaxCorner);
                        if (region != QuadrantRegion.NONE)
                        {
                            children[(int)region].Insert(contents[i]);
                            contents.RemoveAt(i);
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
            }

            public void Clear()
            {
                contents.Clear();
                count = 0;

                if (children == null)
                {
                    return;
                }

                foreach (QuadTreeNode child in children)
                {
                    child.Clear();
                }

                children.Clear();
                children = null;
            }

            public List<PhysicsObject> Query(ref List<PhysicsObject> results, Vector2 regionMinCorner, Vector2 regionMaxCorner)
            {
                QuadrantRegion queryRegion = GetQuadrantIndex(regionMinCorner, regionMaxCorner);
                if (children != null)
                {
                    if (queryRegion != QuadrantRegion.NONE)
                    {
                        children[(int)queryRegion].Query(ref results, regionMinCorner, regionMaxCorner);
                    }
                    else
                    {
                        foreach (QuadTreeNode child in children)
                        {
                            child.Query(ref results, regionMinCorner, regionMaxCorner);
                        }
                    }
                }

                results.AddRange(contents);

                return results;
            }

            public void Render(SpriteBatch spriteBatch, Texture2D lineTexture)
            {
                int[] xOutline      = { (int)minCorner.X,  (int)maxCorner.X - 2, (int)minCorner.X,     (int)minCorner.X  };
                int[] yOutline      = { (int)maxCorner.Y,  (int)maxCorner.Y,     (int)minCorner.Y + 2, (int)maxCorner.Y  };
                int[] widthOutline  = { (int)dimensions.X, 2,                    (int)dimensions.X,    2                 };
                int[] heightOutline = { 2,                 (int)dimensions.Y,    2,                    (int)dimensions.Y };

                for (int i = 0; i < 4; ++i)
                {
                    Rectangle rect = new Rectangle(
                        xOutline[i] - (int)ActiveScene.Camera.BottomLeftPosition.X,
                        ActiveScene.Camera.Viewport.Height - yOutline[i] - (int)ActiveScene.Camera.BottomLeftPosition.Y, 
                        widthOutline[i], 
                        heightOutline[i]);

                    spriteBatch.Draw(lineTexture, rect, Color.White);
                }

                if (children == null)
                {
                    return;
                }

                foreach (QuadTreeNode child in children)
                {
                    child.Render(spriteBatch, lineTexture);
                }
            }

            #endregion

            #region Helper Methods

            private QuadrantRegion GetQuadrantIndex(Vector2 eleMinCorner, Vector2 eleMaxCorner)
            {
                bool northContains = eleMinCorner.Y > this.center.Y && eleMaxCorner.Y < this.maxCorner.Y;
                bool southContains = eleMinCorner.Y > this.minCorner.Y && eleMaxCorner.Y < this.center.Y;
                bool westContains  = eleMinCorner.X > this.minCorner.X && eleMaxCorner.X < this.center.X;
                bool eastContains  = eleMinCorner.X > this.center.X && eleMaxCorner.X < this.maxCorner.X;

                if (northContains)
                {
                    if (eastContains)
                    {
                        return QuadrantRegion.NORTHEAST;
                    }
                    else if (westContains)
                    {
                        return QuadrantRegion.NORTHWEST;
                    }
                }
                else if (southContains)
                {
                    if (eastContains)
                    {
                        return QuadrantRegion.SOUTHEAST;
                    }
                    else if (westContains)
                    {
                        return QuadrantRegion.SOUTHWEST;
                    }
                }

                return QuadrantRegion.NONE;
            }

            private void Split()
            {
                children = new List<QuadTreeNode>(4);

                children.Add(new QuadTreeNode(this.center, this.maxCorner, depth + 1));
                children.Add(new QuadTreeNode(new Vector2(this.minCorner.X, this.center.Y), new Vector2(this.center.X, this.maxCorner.Y), depth + 1));
                children.Add(new QuadTreeNode(this.minCorner, this.center, depth + 1));
                children.Add(new QuadTreeNode(new Vector2(this.center.X, this.minCorner.Y), new Vector2(this.maxCorner.X, this.center.Y), depth + 1));
            }

            #endregion
        }

        #endregion
    }
}
