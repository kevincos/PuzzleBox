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
        private static int gridSize = Engine.gridSize;
        private static int boxSize = Engine.boxSize;
        private static int boxOffset = Engine.boxOffset;
        private static int centerIndex = Engine.centerGridIndex;

        private static void AddBoxToGrid(PuzzleBox box, MasterGrid grid)
        {
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    grid[x + boxOffset, y + boxOffset] = box[box.activeZ, y, x];
                }
            }
        }

        private static void SetBoxFromGrid(PuzzleBox box, MasterGrid grid)
        {
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    box[box.activeZ, y, x] = grid[x + boxOffset, y + boxOffset];
                }
            }
        }

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
            if (p.replace_orb == null)
                p.replace_orb.bonus = 9;
            return p;
        }

        //Identifies all scoring matches and marks them.
        public static List<ScoringSet> Solve(PuzzleBox box, MasterGrid grid)
        {
            Matcher.AddBoxToGrid(box, grid);

            List<ScoringSet> scoringSets = new List<ScoringSet>();

            // Marking orbs
            #region InitialOrbMarking
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
            #endregion

            // Extend wildcard markings
            #region ExtendWildcardMarkings
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (IsWildCard(grid[x, y]))
                    {
                        for (int i = x+1; i < gridSize; i++)
                        {
                            if (grid[i, y].marked)
                                grid[i, y].replace_right = true;
                            else
                                break;
                        }
                        for (int i = x-1; i >= 0; i--)
                        {
                            if (grid[i, y].marked)
                                grid[i, y].replace_left = true;
                            else
                                break;
                        }
                        for (int i = y+1; i < gridSize; i++)
                        {
                            if (grid[x, i].marked)
                                grid[x, i].replace_bottom = true;
                            else
                                break;
                        }
                        for (int i = y-1; i >= 0; i--)
                        {
                            if (grid[x, i].marked)
                                grid[x,i].replace_top = true;
                            else
                                break;
                        }
                    }
                }
            }
            #endregion

            // Mark non-scoring orbs as needing to be replaced
            #region MarkNonScoringOrbsForReplacement
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
            #endregion

            // fix replace distances
            #region FixReplaceDistances
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (IsWildCard(grid[x, y]))
                    {
                        if(grid[x,y].replace_right)
                            for (int i = x; i < gridSize; i++)
                            {
                                if (grid[i, y].marked)
                                    grid[i, y].replace_distance--;
                                else
                                    break;
                            }
                        if(grid[x,y].replace_left)
                            for (int i = x; i >= 0; i--)
                            {
                                if (grid[i, y].marked)
                                    grid[i, y].replace_distance--;
                                else
                                    break;
                            }
                        if(grid[x,y].replace_bottom)
                            for (int i = y; i < gridSize; i++)
                            {
                                if (grid[x, i].marked)
                                    grid[x, i].replace_distance--;
                                else
                                    break;
                            }
                        if(grid[x,y].replace_top)
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
            #endregion

            Matcher.SetBoxFromGrid(box, grid);
            return scoringSets;        
        }

        public static int GetMaxReplaceDistance(PuzzleBox box, MasterGrid grid)
        {
            AddBoxToGrid(box, grid);

            int max_distance = 0;
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (IsWildCard(grid[x, y]) == true && max_distance == 0)
                        max_distance = 1;                    
                    if (grid[x, y].replace_distance > max_distance) max_distance = grid[x, y].replace_distance;
                }
            }

            SetBoxFromGrid(box, grid);
            return max_distance;     
        }

        public static void UpdateToggleState(PuzzleBox box, MasterGrid grid)
        {
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    for (int z = 0; z < boxSize; z++)
                    {
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

            List<Color> sideColorsBox1 = new List<Color>();
            List<Color> sideColorsBox2 = new List<Color>();
            List<Color> cornerColorsBox = new List<Color>();
            List<Color> faceColorsBox = new List<Color>();
            List<Color> sideColorsGrid = new List<Color>();
            List<Color> cornerColorsGrid = new List<Color>();
            // Get potential box colors
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    for (int z = 0; z < boxSize; z++)
                    {
                        PuzzleNode p = box[x,y,z];
                        int centerCount = 0;
                        if (x == 1) centerCount++;
                        if (y == 1) centerCount++;
                        if (z == 1) centerCount++;
                        if (centerCount == 0)
                        {
                            cornerColorsBox.Add(p.color);
                            if(p.toggleOrb)
                                cornerColorsBox.Add(p.toggleColor);
                        }
                        if (centerCount == 1)
                        {
                            sideColorsBox1.Add(p.color);
                            sideColorsBox2.Add(p.color);
                            if (p.toggleOrb)
                            {
                                sideColorsBox1.Add(p.toggleColor);
                                sideColorsBox2.Add(p.toggleColor);                            
                            }
                        }
                        if (centerCount == 2)
                        {                            
                            faceColorsBox.Add(p.color);
                            if (p.toggleOrb)
                                faceColorsBox.Add(p.toggleColor);
                        }
                    }
                }
            }            
            // Get potential grid colors
            cornerColorsGrid.Add(grid[1, 0].color);
            if (grid[1, 0].toggleOrb) cornerColorsGrid.Add(grid[1, 0].toggleColor);
            cornerColorsGrid.Add(grid[3, 0].color);
            if (grid[3, 0].toggleOrb) cornerColorsGrid.Add(grid[3, 0].toggleColor);
            sideColorsGrid.Add(grid[2, 0].color);
            if (grid[2, 0].toggleOrb) cornerColorsGrid.Add(grid[2, 0].toggleColor);
            sideColorsGrid.Add(grid[2, 4].color);
            if (grid[2, 4].toggleOrb) cornerColorsGrid.Add(grid[2, 4].toggleColor);
            cornerColorsGrid.Add(grid[1, 4].color);
            if (grid[1, 4].toggleOrb) cornerColorsGrid.Add(grid[1, 4].toggleColor);
            cornerColorsGrid.Add(grid[3, 4].color);
            if (grid[3, 4].toggleOrb) cornerColorsGrid.Add(grid[3, 4].toggleColor);

            cornerColorsGrid.Add(grid[0,1].color);
            if (grid[0, 1].toggleOrb) cornerColorsGrid.Add(grid[0, 1].toggleColor);
            cornerColorsGrid.Add(grid[0,3].color);
            if (grid[0, 3].toggleOrb) cornerColorsGrid.Add(grid[0, 3].toggleColor);
            sideColorsGrid.Add(grid[0,2].color);
            if (grid[0, 2].toggleOrb) cornerColorsGrid.Add(grid[0, 2].toggleColor);
            sideColorsGrid.Add(grid[4,2].color);
            if (grid[4, 2].toggleOrb) cornerColorsGrid.Add(grid[4, 2].toggleColor);
            cornerColorsGrid.Add(grid[4, 1].color);
            if (grid[4, 1].toggleOrb) cornerColorsGrid.Add(grid[4, 1].toggleColor);
            cornerColorsGrid.Add(grid[4, 3].color);
            if (grid[4, 3].toggleOrb) cornerColorsGrid.Add(grid[4, 3].toggleColor);
            
            foreach (Color c in faceColorsBox)
            {
                if (c == Color.Gray || c == Game.currentSettings.dangerColor)
                    continue;
                if (sideColorsGrid.Contains(c))
                    return true;
            }
            foreach (Color c in sideColorsBox1)
            {
                if (c == Color.Gray || c == Game.currentSettings.dangerColor)
                    continue;
                if (cornerColorsGrid.Contains(c) || sideColorsGrid.Contains(c))
                    return true;
            }
            foreach (Color c in cornerColorsBox)
            {
                if (c == Color.Gray || c == Game.currentSettings.dangerColor)
                    continue;
                if (cornerColorsGrid.Contains(c))
                    return true;
            }
            return false;
        }

        public static bool AllGray(PuzzleBox box, MasterGrid grid)
        {
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    for (int z = 0; z < boxSize; z++)
                    {
                        if(box[x, y, z].color!=Color.Gray && box[x,y,z].color!=Game.currentSettings.dangerColor)
                            return false;
                        if (box[x, y, z].toggleColor != Color.Gray && box[x, y, z].toggleColor != Game.currentSettings.dangerColor)
                            return false;
                    }
                }
            }
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if(grid.queues[x,y]!=null)
                    {
                        for (int i = 0; i < grid.queues[x,y].Count; i++)
                        {
                            if (grid.queues[x, y][i].color != Color.Gray && grid.queues[x, y][i].color != Game.currentSettings.dangerColor)
                                return false;
                        }
                    }
                    if (grid[x, y].color != Color.Gray && grid[x, y].color != Game.currentSettings.dangerColor)
                        return false;
                }
            }
            return true;
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
                    //if (x == 0 || x == gridSize-1 || y == 0 || y == gridSize-1)
                    if(grid.queues[x,y]!=null)
                    {
                        for (int i = 0; i < grid[x, y].replace_distance; i++)
                        {
                            grid.queues[x, y].RemoveAt(0);
                            if (Game.currentSettings.refillQueues)
                            {
                                PuzzleNode p = new PuzzleNode();
                                while (p.color == grid.queues[x, y][grid.queues[x, y].Count - 1].color)
                                {
                                    p = new PuzzleNode();
                                }
                                grid.queues[x, y].Add(p);
                            }
                            else
                                grid.queues[x, y].Add(new PuzzleNode(Color.Gray));
                        }
                        for (int i = 0; i < grid.queues[x, y].Count; i++)
                        {
                            grid.queues[x, y][i].ClearMarking();
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
                        //if (box[x, y, z].moveCountdownOrb == true) box[x, y, z].color = Color.Gray;
                        //if (box[x, y, z].timeCountdownOrb == true) box[x, y, z].color = Color.Gray;

                            box[x, y, z] = new PuzzleNode();
                            box[x, y, z].marked = true;
                        //}
                    }
                }
            }
            Random r = new Random();
            for (int i = 0; i < Game.currentSettings.grayOrbStart;)
            {
                int x = r.Next(3);
                int y = r.Next(3);
                int z = r.Next(3);
                if (box[x, y, z].color != Color.Gray)
                {
                    box[x, y, z] = new PuzzleNode(Color.Gray);
                    box[x, y, z].marked = true;
                    i++;
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
            AddBoxToGrid(box, grid);

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

            SetBoxFromGrid(box, grid);
            return anythingChanged;
        }
    }
}
