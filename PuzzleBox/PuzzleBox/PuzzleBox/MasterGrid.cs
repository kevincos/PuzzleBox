using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBox
{
    class MasterGrid
    {
        public PuzzleNode[,] arr = new PuzzleNode[7, 7];

        private void AllBlue()
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    arr[x, y] = new PuzzleNode(Color.Blue);
                }
            }
        }

        private void RandomSetup()
        {
            Random r = new Random();
            for (int x = 0; x < 7; x++)
            {                
                for (int y = 0; y < 7; y++)
                {
                    if (x < 2 && y < 2 ||
                        x > 4 && y > 4 ||
                        x < 2 && y > 4 ||
                        x > 4 && y < 2)
                    {
                        arr[x, y] = new PuzzleNode(Color.Black);
                        continue;
                    }
                    arr[x, y] = new PuzzleNode();                    
                }
            }
        }

        public MasterGrid()
        {
            AllBlue();
            //RandomSetup();
        }
    }
}
