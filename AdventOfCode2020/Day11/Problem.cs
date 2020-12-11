using AdventOfCode2020.Common;
using System;
using System.Linq;

namespace AdventOfCode2020.Day11
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("dayeleven.txt");
            var test = InputParser.GetLines("dayeleven-test.txt");
            RunRoundsUntilStable(actual);
        }

        private static void RunRoundsUntilStable(string[] layout)
        {
            bool isEqual;
            do
            {
                var newLayout = RunSingleRound(layout);
                isEqual = layout.SequenceEqual(newLayout);
                layout = newLayout;
            } while (!isEqual);

            Console.WriteLine($"Number: {layout.Sum(l => l.Count(c => c == '#'))}");
        }

        private static string[] RunSingleRound(string[] initialLayout)
        {
            var height = initialLayout.Length;
            var width = initialLayout[0].Length;

            var outLayout = new string[initialLayout.Length];
            for (int y = 0; y < height; y++)
            {
                outLayout[y] = "";
                for (int x = 0; x < width; x++)
                {
                    switch (initialLayout[y][x])
                    {
                        case 'L':   // Current seat is empty.
                            if (AreThereAnyOccupiedSeatsWithinSight(y, x, initialLayout))
                            {
                                outLayout[y] += "#";
                            }
                            else
                            {
                                outLayout[y] += "L";
                            }

                            break;
                        case '#':   // Current seat is occupied
                            if (TheNumberOfOccupiedSeatsWithinSightAreGreaterThanFive(y, x, initialLayout))
                            {
                                outLayout[y] += "L";
                            }
                            else
                            {
                                outLayout[y] += "#";
                            }
                            break;
                        default:    // This is a floor spot.
                            outLayout[y] += ".";
                            break;
                    }
                }
            }

            return outLayout;
        }

        private static bool AreThereAnyOccupiedSeatsWithinSight(int y, int x, string[] seats)
        {
            if (CheckEast(y, x, seats))
                return false;

            if (CheckSouthEast(y, x, seats))
                return false;

            if (CheckSouth(y, x, seats))
                return false;

            if (CheckSouthWest(y, x, seats))
                return false;

            if (CheckWest(y, x, seats))
                return false;

            if (CheckNorthWest(y, x, seats))
                return false;

            if (CheckNorth(y, x, seats))
                return false;

            if (CheckNorthWest(y, x, seats))
                return false;

            if (CheckNorthEast(y, x, seats))
                return false;

            return true;
        }

        private static bool TheNumberOfOccupiedSeatsWithinSightAreGreaterThanFive(int y, int x, string[] seats)
        {
            int numberOfOccupiedSeats = 0;
            if (CheckEast(y, x, seats))
                numberOfOccupiedSeats++;

            if (CheckSouthEast(y, x, seats))
                numberOfOccupiedSeats++;

            if (CheckSouth(y, x, seats))
                numberOfOccupiedSeats++;

            if (CheckSouthWest(y, x, seats))
                numberOfOccupiedSeats++;

            if (CheckWest(y, x, seats))
                numberOfOccupiedSeats++;

            if (CheckNorthWest(y, x, seats))
                numberOfOccupiedSeats++;

            if (CheckNorth(y, x, seats))
                numberOfOccupiedSeats++;

            if (CheckNorthEast(y, x, seats))
                numberOfOccupiedSeats++;

            return numberOfOccupiedSeats >= 5;

        }

        private static bool AreThereAnyOccupiedSeatsImmediatelyAround(int y, int x, string[] seats)
        {
            // Check the east
            if (x + 1 < seats[y].Length && seats[y][x + 1] == '#')
                return false;

            // Check the south-east
            if (y + 1 < seats.Length && x + 1 < seats[y + 1].Length && seats[y + 1][x + 1] == '#')
                return false;

            // Check the south
            if (y + 1 < seats.Length && seats[y + 1][x] == '#')
                return false;

            // Check the south-west
            if (y + 1 < seats.Length && x - 1 >= 0 && seats[y + 1][x - 1] == '#')
                return false;

            // Check the west
            if (x - 1 >= 0 && seats[y][x - 1] == '#')
                return false;

            // Check the north-west
            if (y - 1 >= 0 && x - 1 >= 0 && seats[y - 1][x - 1] == '#')
                return false;

            // Check the north
            if (y - 1 >= 0 && seats[y - 1][x] == '#')
                return false;

            // Check the north-east
            if (y - 1 >= 0 && x + 1 < seats[y - 1].Length && seats[y - 1][x + 1] == '#')
                return false;

            // No adjacent seats are occupied, this one can be occupied.
            return true;
        }

        private static bool CheckEast(int y, int x, string[] seats)
        {
            var thisX = x + 1;
            while (thisX < seats[y].Length)
            {
                if (seats[y][thisX] == 'L')
                    return false;

                if (seats[y][thisX] == '#')
                    return true;

                thisX++;
            }
            return false;
        }

        private static bool CheckSouthEast(int y, int x, string[] seats)
        {
            var thisY = y + 1;
            var thisX = x + 1;
            while (thisY < seats.Length && thisX < seats[y].Length)
            {
                if (seats[thisY][thisX] == 'L')
                    return false;

                if (seats[thisY][thisX] == '#')
                    return true;
                thisY++;
                thisX++;
            }
            return false;
        }

        private static bool CheckSouth(int y, int x, string[] seats)
        {
            var thisY = y + 1;
            while (thisY < seats.Length)
            {
                if (seats[thisY][x] == 'L')
                    return false;

                if (seats[thisY][x] == '#')
                    return true;

                thisY++;
            }

            return false;
        }

        private static bool CheckSouthWest(int y, int x, string[] seats)
        {
            var thisY = y + 1;
            var thisX = x - 1;
            while (thisY < seats.Length && thisX >= 0)
            {
                if (seats[thisY][thisX] == 'L')
                    return false;

                if (seats[thisY][thisX] == '#')
                    return true;
                thisY++;
                thisX--;
            }
            return false;
        }

        private static bool CheckWest(int y, int x, string[] seats)
        {
            var thisX = x - 1;
            while (thisX >= 0)
            {
                if (seats[y][thisX] == 'L')
                    return false;

                if (seats[y][thisX] == '#')
                    return true;

                thisX--;
            }

            return false;
        }

        private static bool CheckNorthWest(int y, int x, string[] seats)
        {
            var thisY = y - 1;
            var thisX = x - 1;
            while (thisY >= 0 && thisX >= 0)
            {
                if (seats[thisY][thisX] == 'L')
                    return false;

                if (seats[thisY][thisX] == '#')
                    return true;
                thisY--;
                thisX--;
            }
            return false;
        }

        private static bool CheckNorth(int y, int x, string[] seats)
        {
            var thisY = y - 1;
            while(thisY >= 0)
            {
                if (seats[thisY][x] == 'L')
                    return false;

                if (seats[thisY][x] == '#')
                    return true;

                thisY--;
            }

            return false;
        }

        private static bool CheckNorthEast(int y, int x, string[] seats)
        {
            var thisY = y - 1;
            var thisX = x + 1;
            
            while(thisY >= 0 && thisX < seats[y].Length)
            {
                if (seats[thisY][thisX] == 'L')
                    return false;

                if (seats[thisY][thisX] == '#')
                    return true;

                thisY--;
                thisX++;
            }

            return false;
        }

        private static bool TheNumberOfOccupiedSeatsImmediatelyAroundAreGreaterThanFour(int y, int x, string[] seats)
        {
            int numOfOccupiedSeats = 0;
            // Check the east
            if (x + 1 < seats[y].Length && seats[y][x + 1] == '#')
                numOfOccupiedSeats++;

            // Check the south-east
            if (y + 1 < seats.Length && x + 1 < seats[y + 1].Length && seats[y + 1][x + 1] == '#')
                numOfOccupiedSeats++;

            // Check the south
            if (y + 1 < seats.Length && seats[y + 1][x] == '#')
                numOfOccupiedSeats++;

            // Check the south-west
            if (y + 1 < seats.Length && x - 1 >= 0 && seats[y + 1][x - 1] == '#')
                numOfOccupiedSeats++;

            // Check the west
            if (x - 1 >= 0 && seats[y][x - 1] == '#')
                numOfOccupiedSeats++;

            // Check the north-west
            if (y - 1 >= 0 && x - 1 >= 0 && seats[y - 1][x - 1] == '#')
                numOfOccupiedSeats++;

            // Check the north
            if (y - 1 >= 0 && seats[y - 1][x] == '#')
                numOfOccupiedSeats++;

            // Check the north-east
            if (y - 1 >= 0 && x + 1 < seats[y - 1].Length && seats[y - 1][x + 1] == '#')
                numOfOccupiedSeats++;

            return numOfOccupiedSeats >= 5;
        }

        private static void PrintLayout(string[] layout)
        {
            foreach(string row in layout)
            {
                foreach (char seat in row)
                {
                    Console.Write(seat.ToString());
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
