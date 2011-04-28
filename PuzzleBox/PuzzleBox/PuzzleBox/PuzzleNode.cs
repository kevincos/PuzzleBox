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
            if ((p1.color.R==128 && p1.color.G ==128 && p1.color.B==128) || p1.color == Color.Black) return false;
            return p1.color == p2.color;
        }

        public static void SetSeed(int seed)
        {
            r = new Random(seed);
        }

        public override string ToString()
        {
            return this.color.R + 
                "-" + this.color.G + 
                "-" + this.color.B + 
                "-" + this.toggleOrb + 
                "-" + this.moveCountdownOrb + 
                "-" + this.timeCountdownOrb + 
                "-" + this.toggleColor.R + 
                "-" + this.toggleColor.G + 
                "-" + this.toggleColor.B + 
                "-" + this.countdown;
        }

        public PuzzleNode(string s)
        {
            string[] data = s.Split('-');
            Color c = Color.White;
            Color tc = Color.White;
            c.R = Convert.ToByte(data[0]);
            c.G = Convert.ToByte(data[1]);
            c.B = Convert.ToByte(data[2]);
            if (c.R == 128 && c.B == 128 && c.G == 128)
            {
                //c.A = 192;
            }
            tc.R = Convert.ToByte(data[6]);
            tc.G = Convert.ToByte(data[7]);
            tc.B = Convert.ToByte(data[8]);
            this.color = c;
            this.toggleColor = tc;
            this.toggleOrb = Convert.ToBoolean(data[3]);
            this.moveCountdownOrb = Convert.ToBoolean(data[4]);
            this.timeCountdownOrb = Convert.ToBoolean(data[5]);
            this.countdown = Convert.ToInt32(data[9]);
        }

        public static Color RandomColor()
        {
            if (r == null)
                r = new Random();
            int v = r.Next(0, Game.currentSettings.numColors);
            switch (v)
            {
                case 0:
                    return Color.Blue;
                case 1:
                    return Color.Yellow;
                case 2:
                    return Color.Red;
                case 3:
                    return Color.Green;
                case 4:
                    return Color.Magenta;
                case 5:
                    return Color.DarkOrange;
                case 6:
                    return Color.Brown;
                case 7:
                    return Color.DarkViolet;
                case 8:
                    return Color.DarkTurquoise;
                case 9:
                    return Color.White;
                default:
                    return Color.Black;
            }     
        }

        public PuzzleNode()
        {
            this.color = PuzzleNode.RandomColor();

            int toggleThreshold = Game.currentSettings.toggleFreq * 30;
            int timerThreshold =  Game.currentSettings.timerFreq * 30;
            int counterThreshold = Game.currentSettings.counterFreq * 30;
            int normal = (int)Math.Max(100 - toggleThreshold - timerThreshold - counterThreshold, 1f / 9f * (toggleThreshold + timerThreshold + counterThreshold));
            int total = toggleThreshold + timerThreshold + counterThreshold + normal;
            float scalingFactor = 1f;
            if (total > 100) scalingFactor = total / 100f;
            toggleThreshold = (int)(toggleThreshold / scalingFactor);
            timerThreshold = (int)(timerThreshold / scalingFactor);
            counterThreshold = (int)(counterThreshold / scalingFactor);
            int v = r.Next(0, 100);
            if (v < toggleThreshold)
            {
                this.toggleColor = PuzzleNode.RandomColor();
                this.toggleOrb = true;
            }
            else if (v >= toggleThreshold && v < toggleThreshold+timerThreshold)
            {
                this.moveCountdownOrb = true;
                this.countdown = 10;
            }
            else if (v >= toggleThreshold + timerThreshold && v < toggleThreshold + timerThreshold+ counterThreshold)
            {
                this.timeCountdownOrb = true;
                this.countdown = 15000;
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
        public bool selected = false;
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
