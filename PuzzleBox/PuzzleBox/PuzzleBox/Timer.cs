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
    class Countdown
    {
        int posX;
        int posY;

        public int timeRemaining;

        public bool enabled;

        public Countdown(int initialTime, int posX, int posY)
        {
            timeRemaining = initialTime;
            this.posX = posX;
            this.posY = posY;
        }

        public bool Update(GameTime gameTime)
        {
            if (enabled)
            {
                timeRemaining -= gameTime.ElapsedGameTime.Milliseconds;
                if (timeRemaining < 0)
                    return false;
            }
            return true;
        }

        public void Draw()
        {
            if (enabled)
            {
                TimeSpan t = new TimeSpan(0, 0, 0, 0, timeRemaining);
                Game.spriteBatch.DrawString(Game.spriteFont, string.Format("TIME - {0}:{1:D2}", t.Minutes, t.Seconds), new Vector2(posX, posY), Color.White);
            }
        }
    }
    class Countup
    {
        int posX;
        int posY;

        public int timeElapsed;

        public bool enabled;

        public Countup(int initialTime, int posX, int posY)
        {
            timeElapsed = initialTime;
            this.posX = posX;
            this.posY = posY;
        }

        public bool Update(GameTime gameTime)
        {
            if (enabled)
            {
                timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            }
            return true;
        }

        public void Draw()
        {
            if (enabled)
            {
                TimeSpan t = new TimeSpan(0, 0, 0, 0, timeElapsed);
                Game.spriteBatch.DrawString(Game.spriteFont, string.Format("TIME - {0}:{1:D2}", t.Minutes, t.Seconds), new Vector2(posX, posY), Color.White);
            }
        }
    }
}
