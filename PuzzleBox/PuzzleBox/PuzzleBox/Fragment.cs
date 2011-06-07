using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBox
{
    public class Fragment
    {
        public Color color;
        public int posX;
        public int posY;
        public int goalX;
        public int goalY;
        public float scale;
        private int velX = 0;
        private int velY = 0;

        public Fragment(int posX, int posY, int goalX, int goalY, float scale, Color color)
        {
            this.color = color;
            this.scale = scale;
            this.posX = posX;
            this.posY = posY;
            this.goalX = goalX;
            this.goalY = goalY;
        }

        public bool Update(GameTime elapsedTime)
        {
            posX += velX;
            posY += velY;
            if (posX > goalX) posX = goalX;
            if (posY > goalY) posY = goalY;
            if (posX == goalX || posY == goalY) return false;
            velX += (goalX - posX) / 100;
            velY += (goalY - posY) / 100;
            return true;
        }
    }
}

