using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBox
{
    public enum JellyFishId
    {
        ORANGE,
        MUSTACHE,
        BASEBALL,
        AGENT
    }
    
    class JellyfishRenderer
    {

        public static Texture2D jellytexture;

        public static Texture2D orangeJelly;
        public static Texture2D baseballJelly;
        public static Texture2D agentJelly;
        public static Texture2D mustacheJelly;
        public static Texture2D mysteryJelly;

        public static Texture2D transparentBody;
        public static Texture2D transparentRing;

        public static Texture2D doctorJellyfish;
        public static Texture2D nurseJellyfish;
        public static Texture2D speechBubble;

        public static void DrawSpeechBubble(int x, int y, int opacity, SpriteEffects effects)
        {
            Color c = Color.White;
            c.R = (Byte)(c.R * opacity / 100);
            c.G = (Byte)(c.G * opacity / 100);
            c.B = (Byte)(c.B * opacity / 100);
            c.A = (Byte)(c.A * opacity / 100);
            Game.spriteBatch.Draw(speechBubble,
                new Rectangle(x -300, y-50, 600, 100),null,
                c,0f,Vector2.Zero,effects,0);
        }

        public static void DrawTransparentRing(int x, int y, int opacity, float scale)
        {
            Color c = Color.White;
            c.R = (Byte)(c.R * opacity / 100);
            c.G = (Byte)(c.G * opacity / 100);
            c.B = (Byte)(c.B * opacity / 100);
            c.A = (Byte)(c.A * opacity / 100);
            
            Game.spriteBatch.Draw(transparentRing,
                new Rectangle(x - (int)(32 * scale), y - (int)(32 * scale), (int)(64 * scale), (int)(64 * scale)),
                c);
        }

        public static void DrawTransparentJellyfish(int x, int y, int opacity, float scale)
        {
            Color c = Color.White;
            c.R = (Byte)(c.R * opacity / 100);
            c.G = (Byte)(c.G * opacity / 100);
            c.B = (Byte)(c.B * opacity / 100);
            c.A = (Byte)(c.A * opacity / 100);
            Game.spriteBatch.Draw(transparentBody,
                new Rectangle(x - (int)(256 * scale), y - (int)(256 * scale), (int)(512 * scale), (int)(512 * scale)),
                c);
        }

        public static void DrawJellyfish(int x, int y, int opacity, Texture2D texture, float scale)
        {
            DrawJellyfish(x, y, opacity, texture, scale, SpriteEffects.None);
        }

        public static void DrawJellyfish(int x, int y, int opacity, Texture2D texture, float scale, SpriteEffects effects)
        {
            Color c = Color.White;
            c.R = (Byte)(c.R * opacity / 100);
            c.G = (Byte)(c.G * opacity / 100);
            c.B = (Byte)(c.B * opacity / 100);
            c.A = (Byte)(c.A * opacity / 100);
            Game.spriteBatch.Draw(texture,
                new Rectangle(x - (int)(256*scale), y - (int)(256*scale), (int)(512*scale), (int)(512*scale)), null,
                c,0f,Vector2.Zero,effects,0);   
        }
    }
}