using AdventOfCode2020.Common;
using System;

namespace AdventOfCode2020.Day03
{
    public class Problem
    {
        public static void Solve()
        {
            var lines = InputParser.GetLines("daythree.txt");
            var testLines = InputParser.GetLines("daythree-test.txt");
            var allTreesHit = TraverseMultipleTimes(lines);
            Console.WriteLine(allTreesHit);
        }

        private static int TraverseMultipleTimes(string[] lines)
        {
            var firstRun = TraverseSlope(lines, 1, 1);
            var secondRun = TraverseSlope(lines, 3, 1);
            var thirdRun = TraverseSlope(lines, 5, 1);
            var fourthRun = TraverseSlope(lines, 7, 1);
            var fifthRun = TraverseSlope(lines, 1, 2);
            return firstRun * secondRun * thirdRun * fourthRun * fifthRun;
        }

        private static int TraverseSlope(string[] lines, int right, int down)
        {
            var currentRight = right;
            var numberOfTreesHit = 0;

            for (int y = down; y < lines.Length; y += down)
            {
                if (lines[y][currentRight] == '#')
                    numberOfTreesHit++;

                currentRight = (currentRight + right) % lines[y].Length;
            }

            return numberOfTreesHit;
        }
    }
}
