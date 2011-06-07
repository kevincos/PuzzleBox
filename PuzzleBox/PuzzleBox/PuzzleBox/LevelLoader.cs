using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBox
{
    public class LevelLoader
    {        
        public static String savePath = "newPuzzle" + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".txt";

        public static StreamWriter w;
        public static StreamReader r;

        private static int gridSize = Engine.gridSize;
        private static int boxSize = Engine.boxSize;
        private static int boxOffset = Engine.boxOffset;
        private static int centerIndex = Engine.centerGridIndex;

        public static void LoadLevel(string fileName, PuzzleBox box, MasterGrid grid)
        {
            String loadPath = "Data\\"+fileName;
            try
            {
                r = new StreamReader(loadPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (grid.queues[x, y] != null)
                        grid.queues[x, y] = new List<PuzzleNode>();
                }
            }
            for(;;)
            {
                String s = r.ReadLine();
                if (s == null)
                    return;
                string[] data = s.Split(',');
                if (data[0] == "WIN")
                {

                }
                if (data[0] == "LOSE")
                {
                }
                if (data[0] == "BOX")
                {
                    int x = Convert.ToInt32(data[1]);
                    int y = Convert.ToInt32(data[2]);
                    int z = Convert.ToInt32(data[3]);

                    box[x, y, z] = new PuzzleNode(data[4]);
                }
                else if (data[0] == "GRID")
                {
                    int x = Convert.ToInt32(data[1]);
                    int y = Convert.ToInt32(data[2]);
                    grid[x, y] = new PuzzleNode(data[3]);
                }
                else if (data[0] == "QUEUE")
                {
                    int x = Convert.ToInt32(data[1]);
                    int y = Convert.ToInt32(data[2]);
                    int z = Convert.ToInt32(data[3]);
                    grid.queues[x, y].Add(new PuzzleNode(data[4]));
                }
            }
        }

        public static void SaveLevel(PuzzleBox box, MasterGrid grid)
        {
            try
            {
                w = new StreamWriter(savePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    for (int z = 0; z < boxSize; z++)
                    {
                        PuzzleNode p = box[x,y,z];
                        w.WriteLine("BOX," + x + "," + y + ","+z+"," + p.ToString());
                    }
                }
            }
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    PuzzleNode p = grid[x, y];
                    w.WriteLine("GRID," + x + "," + y + "," + p.ToString());
                }
            }
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (grid.queues[x, y] != null)
                    {
                        for (int z = 0; z < grid.queues[x, y].Count; z++)
                        {
                            PuzzleNode p = grid.queues[x, y][z];
                            w.WriteLine("QUEUE," + x + "," + y + "," + z + "," + p.ToString());
                        }
                    }
                }
            }
            w.Flush();
            w.Close();
        }
    }
}
