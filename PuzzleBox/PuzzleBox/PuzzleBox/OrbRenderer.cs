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
                        /*float scale = node.scale * (1f-animationPercentage);                        
                        spriteBatch.Draw(orbTexture,
                            new Rectangle(node.screenX - (int)(16 * scale), node.screenY - (int)(16 * scale), (int)(32 * scale), (int)(32 * scale)), new Rectangle(0, 0, 64, 64),
                            node.color);*/
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
                        int currentX = node.screenX;
                        int currentY = node.screenY;
                        if (node.replace_top)
                        {                            
                            currentY -= node.replace_distance * Game1.spacing;
                            currentY += (int)(animationPercentage * (float)Game1.spacing);                        
                        }
                        if (node.replace_right)
                        {
                            currentX += node.replace_distance * Game1.spacing;
                            currentX -= (int)(animationPercentage * (float)Game1.spacing);                        
                        }
                        if (node.replace_bottom)
                        {
                            currentY += node.replace_distance * Game1.spacing;
                            currentY -= (int)(animationPercentage * (float)Game1.spacing);                        
                        }
                        if (node.replace_left)
                        {
                            currentX -= node.replace_distance * Game1.spacing;
                            currentX += (int)(animationPercentage * (float)Game1.spacing);                        
                        }
                        if(!(currentX > Game1.screenCenterX + Game1.spacing*3 ||
                            currentX < Game1.screenCenterX - Game1.spacing*3 ||
                            currentY > Game1.screenCenterY + Game1.spacing*3 ||
                            currentY < Game1.screenCenterY - Game1.spacing*3))
                            spriteBatch.Draw(orbTexture,
                                new Rectangle(currentX - (int)(16 * node.scale), currentY - (int)(16 * node.scale), (int)(32 * node.scale), (int)(32 * node.scale)), new Rectangle(0, 0, 64, 64),
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
