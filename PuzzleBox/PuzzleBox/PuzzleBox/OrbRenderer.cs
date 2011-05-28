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
        public static Texture2D bubbleTexture;
        public static Texture2D orbTexture;
        public static Texture2D orbCrackedLeftTexture;
        public static Texture2D orbCrackedRightTexture;
        public static Texture2D orbCrackedTopTexture;
        public static Texture2D orbFragmentLeftTexture;
        public static Texture2D orbFragmentRightTexture;
        public static Texture2D orbFragmentTopTexture;
        //public static Texture2D numbersTexture;
        //public static Texture2D clocknumbersTexture;
        public static Texture2D doubleTexture;
        public static Texture2D highlightTexture;
        public static Texture2D haloTexture;
        public static Texture2D backgroundTexture;
        public static Texture2D vortexTexture;
        public static Texture2D toggleInnerTexture;
        public static Texture2D toggleOuterTexture;
        
        public static void DrawScoreBonus(ScoringSet set)
        {
            set.CalculateScore();
            Game.spriteBatch.DrawString(Game.menuFont, "+" + set.score, new Vector2(set.drawX, set.drawY), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            Game.spriteBatch.DrawString(Game.menuFont, "+" + set.score, new Vector2(set.drawX, set.drawY), set.color, 0, Vector2.Zero, .96f, SpriteEffects.None, 0);
        }

        public static void DrawFragments(Fragment frag)
        {
            float scale = frag.scale * 2f;
            Color alphaColor = frag.color;
            alphaColor.A = 0;
            int topX = frag.posX - (int)(16 * scale);
            int topY = frag.posY - (int)(16 * scale);
            int leftX = frag.posX - (int)(16* scale);
            int leftY = frag.posY - (int)(16* scale);
            int rightX = frag.posX - (int)(16* scale);
            int rightY = frag.posY - (int)(16* scale);
            Game.spriteBatch.Draw(orbFragmentLeftTexture,
                new Rectangle(leftX, leftY, (int)(32 * scale), (int)(32 * scale)),
                new Rectangle(0, 0, 64, 64),
                alphaColor);
            Game.spriteBatch.Draw(orbFragmentRightTexture,
                new Rectangle(rightX, rightY, (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                alphaColor);
            Game.spriteBatch.Draw(orbFragmentTopTexture,
                new Rectangle(topX, topY, (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                alphaColor);
        }

        public static float GetSlideX(int x, PuzzleNode p, float animationPercentage)
        {
            float effectivePercentage = Math.Min(animationPercentage, (float)p.replace_distance);            
            float slideX = (float)x;
            if (p.replace_left)
            {
                slideX -= p.replace_distance;
                slideX += effectivePercentage;
            }
            if (p.replace_right)
            {
                slideX += p.replace_distance;
                slideX -= effectivePercentage;
            }
            return slideX;
        }

        public static float GetSlideY(int y, PuzzleNode p, float animationPercentage)
        {
            float effectivePercentage = Math.Min(animationPercentage, (float)p.replace_distance);
            float slideY = (float)y;
            if (p.replace_top)
            {
                slideY -= p.replace_distance;
                slideY += effectivePercentage;
            }
            if (p.replace_bottom)
            {
                slideY += p.replace_distance;
                slideY -= effectivePercentage;
            }
            return slideY;
        }

        public static void DrawBackground()
        {
            Game.spriteBatch.Draw(backgroundTexture,
                new Rectangle(0, 0, Game.screenSizeX, Game.screenSizeY), null,
                Color.White);
        }

        public static void DrawVortex(int x, int y, int opacity)
        {
            Color c = Color.White;            
            c.R = (Byte)(c.R * opacity / 100);
            c.G = (Byte)(c.G * opacity / 100);
            c.B = (Byte)(c.B * opacity / 100);
            c.A = (Byte)(c.A * opacity / 100);
            Game.spriteBatch.Draw(vortexTexture,
                new Rectangle(x, y, 128, 128), null,
                c);
        }



        public static void DrawOrb(PuzzleNode node, int x, int y, int size)
        {
            if (node.color != Color.Gray)
            {
                if (node.toggleOrb)
                {
                    Game.spriteBatch.Draw(toggleInnerTexture,
                        new Rectangle(x, y, size, size), new Rectangle(0, 0, 64, 64),
                        node.color);
                }
                else
                {
                    Game.spriteBatch.Draw(orbTexture,
                        new Rectangle(x, y, size, size), new Rectangle(0, 0, 64, 64),
                        node.color);
                }
            }
            else
            {
                Game.spriteBatch.Draw(bubbleTexture,
                    new Rectangle(x, y, size, size), new Rectangle(0, 0, 64, 64),
                    Color.White);
            }
            if (node.bonus == 2)
            {
                Game.spriteBatch.Draw(doubleTexture,
                    new Rectangle(x, y, size, size), new Rectangle(0, 0, 64, 64),
                    node.color);            
            }
            /*if (node.moveCountdownOrb == true)
            {
                int texX = 64*(node.countdown % 4);
                int texY = 64*(node.countdown / 4);
                Game.spriteBatch.Draw(numbersTexture,
                    new Rectangle(x, y, size, size), new Rectangle(texX, texY, 64, 64),
                    node.color);
            }
            if (node.timeCountdownOrb == true)
            {
                int texX = 64 * ((node.countdown/1000) % 4);
                int texY = 64 * ((node.countdown/1000) / 4);
                Game.spriteBatch.Draw(clocknumbersTexture,
                    new Rectangle(x, y, size, size), new Rectangle(texX, texY, 64, 64),
                    node.color);
            }*/
            if (node.toggleOrb == true)
            {
                if (node.toggleColor != Color.Gray)
                {
                    Game.spriteBatch.Draw(toggleOuterTexture,
                        new Rectangle(x, y, size, size), new Rectangle(0, 0, 64, 64),
                        node.toggleColor);
                }
                else
                {
                    Game.spriteBatch.Draw(bubbleTexture,
                    new Rectangle(x, y, size, size), new Rectangle(0, 0, 64, 64),
                        node.toggleColor);
                }

            }
        }

        public static void DrawOrb(PuzzleNode node, State gameState, float animationPercentage)
        {
            float delta = (int)Math.Abs(node.distance - Engine.baseDistance);
            Color highlightColor = node.color;
            highlightColor.A = 0;
            highlightColor.R = (Byte)(Math.Max(0, (1 - .03 * delta)) * highlightColor.R);
            highlightColor.G = (Byte)(Math.Max(0, (1 - .03 * delta)) * highlightColor.G);
            highlightColor.B = (Byte)(Math.Max(0, (1 - .03 * delta)) * highlightColor.B);
            if (node.color == Color.Gray)
            {
                highlightColor.A = 0;
                highlightColor.R = (Byte)(Math.Max(0, (1 - .03 * delta)) * 75);
                highlightColor.G = (Byte)(Math.Max(0, (1 - .03 * delta)) * 75);
                highlightColor.B = (Byte)(Math.Max(0, (1 - .03 * delta)) * 75);
            }
            if (node.color == Color.Gray && node.toggleColor == Color.Gray && gameState!=State.VANISH && gameState!=State.NEWSET)
            {
                Game.spriteBatch.Draw(highlightTexture,
                    new Rectangle(node.screenX - (int)(21 * node.scale), node.screenY - (int)(21 * node.scale), (int)(42 * node.scale), (int)(42 * node.scale)), new Rectangle(0, 0, 64, 64),
                    highlightColor);
            }
            else
            {
                if (node.selected)
                {
                    Game.spriteBatch.Draw(haloTexture,
                        new Rectangle(node.screenX - (int)(21 * node.scale), node.screenY - (int)(21 * node.scale), (int)(42 * node.scale), (int)(42 * node.scale)), new Rectangle(0, 0, 64, 64),
                        Color.White);
                }
                if (node.selected == false && !(node.marked == true && (gameState == State.VANISH || gameState == State.NEWSET)))
                {
                    if (gameState == State.DESTROY || gameState == State.VANISH || gameState == State.REGENERATE)
                    {
                        Game.spriteBatch.Draw(highlightTexture,
                            new Rectangle(node.screenX - (int)(21 * node.scale), node.screenY - (int)(21 * node.scale), (int)(42 * node.scale), (int)(42 * node.scale)), new Rectangle(0, 0, 64, 64),
                            highlightColor);
                    }
                    else
                    {
                        Game.spriteBatch.Draw(haloTexture,
                            new Rectangle(node.screenX - (int)(21 * node.scale), node.screenY - (int)(21 * node.scale), (int)(42 * node.scale), (int)(42 * node.scale)), new Rectangle(0, 0, 64, 64),
                            highlightColor);
                    }
                }
            }
            if (node.marked)
            {
                //if (gameState == State.VANSIH)
                //float scale = node.scale * (1f - animationPercentage);
                //spriteBatch.Draw(orbCrackedLeftTexture,
                    //new Rectangle(node.screenX - (int)(16 * scale), node.screenY - (int)(16 * scale), (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                    //node.color);
                if (gameState == State.DESTROY)
                {
                    if (node.scoring)
                    {
                        float scale = node.scale * (1f + animationPercentage);
                        Color alphaColor = node.color;
                        alphaColor.A = (Byte)((1f - animationPercentage) * 255);
                        int topX = node.screenX - (int)(16 * scale);
                        int topY = node.screenY - (int)((16 + 4 * animationPercentage) * scale);
                        int leftX = node.screenX - (int)((16 + 3 * animationPercentage) * scale); ;
                        int leftY = node.screenY - (int)((16 - 3 * animationPercentage) * scale); ;
                        int rightX = node.screenX - (int)((16 - 3 * animationPercentage) * scale); ;
                        int rightY = node.screenY - (int)((16 - 3 * animationPercentage) * scale); ;

                        if (animationPercentage > .50)
                        {
                            Game.spriteBatch.Draw(orbFragmentLeftTexture,
                                new Rectangle(leftX, leftY, (int)(32 * scale), (int)(32 * scale)),
                                new Rectangle(0, 0, 64, 64),
                                alphaColor);
                            Game.spriteBatch.Draw(orbFragmentRightTexture,
                                new Rectangle(rightX, rightY, (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                                alphaColor);
                            Game.spriteBatch.Draw(orbFragmentTopTexture,
                                new Rectangle(topX, topY, (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                                alphaColor);
                        }
                        else
                        {
                            Game.spriteBatch.Draw(orbCrackedLeftTexture,
                                new Rectangle(leftX, leftY, (int)(32 * scale), (int)(32 * scale)),
                                new Rectangle(0, 0, 64, 64),
                                alphaColor);
                            Game.spriteBatch.Draw(orbCrackedRightTexture,
                                new Rectangle(rightX, rightY, (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                                alphaColor);
                            Game.spriteBatch.Draw(orbCrackedTopTexture,
                                new Rectangle(topX, topY, (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                                alphaColor);
                        }
                    }
                    else
                    {
                        float scale = node.scale;
                        DrawOrb(node, node.screenX - (int)(16 * scale), node.screenY - (int)(16 * scale), (int)(32 * scale));
                    }
                }
                else if (gameState == State.VANISH)
                {
                    float scale = node.scale * (1f-animationPercentage);
                    if (scale < 0) scale = 0;
                    DrawOrb(node, node.screenX - (int)(16 * scale), node.screenY - (int)(16 * scale), (int)(32 * scale));
                }
                else if (gameState == State.NEWSET)
                {
                    float scale = node.scale * animationPercentage;
                    if (scale < 0) scale = 0;
                    DrawOrb(node, node.screenX - (int)(16 * scale), node.screenY - (int)(16 * scale), (int)(32 * scale));
                }
                else if (gameState == State.REGENERATE)
                {
                    float scale = node.scale * animationPercentage;
                    if (scale > node.scale) scale = node.scale;
                    if (Matcher.IsWildCard(node))
                    {
                        DrawOrb(node, node.screenX - (int)(16 * scale), node.screenY - (int)(16 * scale), (int)(32 * scale));
                    }
                    else
                    {
                        DrawOrb(node, node.screenX - (int)(16 * node.scale), node.screenY - (int)(16 * node.scale), (int)(32 * node.scale));
                    }
                }
            }
            else
            {
                DrawOrb(node, node.screenX - (int)(16 * node.scale), node.screenY - (int)(16 * node.scale), (int)(32 * node.scale));
            }
        }
    }
}
