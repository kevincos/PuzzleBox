using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBox
{
    class OrbRenderer
    {
        static SpriteBatch spriteBatch;
        static Texture2D orbTexture;
        static Texture2D doubleTexture;

        public static void Init(SpriteBatch spriteBatch, Texture2D orbTexture, Texture2D doubleTexture)
        {
            OrbRenderer.spriteBatch = spriteBatch;
            OrbRenderer.orbTexture = orbTexture;
            OrbRenderer.doubleTexture = doubleTexture;
        }

        public static void DrawOrb(PuzzleNode node, State gameState, float animationPercentage)
        {
            if (node.hightlight)
            {
                if (gameState == State.DESTROY)
                {
                    float scale = node.scale * (1f - animationPercentage);
                    spriteBatch.Draw(orbTexture,
                        new Rectangle(node.screenX - (int)(16 * scale), node.screenY - (int)(16 * scale), (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                        node.color);
                }
                else if (gameState == State.REGENERATE)
                {
                    float scale = node.scale * animationPercentage;
                    spriteBatch.Draw(orbTexture,
                        new Rectangle(node.screenX - (int)(16 * scale), node.screenY - (int)(16 * scale), (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                        node.color);
                }
            }
            else
            {
                spriteBatch.Draw(orbTexture,
                    new Rectangle(node.screenX - (int)(16 * node.scale), node.screenY - (int)(16 * node.scale), (int)(32 * node.scale), (int)(32 * node.scale)), new Rectangle(0, 0, 64, 64),
                    node.color);
                if (node.bonus == 2)
                    spriteBatch.Draw(doubleTexture,
                        new Rectangle(node.screenX - (int)(16 * node.scale), node.screenY - (int)(16 * node.scale), (int)(32 * node.scale), (int)(32 * node.scale)), new Rectangle(0, 0, 64, 64),
                        node.color);
            }
        }
    }
}
