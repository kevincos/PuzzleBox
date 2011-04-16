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
    class LifeBar
    {
        public static Texture2D bar;
        public static Texture2D pointer;

        public int lifePoints = 30000;

        public bool enabled;

        public bool Update(GameTime gameTime)
        {
            if (enabled)
            {
                lifePoints -= gameTime.ElapsedGameTime.Milliseconds;


                if (lifePoints < 0)
                    return false;             
            }
            return true;
        }

        public void Draw()
        {
            if (enabled)
            {
                Game.spriteBatch.Draw(bar, new Rectangle(680, 25, 100, 400), Color.White);
                Game.spriteBatch.Draw(pointer, new Rectangle(680, (int)(375 - 315 * (1f * lifePoints / 60000)), 40, 20), Color.White);
            }
        }


    }
}
