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
        int mostRecentValue;

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
                if (t.Minutes == 0 && t.Seconds < 10)
                {
                    if (mostRecentValue != t.Seconds)
                    {
                        SoundEffects.PlayAlert();
                    }
                    mostRecentValue = t.Seconds;
                    Game.spriteBatch.DrawString(Game.spriteFont, string.Format("TIME  {0}:{1:D2}", t.Minutes, t.Seconds), new Vector2(posX , posY - 5), Color.Red, 0, Vector2.Zero, 1.3f, SpriteEffects.None, 0);
                }
                else
                    Game.spriteBatch.DrawString(Game.spriteFont, string.Format("TIME  {0}:{1:D2}", t.Minutes, t.Seconds), new Vector2(posX, posY), Color.White);
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
                Game.spriteBatch.DrawString(Game.spriteFont, string.Format("TIME-  {0}:{1:D2}", t.Minutes, t.Seconds), new Vector2(posX, posY), Color.White);
            }
        }
    }
}
