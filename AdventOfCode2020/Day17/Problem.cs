using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Day17
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("dayseventeen.txt");
            var test = InputParser.GetLines("dayseventeen-test.txt");
            Alternative.Solve(actual);
            //char[,] slice = ParseInputToSlice(actual);
            //RunCycles(slice, 6);
        }

        private static void RunCycles(char[,] slice, int cycles)
        {
            // populate cube
            int len = slice.Length + (cycles * 2);

            int depth = cycles + 20;
            int height = len + 4;
            int width = len + 4;

            char[,,] cube = new char[depth, height, width];

            var fromTop = (cube.GetLength(1) - slice.GetLength(0)) / 2;
            var fromLeft = (cube.GetLength(2) - slice.GetLength(1)) / 2;
            var center = depth / 2;

            for (int z = 0; z < cube.GetLength(0); z++)
            {
                var sliceY = 0;
                bool shouldIncrement = false;
                for (int y = 0; y < cube.GetLength(1); y++)
                {
                    var sliceX = 0;
                    for (int x = 0; x < cube.GetLength(2); x++)
                    {
                        if (z == center && 
                            y >= fromTop && y < fromTop + slice.GetLength(0) &&
                            x >= fromLeft && x < fromLeft + slice.GetLength(1))
                        {
                            cube[z, y, x] = slice[sliceY, sliceX];
                            sliceX++;
                            shouldIncrement = true;
                        }
                        else
                        {
                            cube[z, y, x] = '.';
                        }
                    }
                    if (shouldIncrement)
                        sliceY++;
                }
            }

            //PrintCube(cube);

            for (int i = 1; i <= cycles; i++)
            {
                var newCube = GetEmptyCube(depth, height, width);
                for (int z = 0; z < cube.GetLength(0); z++)
                {
                    for (int y = 0; y < cube.GetLength(1); y++)
                    {
                        for (int x = 0; x < cube.GetLength(2); x++)
                        {
                            //if (z == 2 && y == 11 && x == 10)
                            //    Console.WriteLine("Break here!");
                            int activeNeighbours = FindNumberOfActiveNeighbours(z, y, x, cube);
                            if (cube[z, y, x] == '#')
                            {
                                //var what = FindNumberOfActiveNeighbours(z, y, x, cube);
                                if (activeNeighbours == 2 || activeNeighbours == 3)
                                    newCube[z, y, x] = '#';
                                else
                                    newCube[z, y, x] = '.';
                            }
                            else if (cube[z, y, x] == '.')
                            {
                                if (activeNeighbours == 3)
                                    newCube[z, y, x] = '#';
                                else
                                    newCube[z, y, x] = '.';
                            }
                        }
                    }
                }
                // go through each point, search all neighbours
                // if point is active and exactly 2 or 3 neighbours are also active, keep active. Else turn off
                // if point is inactive and exactly 3 neighbours are active, turn on. Else, keep off
                //PrintCube(newCube);
                cube = newCube;
            }

            int countOfActive = 0;

            for (int z = 0; z < cube.GetLength(0); z++)
            {
                for (int y = 0; y < cube.GetLength(1); y++)
                {
                    for (int x = 0; x < cube.GetLength(2); x++)
                    {
                        if (cube[z, y, x] == '#')
                            countOfActive++;
                    }
                }
            }
            //PrintCube(cube);
            Console.WriteLine(countOfActive);
        }

        private static int FindNumberOfActiveNeighbours(int z, int y, int x, char[,,] cube)
        {
            int activeNeighbours = 0;

            int yCoord = y - 1 > 0 ? y - 1 : y;
            int xCoord = x - 1 > 0 ? x - 1 : x;
            // search 26 spots
            // z - 1
            if (z - 1 > 0)
            {
                int zCoord = z - 1;
                
                activeNeighbours += FindActiveNeighboursInSlice(zCoord, yCoord, xCoord, cube);
            }

            if (z + 1 < cube.GetLength(0))
            {
                int zCoord = z + 1;
                activeNeighbours += FindActiveNeighboursInSlice(zCoord, yCoord, xCoord, cube);
            }

            for (int thisY = yCoord; thisY < yCoord + 3 && thisY < cube.GetLength(1); thisY++)
            {
                for (int thisX = xCoord; thisX < xCoord + 3 && thisX < cube.GetLength(2); thisX++)
                {
                    if (thisY == y && thisX == x)
                        continue;

                    if (cube[z, thisY, thisX] == '#')
                        activeNeighbours++;
                }
            }

            return activeNeighbours;
        }

        private static int FindActiveNeighboursInSlice(int z, int y, int x, char[,,] cube)
        {
            int activeNeighbours = 0;
            for (int thisY = y; thisY < y + 3 && thisY < cube.GetLength(1); thisY++)
            {
                for (int thisX = x; thisX < x + 3 && thisX < cube.GetLength(2); thisX++)
                {
                    if (cube[z, thisY, thisX] == '#')
                        activeNeighbours++;
                }
            }
            return activeNeighbours;
        }


        private static char[,] ParseInputToSlice(string[] input)
        {
            var len = input.Length;
            char[,] cube = new char[len, len];
            
            for (int y = 0; y < input.Length; y++)
            {
                string line = input[y];
                for (int x = 0; x < line.Length; x++)
                {
                    cube[y, x] = line[x];
                }
            }

            return cube;
        }

        private static char[,,] GetEmptyCube(int depth, int height, int width)
        {
            char[,,] cube = new char[depth, height, width];
            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        cube[z, y, x] = '.';
                    }
                }
            }

            return cube;
        }

        private static void PrintCube(char[,,] cube)
        {
            Console.WriteLine("Printing cube:");
            for (int z = 0; z < cube.GetLength(0); z++)
            {
                Console.WriteLine($"Z={z}");
                for (int y = 0; y < cube.GetLength(1); y++)
                {
                    for (int x = 0; x < cube.GetLength(2); x++)
                    {
                        Console.Write(cube[z, y, x]);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
