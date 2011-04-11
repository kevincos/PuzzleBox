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
        private PuzzleNode[,] arr = new PuzzleNode[7, 7];
        public List<PuzzleNode>[,] queues = new List<PuzzleNode>[7, 7];

        public MasterGrid()
        {
            //AllBlue();
            //Test();
            //Edges();
            RandomSetup();
        }

        public PuzzleNode this[int x, int y]
        {
            get
            {
                return arr[x, y];
            }
            set
            {
                arr[x, y] = value;
            }
        }

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

        private void Edges()
        {
            arr[5, 3] = new PuzzleNode(Color.Yellow);
            arr[3, 1] = new PuzzleNode(Color.Yellow);
            queues[0, 3] = new List<PuzzleNode>();
            queues[0, 3].Add(new PuzzleNode(Color.White));
            queues[6, 3] = new List<PuzzleNode>();
            queues[6, 3].Add(new PuzzleNode(Color.Green));
            queues[3, 0] = new List<PuzzleNode>();
            queues[3, 0].Add(new PuzzleNode(Color.Green));
            queues[3, 6] = new List<PuzzleNode>();
            queues[3, 6].Add(new PuzzleNode(Color.Green));
            queues[0, 3].Add(new PuzzleNode(Color.DarkBlue));
            queues[6, 3].Add(new PuzzleNode(Color.Orange));
            queues[3, 0].Add(new PuzzleNode(Color.Pink));
            queues[3, 6].Add(new PuzzleNode(Color.LightGreen));
            queues[0, 3].Add(new PuzzleNode(Color.Yellow));
            queues[6, 3].Add(new PuzzleNode(Color.Orange));
            queues[3, 0].Add(new PuzzleNode(Color.Pink));
            queues[3, 6].Add(new PuzzleNode(Color.LightGreen));
            queues[0, 3].Add(new PuzzleNode(Color.Red));
            queues[6, 3].Add(new PuzzleNode(Color.Orange));
            queues[3, 0].Add(new PuzzleNode(Color.Pink));
            queues[3, 6].Add(new PuzzleNode(Color.LightGreen));
        }

        private void Test()
        {
            for (int y = 0; y < 7; y++)
            {
                arr[3, y] = new PuzzleNode(Color.Yellow);
            }
            for (int x = 0; x < 7; x++)
            {
                arr[x, 3] = new PuzzleNode(Color.Orange);
            }
            arr[2, 1] = new PuzzleNode(Color.Pink);
            arr[1, 2] = new PuzzleNode(Color.Pink);
            arr[5, 4] = new PuzzleNode(Color.Green);
            arr[4, 5] = new PuzzleNode(Color.Green);
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
                    if (x == 0 || x == 6 || y == 0 || y == 6)
                    {
                        queues[x, y] = new List<PuzzleNode>();
                        for (int i = 0; i < 10; i++)
                            queues[x, y].Add(new PuzzleNode());
                    }
                }
            }
        }
    }
}
