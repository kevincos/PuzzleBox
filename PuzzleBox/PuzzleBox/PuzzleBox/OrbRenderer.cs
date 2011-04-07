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

        public static void Init(SpriteBatch spriteBatch, Texture2D orbTexture)
        {
            OrbRenderer.spriteBatch = spriteBatch;
            OrbRenderer.orbTexture = orbTexture;
        }

        public static void DrawOrb(int x, int y, float scale, Color color)
        {
            spriteBatch.Draw(orbTexture,
                new Rectangle(x - (int)(16 * scale), y - (int)(16 * scale), (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                color);
        }

        public static void DrawOrb(int x, int y, float scale, Color color, AnimationState state, float percentage)
        {
            if (state == AnimationState.NORMAL)
                DrawOrb(x, y, scale, color);
            if (state == AnimationState.EXPLODING)
            {
                scale = scale * (1f-percentage);
                spriteBatch.Draw(orbTexture,
                    new Rectangle(x - (int)(16 * scale), y - (int)(16 * scale), (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                    color);
            }
        }

        public static void DrawHighlight(int x, int y, float scale)
        {
            spriteBatch.Draw(orbTexture,
                new Rectangle(x - (int)(16 * scale*1.2f), y - (int)(16 * scale*1.2f), (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                Color.White);
        }

    }
}
