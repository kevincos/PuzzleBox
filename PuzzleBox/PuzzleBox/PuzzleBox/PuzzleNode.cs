using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBox
{
    class PuzzleNode : IComparable
    {
        private static Random r;

        static PuzzleNode()
        {
            PuzzleNode.r = new Random();
        }

        public static bool Match(PuzzleNode p1, PuzzleNode p2)
        {     
            if (p1.color == Color.Gray || p1.color == Color.Black) return false;
            return p1.color == p2.color;
        }

        public PuzzleNode()
        {            
            int v = r.Next(0, 6);
            switch (v)
            {
                case 0:
                    this.color = Color.Red;
                    break;
                case 1:
                    this.color = Color.Green;
                    break;
                case 2:
                    this.color = Color.Orange;
                    break;
                case 3:
                    this.color = Color.Magenta;
                    break;
                case 4:
                    this.color = Color.Blue;
                    break;
                case 5:
                    this.color = Color.Yellow;
                    break;
                default:
                    this.color = Color.Black;
                    break;

            }
            v = r.Next(0, 10);
            
            if (v == 5)
                this.bonus = 2;
            else if (v == 3)
            {
                this.moveCountdownOrb = true;
                this.countdown = 10;
            }
            else if (v == 4)
            {
                this.timeCountdownOrb = true;
                this.countdown = 10000;
            }
            else if (v == 2)
            {
                this.toggleOrb = true;
                this.toggleColor = Color.Gray;
            }            
        }

        public PuzzleNode(Color color)
        {
            this.color = color;
        }

        public PuzzleNode(Color color, int screenX, int screenY, float distance)
        {
            this.color = color;
            this.screenX = screenX;
            this.screenY = screenY;
            this.distance = distance;
        }

        public int CompareTo(object obj)
        {
            PuzzleNode p = obj as PuzzleNode;
            return distance.CompareTo(p.distance);
        }

        public void ClearMarking()
        {
            marked = false;
            replace_top = false;
            replace_left = false;
            replace_right = false;
            replace_bottom = false;
            replace_distance = 0;
            scoring = false;
        }

        public Color color;
        public Color toggleColor = Color.Gray;
        public bool toggleOrb;
        public bool moveCountdownOrb;
        public int countdown;
        public bool timeCountdownOrb;                
        public int screenX;
        public int screenY;
        public int bonus = 1;
        public float distance;
        public float scale;
        public bool front = false;
        public bool marked = false;
        public bool scoring = false;
        public bool replace_left = false;
        public bool replace_right = false;
        public bool replace_top = false;
        public bool replace_bottom = false;
        public int replace_distance = 0;
        public PuzzleNode replace_orb;        
    }
}
