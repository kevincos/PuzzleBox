using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBox
{
    class PuzzleBox
    {
        public PuzzleNode[, ,] arr = new PuzzleNode[3, 3, 3];

        private void AllRed()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        arr[x, y, z] = new PuzzleNode(Color.Red);
                    }
                }
            }
        }

        private void RandomSetup()
        {
            Random r = new Random();

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        int v = r.Next(0, 6);
                        switch (v)
                        {
                            case 0:
                                arr[x, y, z] = new PuzzleNode(Color.Red);
                                break;
                            case 1:
                                arr[x, y, z] = new PuzzleNode(Color.Green);
                                break;
                            case 2:
                                arr[x, y, z] = new PuzzleNode(Color.Orange);
                                break;
                            case 3:
                                arr[x, y, z] = new PuzzleNode(Color.Magenta);
                                break;
                            case 4:
                                arr[x, y, z] = new PuzzleNode(Color.Blue);
                                break;
                            case 5:
                                arr[x, y, z] = new PuzzleNode(Color.Yellow);
                                break;
                            default:
                                break;

                        }
                    }
                }
            }
        }

        public PuzzleBox()
        {
            AllRed();
            //RandomSetup();
        }

        public enum ROTATION
        {
            POSX,
            NEGX,
            POSY,
            NEGY,
            POSZ,
            NEGZ
        }        

        public void Rotate(ROTATION r)
        {
            PuzzleNode[, ,] newArr = new PuzzleNode[3, 3, 3];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        switch (r)
                        {
                            case (ROTATION.POSZ):
                                newArr[x, y, z] = arr[y, 2 - x, z];
                                break;
                            case (ROTATION.NEGZ):
                                newArr[x, y, z] = arr[2 - y, x, z];
                                break;
                            case (ROTATION.POSY):
                                newArr[x, y, z] = arr[2 - z, y, x];
                                break;
                            case (ROTATION.NEGY):
                                newArr[x, y, z] = arr[z, y, 2 - x];
                                break;
                            case (ROTATION.NEGX):
                                newArr[x, y, z] = arr[x, 2 - z, y];
                                break;
                            case (ROTATION.POSX):
                                newArr[x, y, z] = arr[x, z, 2 - y];
                                break;
                            default:
                                newArr[x, y, z] = arr[x, y, z];
                                break;
                        }                        
                    }
                }                
            }
            arr = newArr;
        }
    }
}
