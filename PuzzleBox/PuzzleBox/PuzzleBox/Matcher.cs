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
        private static int gridSize = Game1.gridSize;
        private static int boxSize = Game1.boxSize;
        private static int boxOffset = Game1.boxOffset;
        private static int centerIndex = Game1.centerGridIndex;

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
            if (end <= boxOffset || start >= boxOffset+boxSize)
                return false;
            if (start >= boxOffset && end <= boxOffset+boxSize)
                return false;
            return (end - start > 1);                
        }

        private static ScoringSet CreateScoreHelperVert(int start, int end, int x, PuzzleNode p)
        {
            ScoringSet s = new ScoringSet();
            s.start = start;
            s.end = end;
            s.drawX = p.screenX;
            s.drawY = p.screenY;
            s.color = p.color;
            s.x = x;
            s.y = -1;
            if (start < boxOffset)
                s.drawY -= 50;
            else
                s.drawY += 50;
            return s;
        }

        private static ScoringSet CreateScoreHelperHoriz(int start, int end, int y, PuzzleNode p)
        {
            ScoringSet s = new ScoringSet();
            s.start = start;
            s.end = end;
            s.drawX = p.screenX;
            s.drawY = p.screenY;
            s.color = p.color;
            s.x = -1;
            s.y = y;
            if (start < boxOffset)            
                s.drawX -= 50;            
            else
                s.drawX += 50;
            return s;
        }

        private static void MarkHelperVert(int start, int end, int y, PuzzleNode p)
        {
            p.marked = true;
            p.scoring = true;
            if (start < boxOffset && end > boxOffset+boxSize)
            {
                if (y <= centerIndex)
                {
                    p.replace_top = true;
                    p.replace_distance = 1+centerIndex - start;
                }
                if (y >= centerIndex)
                {
                    p.replace_bottom = true;
                    p.replace_distance = end - centerIndex;
                }
            }
            else
            {
                if (start < boxOffset)
                {
                    p.replace_top = true;
                    p.replace_distance = end - start;
                }
                if (end > boxOffset+boxSize)
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
            if (start < boxOffset && end > boxSize+boxOffset)
            {
                if (x <= centerIndex)
                {
                    p.replace_distance = centerIndex+1 - start;
                    p.replace_left = true;
                }
                if (x >= centerIndex)
                {
                    p.replace_distance = end - centerIndex;
                    p.replace_right = true;
                }
            }
            else
            {
                if (start < boxOffset)
                {
                    p.replace_distance = end - start;
                    p.replace_left = true;
                }
                if (end > boxOffset+boxSize)
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
                p.replace_orb = new PuzzleNode(p.color);
                p.replace_orb.bonus = 2;
            }
            else if (p.replace_bottom)
            {
                if (y + p.replace_distance < gridSize)
                    p.replace_orb = grid[x, y + p.replace_distance];
                else
                {
                    int queueDepth = y + p.replace_distance - gridSize;
                    if (queueDepth < grid.queues[x, gridSize-1].Count)
                        p.replace_orb = grid.queues[x, gridSize-1][queueDepth];
                    else
                        p.replace_orb = new PuzzleNode(Color.Black);
                }
            }
            else if (p.replace_top)
            {
                if (y - p.replace_distance >= 0)
                    p.replace_orb = grid[x, y - p.replace_distance];
                else
                {
                    int queueDepth = p.replace_distance - y - 1;
                    if (queueDepth < grid.queues[x, 0].Count)
                        p.replace_orb = grid.queues[x, 0][queueDepth];
                    else
                        p.replace_orb = new PuzzleNode(Color.Black);
                }
            }
            else if (p.replace_left)
            {
                if (x - p.replace_distance >= 0)
                    p.replace_orb = grid[x - p.replace_distance, y];
                else
                {
                    int queueDepth = p.replace_distance - x - 1;
                    if (queueDepth < grid.queues[0, y].Count)
                        p.replace_orb = grid.queues[0, y][queueDepth];
                    else
                        p.replace_orb = new PuzzleNode(Color.Black);
                }
            }
            else if (p.replace_right)
            {
                if (x + p.replace_distance < gridSize)
                    p.replace_orb = grid[x + p.replace_distance, y];
                else
                {
                    int queueDepth = x + p.replace_distance - gridSize;
                    if (queueDepth < grid.queues[gridSize-1, y].Count)
                        p.replace_orb = grid.queues[gridSize-1, y][queueDepth];
                    else
                        p.replace_orb = new PuzzleNode(Color.Black);
                }
            }
            else
            {
                p.replace_orb = new PuzzleNode(Color.Black);
            }

            p.replace_orb.replace_distance = p.replace_distance;
            p.replace_orb.replace_top = p.replace_top;
            p.replace_orb.replace_bottom = p.replace_bottom;
            p.replace_orb.replace_left = p.replace_left;
            p.replace_orb.replace_right = p.replace_right;
            return p;
        }

        //Identifies all scoring matches and marks them.
        public static List<ScoringSet> Solve(PuzzleBox box, MasterGrid grid)
        {            
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    grid[x + boxOffset, y + boxOffset] = box[0, y, x];
                }
            }

            List<ScoringSet> scoringSets = new List<ScoringSet>();

            // Marking vertical lines
            for (int x = boxOffset; x < boxOffset+boxSize; x++)
            {
                int start_index = 0;
                int end_index = 0;
                PuzzleNode lastNode = new PuzzleNode(Color.Black);
                for (int y = 0; y < gridSize; y++)
                {
                    if (grid[x, y] == null)
                        continue;
                    if (PuzzleNode.Match(lastNode, grid[x, y]))
                    {
                        lastNode = grid[x, y];
                        end_index++;
                    }
                    else
                    {
                        lastNode = grid[x, y];
                        if (StartEndHelper(start_index, end_index))
                        {
                            ScoringSet s;
                            if (start_index < boxOffset)
                                s = CreateScoreHelperVert(start_index, end_index, x, grid[x, start_index]);
                            else
                                s = CreateScoreHelperVert(start_index, end_index, x, grid[x, y]);
                            for (int i = start_index; i < end_index; i++)
                            {
                                s.multiplier *= grid[x, i].bonus;
                                MarkHelperVert(start_index, end_index, i, grid[x, i]);
                            }
                            scoringSets.Add(s);
                        }
                        start_index = y;
                        end_index = y + 1;
                    }
                }
                if (StartEndHelper(start_index, end_index))
                {
                    ScoringSet s = CreateScoreHelperVert(start_index, end_index, x, grid[x, end_index-1]);
                    for (int i = start_index; i < end_index; i++)
                    {
                        s.multiplier *= grid[x, i].bonus;
                        MarkHelperVert(start_index, end_index, i, grid[x, i]);
                    }
                    scoringSets.Add(s);
                }
            }

            // Marking horizontal lines
            for (int y = boxOffset; y < boxOffset+boxSize; y++)
            {
                int start_index = 0;
                int end_index = 0;
                PuzzleNode lastNode = new PuzzleNode(Color.Black);
                for (int x = 0; x < gridSize; x++)
                {
                    if (grid[x, y] == null)
                        continue;
                    if (PuzzleNode.Match(lastNode, grid[x, y]))
                    {
                        lastNode = grid[x, y];
                        end_index++;
                    }
                    else
                    {
                        lastNode = grid[x, y];
                        if (StartEndHelper(start_index, end_index))
                        {
                            ScoringSet s;
                            if(start_index < boxOffset)
                                s = CreateScoreHelperHoriz(start_index, end_index, y, grid[start_index, y]);                                                    
                            else
                                s = CreateScoreHelperHoriz(start_index, end_index, y, grid[x, y]);                                                    
                            for (int i = start_index; i < end_index; i++)
                            {
                                s.multiplier *= grid[i, y].bonus;
                                MarkHelperHoriz(start_index, end_index, i, grid[i, y]);
                            }
                            scoringSets.Add(s);
                        }
                        start_index = x;
                        end_index = x + 1;
                    }
                }
                if (StartEndHelper(start_index, end_index))
                {
                    ScoringSet s = CreateScoreHelperHoriz(start_index, end_index, y, grid[end_index-1, y]);  
                                                                              
                    for (int i = start_index; i < end_index; i++)
                    {
                        s.multiplier *= grid[i, y].bonus;
                        MarkHelperHoriz(start_index, end_index, i, grid[i, y]);
                    }
                    scoringSets.Add(s);
                }
            }

            // Extend wildcard markings
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (IsWildCard(grid[x, y]))
                    {
                        for (int i = x; i < gridSize; i++)
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
                        for (int i = y; i < gridSize; i++)
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
            for (int x = boxOffset; x < boxOffset+boxSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (grid[x, y].replace_bottom && !IsWildCard(grid[x, y]))
                    {
                        for (int i = y; i < gridSize; i++)
                        {
                            grid[x,i].marked = true;
                            grid[x,i].replace_bottom = true;
                            grid[x,i].replace_distance = grid[x, y].replace_distance;
                        }
                        break;
                    }
                }
                for (int y = gridSize-1; y >=0; y--)
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
            for (int y = boxOffset; y < boxOffset+boxSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    if (grid[x, y].replace_right && !IsWildCard(grid[x, y]))
                    {
                        for (int i = x; i < gridSize; i++)
                        {
                            grid[i, y].marked = true;
                            grid[i, y].replace_right = true;
                            grid[i, y].replace_distance = grid[x, y].replace_distance;
                        }
                        break;
                    }
                }
                for (int x = gridSize-1; x >= 0; x--)
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

            // fix replace distances
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (IsWildCard(grid[x, y]))
                    {
                        for (int i = x; i < gridSize; i++)
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
                        for (int i = y; i < gridSize; i++)
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

            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    box[0, y, x] = grid[x + boxOffset, y + boxOffset];
                }
            }
            return scoringSets;        
        }

        public static int GetMaxReplaceDistance(PuzzleBox box, MasterGrid grid)
        {
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    grid[x + boxOffset, y + boxOffset] = box[0, y, x];
                }
            }

            int max_distance = 0;
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (grid[x, y].replace_distance > max_distance) max_distance = grid[x, y].replace_distance;
                }
            }

            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    box[0, y, x] = grid[x + boxOffset, y + boxOffset];
                }
            }
            return max_distance;     
        }
        
        public static void UpdateMoveCountdown(PuzzleBox box, MasterGrid grid)
        {
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    for (int z = 0; z < boxSize; z++)
                    {
                        if (box[x, y, z].moveCountdownOrb)
                        {
                            box[x, y, z].countdown--;
                            if (box[x, y, z].countdown == 0)
                            {
                                box[x, y, z].moveCountdownOrb = false;
                                box[x, y, z].color = Color.Gray;
                            }
                        }
                        if (box[x, y, z].toggleOrb)
                        {
                            Color temp = box[x, y, z].color;
                            box[x, y, z].color = box[x, y, z].toggleColor;
                            box[x, y, z].toggleColor = temp;

                        }
                    }
                }
            }
        }

        public static void UpdateTimeCountdown(PuzzleBox box, MasterGrid grid, int elapsedTime)
        {
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    for (int z = 0; z < boxSize; z++)
                    {
                        if (box[x, y, z].timeCountdownOrb)
                        {
                            box[x, y, z].countdown-=elapsedTime;
                            if (box[x, y, z].countdown <= 0)
                            {
                                box[x, y, z].timeCountdownOrb = false;
                                box[x, y, z].color = Color.Gray;
                            }
                        }
                    }
                }
            }
        }


        // Checks if any valid move exists.
        public static bool HasValidMove(PuzzleBox box, MasterGrid grid)
        {            
            HashSet<Color> sideColorsBox = new HashSet<Color>();
            HashSet<Color> cornerColorsBox = new HashSet<Color>();
            HashSet<Color> sideColorsGrid = new HashSet<Color>();
            HashSet<Color> cornerColorsGrid = new HashSet<Color>();
            // Get potential box colors
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    for (int z = 0; z < boxSize; z++)
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
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    for (int z = 0; z < boxSize; z++)
                    {
                        box[x, y, z].ClearMarking();
                    }
                }
            }
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (x == 0 || x == gridSize-1 || y == 0 || y == gridSize-1)
                    {
                        for (int i = 0; i < grid[x, y].replace_distance; i++)
                        {
                            grid.queues[x, y].RemoveAt(0);
                            grid.queues[x, y].Add(new PuzzleNode());
                        }
                    } 
                    grid[x, y].ClearMarking();
                }
            }
        }

        // Completely randomizes the entire box and grid.
        public static void Reset(PuzzleBox box, MasterGrid grid)
        {
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    for (int z = 0; z < boxSize; z++)
                    {
                        box[x, y, z] = new PuzzleNode();
                        box[x, y, z].marked = true;
                    }
                }
            }
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
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
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    grid[x + boxOffset, y + boxOffset] = box[0, y, x];
                }
            }

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (grid[x, y].marked == true)
                    {
                        grid[x, y] = ReplaceHelper(x,y,grid);
                        anythingChanged = true;
                    }
                }
            }

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if(grid[x,y].marked==true)
                    {
                        grid[x, y] = grid[x, y].replace_orb;
                        grid[x, y].marked = true;
                    }
                }
            }

            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    box[0, y, x] = grid[x + boxOffset, y + boxOffset];
                }
            }
            return anythingChanged;
        }
    }
}
