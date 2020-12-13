using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Day12
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("daytwelve.txt");
            var test = InputParser.GetLines("daytwelve-test.txt");
            RunInstructions(actual);
        }

        private static void RunInstructions(string[] instructions)
        {
            var ship = new Ship();

            foreach (string instruction in instructions)
            {
                ship.NavigateWithCorrectInstructions(instruction);
            }

            var md = ship.ManhattanDistance;

            Console.WriteLine(ship.ManhattanDistance);
        }
    }


}
