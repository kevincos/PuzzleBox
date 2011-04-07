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
                    
                    int v = r.Next(0, 6);
                    switch (v)
                    {
                        case 0:
                            arr[x, y] = new PuzzleNode(Color.Red);
                            break;
                        case 1:
                            arr[x, y] = new PuzzleNode(Color.Green);
                            break;
                        case 2:
                            arr[x, y] = new PuzzleNode(Color.Orange);
                            break;
                        case 3:
                            arr[x, y] = new PuzzleNode(Color.Magenta);
                            break;
                        case 4:
                            arr[x, y] = new PuzzleNode(Color.Blue);
                            break;
                        case 5:
                            arr[x, y] = new PuzzleNode(Color.Yellow);                            
                            break;
                        default:
                            break;
                    }
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
