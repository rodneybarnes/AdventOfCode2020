using System;
using System.Collections.Generic;

namespace AdventOfCode2020.Day15
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = new int[] { 7, 14, 0, 17, 11, 1, 2 };
            var test1 = new int[] { 0, 3, 6 };
            var test2 = new int[] { 1, 2, 3 };
            var test3 = new int[] { 1, 3, 2 };
            var sol = FindLastSpokenNumber(actual, 30000000);
            Console.WriteLine(sol);
        }

        private static int FindLastSpokenNumber(int[] startingNumbers, int rounds)
        {
            int lastNumberSpoken = 0;
            Dictionary<int, int> lastSpokenDictionary = new Dictionary<int, int>();
            Dictionary<int, int> beforeThatDictionary = new Dictionary<int, int>();

            for (int i = 1; i <= rounds; i++)
            {
                if (i <= startingNumbers.Length)
                {
                    lastNumberSpoken = startingNumbers[i - 1];
                    lastSpokenDictionary[lastNumberSpoken] = i;
                    continue;
                }

                if (beforeThatDictionary.ContainsKey(lastNumberSpoken))
                {
                    lastNumberSpoken = lastSpokenDictionary[lastNumberSpoken] - beforeThatDictionary[lastNumberSpoken];

                    if (lastSpokenDictionary.ContainsKey(lastNumberSpoken))
                    {
                        beforeThatDictionary[lastNumberSpoken] = lastSpokenDictionary[lastNumberSpoken];
                    }

                    lastSpokenDictionary[lastNumberSpoken] = i;
                }
                else
                {
                    // This is the first time the number has been spoken
                    lastNumberSpoken = 0;

                    if (lastSpokenDictionary.ContainsKey(0))
                    {
                        beforeThatDictionary[0] = lastSpokenDictionary[0];
                    }

                    lastSpokenDictionary[0] = i;
                }
            }

            return lastNumberSpoken;
        }
    }
}
