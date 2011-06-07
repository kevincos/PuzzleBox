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

    public class JellyfishRenderer
    {

        public static Texture2D jellytexture;

        public static Texture2D orangeJelly;
        public static Texture2D baseballJelly;
        public static Texture2D agentJelly;
        public static Texture2D mustacheJelly;
        public static Texture2D mysteryJelly;
        public static Texture2D clownJelly;
        public static Texture2D profJelly;
        public static Texture2D libraryJelly;
        public static Texture2D firemanJelly;
        public static Texture2D officerJelly;
        public static Texture2D baseballJelly2;
        public static Texture2D mogulJelly;
        public static Texture2D fortuneJelly;
        public static Texture2D artistJelly;
        public static Texture2D bikerJelly;
        public static Texture2D birthdayJelly;
        public static Texture2D capnJelly;
        public static Texture2D chefJelly;
        public static Texture2D karateJelly;
        public static Texture2D explorerJelly;
        public static Texture2D kingJelly;
        public static Texture2D hookerJelly;
        public static Texture2D queenJelly;
        public static Texture2D newsieJelly;
        public static Texture2D ballerinaJelly;
        public static Texture2D cowboyJelly;
        public static Texture2D ninjaJelly;
        public static Texture2D greenJelly;
        public static Texture2D redJelly;
        public static Texture2D yellowJelly;

        public static Texture2D transparentBody;
        public static Texture2D transparentRing;

        public static Texture2D doctorJellyfish;
        public static Texture2D nurseJellyfish;
        public static Texture2D speechBubble;
        public static Texture2D speechBubble2;

        public static void DrawSpeechBubble(int x, int y, int opacity, SpriteEffects effects)
        {
            Color c = Color.White;
            c.R = (Byte)(c.R * opacity / 100);
            c.G = (Byte)(c.G * opacity / 100);
            c.B = (Byte)(c.B * opacity / 100);
            c.A = (Byte)(c.A * opacity / 100);
            Game.spriteBatch.Draw(speechBubble,
                new Rectangle(x - 300, y - 50, 600, 150), null,
                c, 0f, Vector2.Zero, effects, 0);
        }
        public static void DrawSpeechBubble2(int x, int y, int opacity, SpriteEffects effects)
        {
            Color c = Color.White;
            c.R = (Byte)(c.R * opacity / 100);
            c.G = (Byte)(c.G * opacity / 100);
            c.B = (Byte)(c.B * opacity / 100);
            c.A = (Byte)(c.A * opacity / 100);
            Game.spriteBatch.Draw(speechBubble2,
                new Rectangle(x - 300, y - 50, 600, 150), null,
                c, 0f, Vector2.Zero, effects, 0);
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