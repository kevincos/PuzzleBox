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
        private PuzzleNode[, ,] arr = new PuzzleNode[3, 3, 3];

        public PuzzleBox()
        {
            AllRed();
            //Cross();
            Edges();
            //RandomSetup();
        }

        public PuzzleNode this[int x, int y, int z]
        {
            get
            {
                return arr[x, y, z];
            }
            set
            {
                arr[x, y, z] = value;
            }
        }

        private void Cross()
        {
            for (int z = 0; z < 3; z++)
            {
                arr[0, 1, z] = new PuzzleNode(Color.Yellow);
            }
            for (int z = 0; z < 3; z++)
            {
                arr[2, 1, z] = new PuzzleNode(Color.Orange);
            }
            arr[0, 0, 0] = new PuzzleNode(Color.Green);
            arr[0, 2, 2] = new PuzzleNode(Color.Pink);
            arr[0, 2, 1] = new PuzzleNode(Color.Pink);
        }

        private void Edges()
        {
            arr[0, 2, 1] = new PuzzleNode(Color.Yellow);
            arr[0, 1, 2] = new PuzzleNode(Color.Yellow);
            arr[0, 1, 0] = new PuzzleNode(Color.Yellow);
            arr[0, 0, 1] = new PuzzleNode(Color.Yellow);
            arr[0, 1, 1] = new PuzzleNode(Color.Yellow);            
        }

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
                        arr[x, y, z] = new PuzzleNode();
                    }
                }
            }
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
