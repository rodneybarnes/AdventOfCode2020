using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day10
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("dayten.txt");
            var smallTest = InputParser.GetLines("dayten-test-1.txt");
            var largerTest = InputParser.GetLines("dayten-test-2.txt");
            //var result = TryAllAdapters(actual, 3);
            var result = FindCountOfDistinctArrangements(actual, 3);
            Console.WriteLine(result);
        }

        private static int TryAllAdapters(string[] input, int difference)
        {
            IEnumerable<int> adapters = input.Select(l => int.Parse(l)).OrderBy(num => num).ToList();

            int numOfDiffOfOne = 0;
            int numOfDiffOfThree = 0;
            int currentOutPut = 0;

            for (int i = 0; i < adapters.Count(); i++)
            {
                int currentDifference = adapters.ElementAt(i) - currentOutPut;

                if (currentDifference > difference)
                {
                    Console.WriteLine($"Current difference was found to be greater than the allowed tolerance. Current: {currentDifference}. Allowed: {difference}");
                    return 0;
                }

                if (currentDifference == 1)
                    numOfDiffOfOne++;

                if (currentDifference == 3)
                    numOfDiffOfThree++;

                currentOutPut = adapters.ElementAt(i);
            }
            numOfDiffOfThree++; // Because the device always adds 3.


            return numOfDiffOfOne * numOfDiffOfThree;
        }

        private static double FindCountOfDistinctArrangements(string[] input, int difference)
        {
            IEnumerable<int> adapters = input.Select(l => int.Parse(l)).OrderBy(num => num).ToList();

            List<int> currentBlock = new List<int>();
            List<List<int>> mutableBlocks = new List<List<int>>();

            for (int i = 0; i < adapters.Count() - 1; i++)
            {
                if ((i + 1 < adapters.Count() && adapters.ElementAt(i + 1) - adapters.ElementAt(i) == 3) ||
                    (i - 1 >= 0 && adapters.ElementAt(i) - adapters.ElementAt(i - 1) == 3))
                {
                    if (currentBlock.Any())
                    {
                        mutableBlocks.Add(currentBlock.ToList());
                        currentBlock.Clear();
                    }
                        
                    continue;
                }

                currentBlock.Add(adapters.ElementAt(i));
            }

            if (currentBlock.Any())
            {
                mutableBlocks.Add(currentBlock.ToList());
                currentBlock.Clear();
            }

            List<int> calculations = new List<int>();

            foreach (List<int> block in mutableBlocks)
            {
                int result = 1;
                if (block.Count() >= 3)
                {
                    result = 7;
                }
                else
                {
                    result = (int)Math.Pow(2, block.Count());
                }
                calculations.Add(result);
            }

            double mult = 1; // because this number got too large for a single int

            foreach (double calc in calculations)
            {
                mult *= calc;
            }

            return mult;
        }
    }
}
