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
        private static int gridSize = Engine.gridSize;
        private static int boxSize = Engine.boxSize;
        private static int boxOffset = Engine.boxOffset;

        private PuzzleNode[,] arr = new PuzzleNode[gridSize, gridSize];
        public List<PuzzleNode>[,] queues = new List<PuzzleNode>[gridSize, gridSize];

        public MasterGrid()
        {

            //AllBlue();
            //Cross();
            //Edges();
            RandomSetup();
            //WildCross();
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
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (x == 0 || x == gridSize - 1 || y == 0 || y == gridSize-1)
                    {
                        queues[x, y] = new List<PuzzleNode>();
                        for (int i = 0; i < 10; i++)
                            queues[x, y].Add(new PuzzleNode());
                    }
                }
            }
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
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

        private void Cross()
        {
            for (int y = 0; y < gridSize; y++)
            {
                arr[3, y] = new PuzzleNode(Color.Yellow);
            }
            for (int x = 0; x < gridSize; x++)
            {
                arr[x, 3] = new PuzzleNode(Color.Orange);
            }
            arr[1, 0] = new PuzzleNode(Color.Pink);
            arr[0, 1] = new PuzzleNode(Color.Pink);
            arr[4, 3] = new PuzzleNode(Color.Green);
            arr[3, 4] = new PuzzleNode(Color.Green);
        }

        private void WildCross()
        {
            for (int y = 0; y < gridSize; y++)
            {
                arr[3, y] = new PuzzleNode(Color.White);
            }
            for (int x = 0; x < gridSize; x++)
            {
                arr[x, 3] = new PuzzleNode(Color.Yellow);
            }            
        }


        private void RandomSetup()
        {
            Random r = new Random();
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (x < boxOffset && y < boxOffset ||
                        x >= boxOffset + boxSize && y >= boxOffset + boxSize ||
                        x < boxOffset && y >= boxOffset + boxSize ||
                        x >= boxOffset+boxSize && y < boxOffset)
                    {
                        arr[x, y] = new PuzzleNode(Color.Black);
                        continue;
                    }
                    arr[x, y] = new PuzzleNode();
                    if (x == 0 || x == gridSize - 1 || y == 0 || y == gridSize - 1)
                    {
                        queues[x, y] = new List<PuzzleNode>();
                        Color prevColor = Color.Black;
                        for (int i = 0; i < 10; i++)
                        {
                            PuzzleNode p = new PuzzleNode();
                            while(p.color==prevColor)
                                p = new PuzzleNode();
                            queues[x, y].Add(p);
                            prevColor = p.color;
                        }
                    }
                }
            }
        }
    }
}
