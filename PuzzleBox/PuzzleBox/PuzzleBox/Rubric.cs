using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PuzzleBox
{
    class Rubric
    {
        public static Texture2D plus;
        public static Texture2D minus;
        
        public bool enabled;

        public bool Update(GameTime gameTime)
        {
            return true;
        }

        public void Draw()
        {
            if (enabled)
            {
                Game.spriteBatch.Draw(plus, new Rectangle(635, 70, 32, 32), Color.White);
                Game.spriteBatch.Draw(plus, new Rectangle(675, 70, 32, 32), Color.White);                
                Game.spriteBatch.Draw(minus, new Rectangle(635, 105, 32, 32), Color.White);
                Game.spriteBatch.Draw(minus, new Rectangle(675, 105, 32, 32), Color.White);
                Game.spriteBatch.Draw(minus, new Rectangle(675, 140, 32, 32), Color.White);
                Game.spriteBatch.Draw(plus, new Rectangle(675, 175, 32, 32), Color.White);
                Game.spriteBatch.Draw(plus, new Rectangle(675, 210, 32, 32), Color.White);
                Game.spriteBatch.Draw(plus, new Rectangle(675, 245, 32, 32), Color.White);
                for (int j = 0; j < 6; j++)
                {
                    PuzzleNode p;
                    if (j == 0)
                        p = new PuzzleNode(Color.Blue);
                    else if (j == 1)
                        p = new PuzzleNode(Color.Yellow);
                    else if (j == 2)
                        p = new PuzzleNode(Color.Red);
                    else if (j == 3)
                        p = new PuzzleNode(Color.Green);
                    else if (j == 4)
                        p = new PuzzleNode(Color.Magenta);
                    else
                        p = new PuzzleNode(Color.Orange);

                    p.screenX = 740;
                    p.screenY = 85 + j * 35;

                    p.distance = 50;
                    p.scale = 1f;
                    OrbRenderer.DrawOrb(p, State.READY, 0f);
                }
            }
            
        }
    }
}
