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

        public static void SetSeed(int seed)
        {
            r = new Random(seed);
        }

        public PuzzleNode() : this(false)
        {            
        }

        public PuzzleNode(bool initialCreate)
        {            
            int v = r.Next(0, Game.currentSettings.numColors);
            switch (v)
            {
                case 0:
                    this.color = Color.Blue;
                    break;
                case 1:
                    this.color = Color.Yellow;
                    break;
                case 2:
                    this.color = Color.Red;
                    break;
                case 3:
                    this.color = Color.Green;
                    break;
                case 4:
                    this.color = Color.Magenta;
                    break;
                case 5:
                    this.color = Color.DarkOrange;
                    break;
                case 6:
                    this.color = Color.GreenYellow;
                    break;
                case 7:
                    this.color = Color.DarkViolet;
                    break;
                case 8:
                    this.color = Color.DarkTurquoise;
                    break;
                case 9:
                    this.color = Color.White;
                    break;
                default:
                    this.color = Color.Black;
                    break;

            }            
            if (initialCreate==false && Game.currentSettings.mode == GameMode.TimeAttack)
            {
                int toggleThreshold = Game.currentSettings.toggleFreq * 10;
                int timerThreshold = toggleThreshold + Game.currentSettings.timerFreq * 10;
                int counterThreshold = timerThreshold + Game.currentSettings.counterFreq * 10;
                v = r.Next(0, 100);
                if (v < toggleThreshold)
                {
                    this.toggleColor = Color.Gray;
                    this.toggleOrb = true;
                }
                else if (v >= toggleThreshold && v < timerThreshold)
                {
                    this.moveCountdownOrb = true;
                    this.countdown = 10;
                }
                else if (v >= timerThreshold && v < counterThreshold)
                {
                    this.timeCountdownOrb = true;
                    this.countdown = 10000;
                }
                
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
