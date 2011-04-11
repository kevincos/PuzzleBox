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
        public static SpriteBatch spriteBatch;
        public static Texture2D orbTexture;
        public static Texture2D orbCrackedLeftTexture;
        public static Texture2D orbCrackedRightTexture;
        public static Texture2D orbCrackedTopTexture;
        public static Texture2D orbFragmentLeftTexture;
        public static Texture2D orbFragmentRightTexture;
        public static Texture2D orbFragmentTopTexture;
        public static Texture2D doubleTexture;

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
            spriteBatch.Draw(orbFragmentLeftTexture,
                new Rectangle(leftX, leftY, (int)(32 * scale), (int)(32 * scale)),
                new Rectangle(0, 0, 64, 64),
                alphaColor);
            spriteBatch.Draw(orbFragmentRightTexture,
                new Rectangle(rightX, rightY, (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                alphaColor);
            spriteBatch.Draw(orbFragmentTopTexture,
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

        public static void DrawOrb(PuzzleNode node, State gameState, float animationPercentage)
        {
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
                            spriteBatch.Draw(orbFragmentLeftTexture,
                                new Rectangle(leftX, leftY, (int)(32 * scale), (int)(32 * scale)),
                                new Rectangle(0, 0, 64, 64),
                                alphaColor);
                            spriteBatch.Draw(orbFragmentRightTexture,
                                new Rectangle(rightX, rightY, (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                                alphaColor);
                            spriteBatch.Draw(orbFragmentTopTexture,
                                new Rectangle(topX, topY, (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                                alphaColor);
                        }
                        else
                        {
                            spriteBatch.Draw(orbCrackedLeftTexture,
                                new Rectangle(leftX, leftY, (int)(32 * scale), (int)(32 * scale)),
                                new Rectangle(0, 0, 64, 64),
                                alphaColor);
                            spriteBatch.Draw(orbCrackedRightTexture,
                                new Rectangle(rightX, rightY, (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                                alphaColor);
                            spriteBatch.Draw(orbCrackedTopTexture,
                                new Rectangle(topX, topY, (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                                alphaColor);
                        }
                    }
                    else
                    {
                        float scale = node.scale;          
                        spriteBatch.Draw(orbTexture,
                            new Rectangle(node.screenX - (int)(16 * scale), node.screenY - (int)(16 * scale), (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                            node.color);
                    }
                }
                else if (gameState == State.REGENERATE)
                {
                    float scale = node.scale * animationPercentage;
                    if (scale > node.scale) scale = node.scale;
                    if (Matcher.IsWildCard(node))
                        spriteBatch.Draw(orbTexture,
                            new Rectangle(node.screenX - (int)(16 * scale), node.screenY - (int)(16 * scale), (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                            node.color);
                    else
                    {                        
                        spriteBatch.Draw(orbTexture,
                            new Rectangle(node.screenX - (int)(16 * node.scale), node.screenY - (int)(16 * node.scale), (int)(32 * node.scale), (int)(32 * node.scale)), new Rectangle(0, 0, 64, 64),
                            node.color);
                    }
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
