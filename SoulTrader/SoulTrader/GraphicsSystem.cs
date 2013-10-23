using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SoulTrader
{
    static public class GraphicsSystem
    {
        #region XNALibraries

        static private SpriteBatch spriteBatch;
        static private ContentManager contentManager;
        
        #endregion

        #region Transforms

        static private Matrix viewProjTransform = Matrix.Identity;

        #endregion

        #region Collections

        static private Dictionary<string, List<Matrix>> renderQueue = new Dictionary<string, List<Matrix>>();
        static private Dictionary<string, Texture2D> texLibrary = new Dictionary<string, Texture2D>();

        #endregion

        static private SpriteFont font;

        static public void Initialize(SpriteBatch sprBatch, ContentManager contentMgr)
        {
            spriteBatch = sprBatch;
            contentManager = contentMgr;

            font = contentManager.Load<SpriteFont>("Fonts/arial");
        }

        static public void RegisterGraphicsObject(GraphicsObject obj)
        {
            string spriteName = obj.TextureString;
            RegisterSpriteByName(spriteName);
        }

        static public void RegisterSpriteByName(string spriteName)
        {
            if (!texLibrary.ContainsKey(spriteName))
            {
                texLibrary.Add(spriteName, contentManager.Load<Texture2D>("Textures/" + spriteName));
            }
        }

        static public void Update(Matrix viewTransform)
        {
            viewProjTransform = viewTransform;
        }

        static public void EnqueueGraphicsObject(GraphicsObject sprite, Matrix worldTransform)
        {
            if (!renderQueue.ContainsKey(sprite.TextureString))
            {
                renderQueue.Add(sprite.TextureString, new List<Matrix>());
            }
            renderQueue[sprite.TextureString].Add(worldTransform);
        }

        static public void RenderGraphicsObjects()
        {
            spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            foreach (string spriteString in renderQueue.Keys)
            {
                List<Matrix> spriteTransforms = renderQueue[spriteString];
                Texture2D texture = texLibrary[spriteString];

                foreach (Matrix worldTransform in spriteTransforms)
                {
                    Matrix worldViewProjTransform = worldTransform * viewProjTransform;

                    Vector2 bottomLeft = Vector2.Transform(Vector2.Zero, worldViewProjTransform);
                    Vector2 topRight = Vector2.Transform(Vector2.One, worldViewProjTransform);

                    int height = (int)topRight.Y - (int)bottomLeft.Y;
                    int width = (int)topRight.X - (int)bottomLeft.X;

                    Rectangle boundingRec = new Rectangle((int)bottomLeft.X, (int)bottomLeft.Y - height, width, height);
                    spriteBatch.Draw(texture, boundingRec, Color.White);
                }
            }
            spriteBatch.End();

            renderQueue.Clear();
        }

        static public void RenderUI(string textureName, Rectangle screenPosition)
        {
            spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            spriteBatch.Draw(texLibrary[textureName], screenPosition, Color.White);
            spriteBatch.End();
        }

        static public void RenderString(string message, Vector2 screenPosition)
        {
            spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            spriteBatch.DrawString(font, message, screenPosition, Color.White);
            spriteBatch.End();
        }
    }
}
