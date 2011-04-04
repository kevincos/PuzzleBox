using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzleBoxTest
{
    class Program
    {
        public enum ROTATION
        {
            POSX,
            NEGX,
            POSY,
            NEGY,
            POSZ,
            NEGZ
        }

        public static String[,,] Rotate(String[,,] arr, ROTATION r)
        {
            String[, ,] newArr = new String[3, 3, 3];
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
                                newArr[x, y, z] = arr[2-z,y,x];
                                break;
                            case (ROTATION.NEGY):
                                newArr[x, y, z] = arr[z,y,2-x];
                                break;
                            case (ROTATION.NEGX):
                                newArr[x, y, z] = arr[x,2-z,y];
                                break;
                            case (ROTATION.POSX):
                                newArr[x, y, z] = arr[x,z,2-y];
                                break;
                            default:
                                newArr[x, y, z] = arr[x,y,z];
                                break;
                        }
                    }
                }
            }
            return newArr;
        }

        public static void DisplayFront(String[, ,] arr)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Console.Write(" " + arr[x, y, 0]);
                }
                Console.Write("\n");
            }
        }

        public static void Display(String[, ,] arr)
        {
            String[, ,] frontArr = arr;
            String[, ,] topArr = Rotate(arr, ROTATION.POSX);
            String[, ,] bottomArr = Rotate(arr, ROTATION.NEGX);
            String[, ,] leftArr = Rotate(arr, ROTATION.NEGY);
            String[, ,] rightArr = Rotate(arr, ROTATION.POSY);
            Console.WriteLine("           TOP");
            for (int y = 0; y < 3; y++)
            {
                Console.Write("         ");
                for (int x = 0; x < 3; x++)
                {
                    Console.Write(" " + topArr[x, y, 0]);
                }
                Console.Write("\n");
            }
            Console.WriteLine("\n  LEFT    FRONT    RIGHT");
            for (int y = 0; y < 3; y++)
            {
                Console.Write("");
                for (int x = 0; x < 3; x++)
                {
                    Console.Write(" " + leftArr[x, y, 0]);
                }
                Console.Write("   ");
                for (int x = 0; x < 3; x++)
                {
                    Console.Write(" " + frontArr[x, y, 0]);
                }
                Console.Write("   ");
                for (int x = 0; x < 3; x++)
                {
                    Console.Write(" " + rightArr[x, y, 0]);
                }
                Console.Write("\n");
            }
            Console.WriteLine("\n          BOTTOM");
            for (int y = 0; y < 3; y++)
            {
                Console.Write("         ");
                for (int x = 0; x < 3; x++)
                {
                    Console.Write(" " + bottomArr[x, y, 0]);
                }
                Console.Write("\n");
            }
        }

        static void Main(string[] args)
        {
            String[,,] arr = new String[3,3,3];
            Random r = new Random();

            for (int x = 0; x < 3; x++) {         
                for (int y = 0; y < 3; y++) {
                    for(int z = 0; z < 3; z++) {                        
                        int v = r.Next(0,6);
                        switch (v)
                        {
                            case 0:
                                arr[x, y, z] = "R";
                                break;
                            case 1:
                                arr[x, y, z] = "G";                                
                                break;
                            case 2:
                                arr[x, y, z] = "B";                                
                                break;
                            case 3:
                                arr[x, y, z] = "O";
                                break;
                            case 4:
                                arr[x, y, z] = "P";
                                break;
                            case 5:
                                arr[x, y, z] = "Y";
                                break;
                            default:
                                break;

                        }
                    }
                }
            }

            while (true)
            {
                Display(arr);
                Console.WriteLine("Enter Rotation:");
                String command = Console.ReadLine();
                switch (command)
                {
                    case "+Z":
                        arr = Rotate(arr, ROTATION.POSZ);
                        break;
                    case "-Z":
                        arr = Rotate(arr, ROTATION.NEGZ);
                        break;
                    case "+X":
                        arr = Rotate(arr, ROTATION.POSX);
                        break;
                    case "-X":
                        arr = Rotate(arr, ROTATION.NEGX);
                        break;
                    case "+Y":
                        arr = Rotate(arr, ROTATION.POSY);
                        break;
                    case "-Y":
                        arr = Rotate(arr, ROTATION.NEGY);
                        break;
                    case "q":
                        return;
                    default:
                        Console.WriteLine("Invalid Command");
                        break;                        
                }
            }                         
        }
    }
}
