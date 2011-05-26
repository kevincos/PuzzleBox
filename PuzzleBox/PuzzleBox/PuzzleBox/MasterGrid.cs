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
                            if (Game.currentSettings.refillQueues)
                            {
                                PuzzleNode p = new PuzzleNode();
                                while (p.color == prevColor)
                                    p = new PuzzleNode();
                                queues[x, y].Add(p);
                                prevColor = p.color;
                            }
                            else
                            {
                                queues[x, y].Add(new PuzzleNode(Color.Gray));
                            }
                        }
                    }
                }
            }
        }

        public MasterGrid Copy()
        {
            MasterGrid newGrid = new MasterGrid();
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    newGrid[x, y] = arr[x, y].Copy();
                    if (queues[x, y] != null)
                    {
                        newGrid.queues[x, y] = new List<PuzzleNode>();
                        for (int z = 0; z < queues[x, y].Count; z++)
                            newGrid.queues[x, y].Add(queues[x, y][z].Copy());
                    }
                }
            }
            return newGrid;
        }

        public void Mark()
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    arr[x, y].marked = true;
                    if (queues[x, y] != null)
                    {
                        for (int z = 0; z < queues[x, y].Count; z++)
                            queues[x, y][z].marked = true;
                    }
                }
            }
        }


        public void Blank()
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    arr[x, y] = new PuzzleNode(Color.Gray);
                    if (queues[x, y] != null)
                    {
                        queues[x, y] = new List<PuzzleNode>();
                        queues[x, y].Add(new PuzzleNode(Color.Gray));
                        queues[x, y].Add(new PuzzleNode(Color.Gray));
                        queues[x, y].Add(new PuzzleNode(Color.Gray));
                        queues[x, y].Add(new PuzzleNode(Color.Gray));
                        queues[x, y].Add(new PuzzleNode(Color.Gray));
                        queues[x, y].Add(new PuzzleNode(Color.Gray));
                    }
                }
            }
        }

        public void ClearSelection()
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    arr[x, y].selected = false;
                    if(queues[x,y]!=null)
                        for (int z = 0; z < queues[x, y].Count; z++)
                            queues[x, y][z].selected = false;
                }
            }
        }
    }
}
