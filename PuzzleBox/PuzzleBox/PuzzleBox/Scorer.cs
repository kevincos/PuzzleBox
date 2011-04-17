using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBox
{
    class ScoringSet
    {
        public int start = -1;
        public int end = -1;
        public int x = -1;
        public int y = -1;
        public int drawX=100;
        public int drawY=100;
        public int animationTime = 1000;
        public Color color;
        public int score = 10;
        public int multiplier = 1;

        public ScoringSet()
        {            
        }

        public void LogScore()
        {
            int matchLength = this.end - this.start;
            int newScore = 10 * matchLength;
            if (matchLength >= 3)
                newScore = newScore * 2;
            
            if (matchLength == 2)
            {
                Logger.numDoubles++;
                Logger.pointsFromDoubles += newScore;
            }
            if (matchLength == 3)
            {
                Logger.numTriples++;
                Logger.pointsFromTriples += newScore;
            }
            if (matchLength == 4)
            {
                Logger.numQuads++;
                Logger.pointsFromQuads += newScore;
            }
            if (matchLength == 5)
            {
                Logger.numFullLines++;
                Logger.pointsFromFullLines += newScore;
            }
        }

        public void CalculateScore()
        {
            int matchLength = this.end-this.start;
            int newScore = 10 * matchLength;            
            if(matchLength>=3)
                newScore = newScore*2;
            this.score = newScore * this.multiplier;
        }

        public bool IsVertical()
        {
            return (this.y == -1);
        }

        public bool ContainsPoint(int x, int y)
        {
            if (x == this.x)
                return (y >= start && y <= end);
            if (y==this.y)
                return (x>=start && x <= end);
            return false;
        }

        public bool Update(GameTime gameTime)
        {
            animationTime -= gameTime.ElapsedGameTime.Milliseconds;
            drawY -= (int)(.1f * gameTime.ElapsedGameTime.Milliseconds);
            if (animationTime < 0)
                return false;
            return true;
        }
    }

}
