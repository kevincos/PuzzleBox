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
        public static bool IsWildCard(PuzzleNode p)
        {
            int count = 0;
            if (p.replace_bottom) count++;
            if (p.replace_top) count++;
            if (p.replace_left) count++;
            if (p.replace_right) count++;
            return count > 1;
        }

        private static bool StartEndHelper(int start, int end)
        {
            if (end < 3 || start > 4)
                return false;
            if (start > 1 && end < 6)
                return false;
            return (end - start > 1);                
        }

        private static void MarkHelperVert(int start, int end, int y, PuzzleNode p)
        {
            p.marked = true;
            p.scoring = true;
            if (start < 2 && end > 5)
            {
                if (y <= 3)
                {
                    p.replace_top = true;
                    p.replace_distance = 3 - start;
                }
                if (y >= 3)
                {
                    p.replace_bottom = true;
                    p.replace_distance = end - 5;
                }
            }
            else
            {
                if (start < 2)
                {
                    p.replace_top = true;
                    p.replace_distance = end - start;
                }
                if (end > 5)
                {
                    p.replace_bottom = true;
                    p.replace_distance = end - start;
                }
            }
        }

        private static void MarkHelperHoriz(int start, int end, int x, PuzzleNode p)
        {
            p.marked = true;
            p.scoring = true;
            if (start < 2 && end > 5)
            {
                if (x <= 3)
                {
                    p.replace_distance = 3 - start;
                    p.replace_left = true;
                }
                if (x >= 3)
                {
                    p.replace_distance = end - 4;
                    p.replace_right = true;
                }
            }
            else
            {
                if (start < 2)
                {
                    p.replace_distance = end - start;
                    p.replace_left = true;
                }
                if (end > 5)
                {
                    p.replace_distance = end - start;
                    p.replace_right = true;
                }
            }
        }

        private static PuzzleNode ReplaceHelper(int x, int y, MasterGrid grid)
        {
            PuzzleNode p = grid[x, y];
            if (IsWildCard(p))
            {
                p.replace_orb = new PuzzleNode();
                p.replace_orb.bonus = 2;
            }
            else if (p.replace_bottom)
            {
                if (y + p.replace_distance < 7)
                    p.replace_orb = new PuzzleNode(grid[x, y + p.replace_distance].color);
                else
                {
                    int queueDepth = y + p.replace_distance - 7;
                    if (queueDepth < grid.queues[x, 6].Count)
                        p.replace_orb = new PuzzleNode(grid.queues[x, 6][queueDepth].color);
                    else
                        p.replace_orb = new PuzzleNode(Color.Black);
                }
            }
            else if (p.replace_top)
            {
                if (y - p.replace_distance >= 0)
                    p.replace_orb = new PuzzleNode(grid[x, y - p.replace_distance].color);
                else
                {
                    int queueDepth = p.replace_distance - y - 1;
                    if (queueDepth < grid.queues[x, 0].Count)
                        p.replace_orb = new PuzzleNode(grid.queues[x, 0][queueDepth].color);
                    else
                        p.replace_orb = new PuzzleNode(Color.Black);
                }
            }
            else if (p.replace_left)
            {
                if (x - p.replace_distance >= 0)
                    p.replace_orb = new PuzzleNode(grid[x - p.replace_distance, y].color);
                else
                {
                    int queueDepth = p.replace_distance - x - 1;
                    if (queueDepth < grid.queues[0, y].Count)
                        p.replace_orb = new PuzzleNode(grid.queues[0, y][queueDepth].color);
                    else
                        p.replace_orb = new PuzzleNode(Color.Black);
                }
            }
            else if (p.replace_right)
            {
                if (x + p.replace_distance < 7)
                    p.replace_orb = new PuzzleNode(grid[x + p.replace_distance, y].color);
                else
                {
                    int queueDepth = x + p.replace_distance - 7;
                    if (queueDepth < grid.queues[6, y].Count)
                        p.replace_orb = new PuzzleNode(grid.queues[6, y][queueDepth].color);
                    else
                        p.replace_orb = new PuzzleNode(Color.Black);
                }
            }
            else
            {
                p.replace_orb = new PuzzleNode(Color.Black);
                //p = new PuzzleNode();
            }

            p.replace_orb.replace_distance = p.replace_distance;
            p.replace_orb.replace_top = p.replace_top;
            p.replace_orb.replace_bottom = p.replace_bottom;
            p.replace_orb.replace_left = p.replace_left;
            p.replace_orb.replace_right = p.replace_right;
            return p;
        }

        //Identifies all scoring matches and marks them.
        public static int Solve(PuzzleBox box, MasterGrid grid)
        {            
            bool[,] solved = new bool[7, 7];

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    grid[x + 2, y + 2] = box[0, y, x];
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
                    if (grid[x, y] == null)
                        continue;
                    if (PuzzleNode.Match(grid[x, y], lastNode))
                    {
                        end_index++;
                    }
                    else
                    {
                        lastNode = grid[x, y];
                        if (StartEndHelper(start_index, end_index))
                        {                            
                            for (int i = start_index; i < end_index; i++)
                            {
                                MarkHelperVert(start_index, end_index, i, grid[x, i]);
                            }
                        }
                        start_index = y;
                        end_index = y + 1;
                    }
                }
                if (StartEndHelper(start_index, end_index))
                {
                    for (int i = start_index; i < end_index; i++)
                    {
                        MarkHelperVert(start_index, end_index, i, grid[x, i]);
                    }
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
                    if (grid[x, y] == null)
                        continue;
                    if (PuzzleNode.Match(grid[x, y],lastNode))
                    {
                        end_index++;
                    }
                    else
                    {
                        lastNode = grid[x, y];
                        if (StartEndHelper(start_index, end_index))
                        {
                            for (int i = start_index; i < end_index; i++)
                            {
                                MarkHelperHoriz(start_index, end_index, i, grid[i, y]);
                            }
                        }
                        start_index = x;
                        end_index = x + 1;
                    }
                }
                if (StartEndHelper(start_index, end_index))
                {
                    for (int i = start_index; i < end_index; i++)
                    {
                        MarkHelperHoriz(start_index, end_index, i, grid[i, y]);
                    }
                }
            }

            // Extend wildcard markings
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (IsWildCard(grid[x, y]))
                    {
                        for (int i = x; i < 7; i++)
                        {
                            if (grid[i, y].marked)
                                grid[i, y].replace_right = true;
                            else
                                break;
                        }
                        for (int i = x; i >= 0; i--)
                        {
                            if (grid[i, y].marked)
                                grid[i, y].replace_left = true;
                            else
                                break;
                        }
                        for (int i = y; i < 7; i++)
                        {
                            if (grid[x, i].marked)
                                grid[x, i].replace_bottom = true;
                            else
                                break;
                        }
                        for (int i = y; i >= 0; i--)
                        {
                            if (grid[x,i].marked)
                                grid[x,i].replace_top = true;
                            else
                                break;
                        }
                    }
                }
            }
            // Mark non-scoring orbs as needing to be replaced
            for (int x = 2; x < 5; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (grid[x, y].replace_bottom && !IsWildCard(grid[x, y]))
                    {
                        for (int i = y; i < 7; i++)
                        {
                            grid[x,i].marked = true;
                            grid[x,i].replace_bottom = true;
                            grid[x,i].replace_distance = grid[x, y].replace_distance;
                        }
                        break;
                    }
                }
                for (int y = 6; y >=0; y--)
                {
                    if (grid[x, y].replace_top && !IsWildCard(grid[x, y]))
                    {
                        for (int i = y; i >=0; i--)
                        {
                            grid[x, i].marked = true;
                            grid[x, i].replace_top = true;
                            grid[x, i].replace_distance = grid[x, y].replace_distance;
                        }
                        break;
                    }
                }
            }
            for (int y = 2; y < 5; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (grid[x, y].replace_right && !IsWildCard(grid[x, y]))
                    {
                        for (int i = x; i < 7; i++)
                        {
                            grid[i, y].marked = true;
                            grid[i, y].replace_right = true;
                            grid[i, y].replace_distance = grid[x, y].replace_distance;
                        }
                        break;
                    }
                }
                for (int x = 6; x >= 0; x--)
                {
                    if (grid[x, y].replace_left && !IsWildCard(grid[x, y]))
                    {
                        for (int i = x; i >= 0; i--)
                        {
                            grid[i, y].marked = true;
                            grid[i, y].replace_left = true;
                            grid[i, y].replace_distance = grid[x, y].replace_distance;
                        }
                        break;
                    }
                }
            }

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (IsWildCard(grid[x, y]))
                    {
                        for (int i = x; i < 7; i++)
                        {
                            if (grid[i, y].marked)
                                grid[i, y].replace_distance--;
                            else
                                break;
                        }
                        for (int i = x; i >= 0; i--)
                        {
                            if (grid[i, y].marked)
                                grid[i, y].replace_distance--;
                            else
                                break;
                        }
                        for (int i = y; i < 7; i++)
                        {
                            if (grid[x, i].marked)
                                grid[x, i].replace_distance--;
                            else
                                break;
                        }
                        for (int i = y; i >= 0; i--)
                        {
                            if (grid[x, i].marked)
                                grid[x, i].replace_distance--;
                            else
                                break;
                        }
                    }
                }
            }

            int max_distance = 0;
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (grid[x, y].replace_distance > max_distance) max_distance = grid[x, y].replace_distance;
                }
            }

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    box[0, y, x] = grid[x + 2, y + 2];
                }
            }
            return max_distance;        
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
                            cornerColorsBox.Add(box[x, y, z].color);
                        if(centerCount==1)
                            sideColorsBox.Add(box[x, y, z].color);
                    }
                }
            }            
            // Get potential grid colors
            cornerColorsGrid.Add(grid[2, 1].color);
            cornerColorsGrid.Add(grid[2, 5].color);
            sideColorsGrid.Add(grid[3, 1].color);
            sideColorsGrid.Add(grid[3, 5].color);
            cornerColorsGrid.Add(grid[4, 1].color);
            cornerColorsGrid.Add(grid[4, 5].color);


            cornerColorsGrid.Add(grid[1, 2].color);
            cornerColorsGrid.Add(grid[5, 2].color);
            sideColorsGrid.Add(grid[1, 3].color);
            sideColorsGrid.Add(grid[5, 3].color);
            cornerColorsGrid.Add(grid[1, 4].color);
            cornerColorsGrid.Add(grid[5, 4].color);

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
                        box[x, y, z].ClearMarking();
                    }
                }
            }
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    grid[x, y].ClearMarking();
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
                        box[x, y, z] = new PuzzleNode();
                        box[x, y, z].marked = true;
                    }
                }
            }
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    grid[x, y] = new PuzzleNode();
                    grid[x, y].marked = true;
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
                    grid[x + 2, y + 2] = box[0, y, x];
                }
            }

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (grid[x, y].marked == true)
                    {
                        grid[x, y] = ReplaceHelper(x,y,grid);
                        anythingChanged = true;
                    }
                }
            }

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if(grid[x,y].marked==true)
                    {
                        if (x == 0 || x == 6 || y == 0 || y == 6)
                        {
                            for (int i = 0; i < grid[x, y].replace_distance; i++)
                            {
                                grid.queues[x, y].RemoveAt(0);
                                grid.queues[x, y].Add(new PuzzleNode());
                            }
                        } 
                        grid[x, y] = grid[x, y].replace_orb;
                        grid[x, y].marked = true;
                    }
                }
            }

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    box[0, y, x] = grid[x + 2, y + 2];
                }
            }
            return anythingChanged;
        }
    }
}
