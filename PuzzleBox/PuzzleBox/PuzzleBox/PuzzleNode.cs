﻿using System;
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

        public Color color;
        public int screenX;
        public int screenY;
        public float distance;
        public bool hightlight = false;
    }
}
