using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBox
{
    class Matcher
    {
        private static bool StartEndHelper(int start, int end)
        {
            if (end < 3 || start > 4)
                return false;
            if (start > 1 && end < 6)
                return false;
            return (end - start > 1);                
        }

        //Identifies all scoring matches and marks them.
        public static bool Solve(PuzzleBox box, MasterGrid grid)
        {
            bool anythingChanged = false;
            bool[,] solved = new bool[7, 7];

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    grid.arr[x + 2, y + 2] = box.arr[0, y, x];
                }
            }

            // Marking vertical lines
            for (int x = 2; x < 5; x++)
            {
                int start_index = 2;
                int end_index = 2;
                PuzzleNode lastNode = new PuzzleNode(Color.Black);
                for (int y = 0; y < 7; y++)
                {
                    if (grid.arr[x, y] == null)
                        continue;
                    if (PuzzleNode.Match(grid.arr[x, y], lastNode))
                    {
                        end_index++;
                    }
                    else
                    {
                        lastNode = grid.arr[x, y];
                        if (StartEndHelper(start_index, end_index))
                        {
                            anythingChanged = true;
                            for (int i = start_index; i < end_index; i++)
                                solved[x, i] = true;
                        }
                        start_index = y;
                        end_index = y + 1;
                    }
                }
                if (StartEndHelper(start_index, end_index))
                {
                    anythingChanged = true;
                    for (int i = start_index; i < end_index; i++)
                        solved[x, i] = true;
                }
            }

            // Marking horizontal lines
            for (int y = 2; y < 5; y++)
            {
                int start_index = 2;
                int end_index = 2;
                PuzzleNode lastNode = new PuzzleNode(Color.Black);
                for (int x = 0; x < 7; x++)
                {
                    if (grid.arr[x, y] == null)
                        continue;
                    if (PuzzleNode.Match(grid.arr[x, y],lastNode))
                    {
                        end_index++;
                    }
                    else
                    {
                        lastNode = grid.arr[x, y];
                        if (StartEndHelper(start_index, end_index))
                        {
                            anythingChanged = true;
                            for (int i = start_index; i < end_index; i++)
                                solved[i, y] = true;
                        }
                        start_index = x;
                        end_index = x + 1;
                    }
                }
                if (StartEndHelper(start_index, end_index))
                {
                    anythingChanged = true;
                    for (int i = start_index; i < end_index; i++)
                        solved[i, y] = true;
                }
            }

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (solved[x, y])
                        grid.arr[x, y].hightlight = true;
                }
            }


            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    box.arr[0, y, x] = grid.arr[x + 2, y + 2];
                }
            }
            return anythingChanged;
        }

        // Checks if any valid move exists.
        public static bool HasValidMove(PuzzleBox box, MasterGrid grid)
        {            
            HashSet<Color> sideColorsBox = new HashSet<Color>();
            HashSet<Color> cornerColorsBox = new HashSet<Color>();
            HashSet<Color> sideColorsGrid = new HashSet<Color>();
            HashSet<Color> cornerColorsGrid = new HashSet<Color>();
            // Get potential box colors
            for (int x = 0; x < 3; x ++)
            {
                for (int y = 0; y < 3; y ++)
                {
                    for (int z = 0; z < 3; z ++)
                    {
                        int centerCount = 0;
                        if (x == 1) centerCount++;
                        if (y == 1) centerCount++;
                        if (z == 1) centerCount++;
                        if(centerCount==0)
                            cornerColorsBox.Add(box.arr[x, y, z].color);
                        if(centerCount==1)
                            sideColorsBox.Add(box.arr[x, y, z].color);
                    }
                }
            }            
            // Get potential grid colors
            cornerColorsGrid.Add(grid.arr[2, 1].color);
            cornerColorsGrid.Add(grid.arr[2, 5].color);
            sideColorsGrid.Add(grid.arr[3, 1].color);
            sideColorsGrid.Add(grid.arr[3, 5].color);
            cornerColorsGrid.Add(grid.arr[4, 1].color);
            cornerColorsGrid.Add(grid.arr[4, 5].color);


            cornerColorsGrid.Add(grid.arr[1, 2].color);
            cornerColorsGrid.Add(grid.arr[5, 2].color);
            sideColorsGrid.Add(grid.arr[1, 3].color);
            sideColorsGrid.Add(grid.arr[5, 3].color);
            cornerColorsGrid.Add(grid.arr[1, 4].color);
            cornerColorsGrid.Add(grid.arr[5, 4].color);

            sideColorsGrid.IntersectWith(sideColorsBox);
            cornerColorsGrid.IntersectWith(cornerColorsBox);
            return (sideColorsGrid.Count > 0 || cornerColorsGrid.Count > 0);
        }

        // Clears state. Called when returning to ready state.
        public static void Clear(PuzzleBox box, MasterGrid grid)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        box.arr[x, y, z].hightlight = false;
                    }
                }
            }
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    grid.arr[x, y].hightlight = false;
                }
            }
        }

        // Completely randomizes the entire box and grid.
        public static void Reset(PuzzleBox box, MasterGrid grid)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        box.arr[x, y, z] = new PuzzleNode();
                        box.arr[x, y, z].hightlight = true;
                    }
                }
            }
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    grid.arr[x, y] = new PuzzleNode();
                    grid.arr[x, y].hightlight = true;
                }
            }
        }

        // Replaces all marked orbs with new random orbs.
        public static bool Replace(PuzzleBox box, MasterGrid grid)
        {
            bool anythingChanged = false;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        if (box.arr[x, y, z].hightlight == true)
                        {
                            box.arr[x, y, z] = new PuzzleNode();
                            box.arr[x, y, z].hightlight = true;
                            anythingChanged = true;
                        }
                    }
                }
            }
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (grid.arr[x, y].hightlight == true)
                    {
                        grid.arr[x, y] = new PuzzleNode();
                        grid.arr[x, y].hightlight = true;
                        anythingChanged = true;
                    }
                }
            }
            return anythingChanged;
        }
    }
}
