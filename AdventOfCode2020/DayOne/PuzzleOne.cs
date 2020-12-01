using System;
using System.Linq;

namespace AdventOfCode2020.DayOne
{
    public class PuzzleOne
    {
        public static void Solve()
        {
            int[] entries = InputParser.GetEntries();
            ParseEntries(entries);
        }

        /// <summary>
        /// A brute-force means of parsing an array of integers to find which two sum to 2020, then multplies them and outputs that 
        /// value to the console.
        /// </summary>
        /// <param name="entries">The entries to parse.</param>
        private static void ParseEntries(int[] entries)
        {
            for (int i = 0; i < entries.Count(); i++)
            {
                for (int j = i + 1; j < entries.Count(); j++)
                {
                    if (entries[i] + entries[j] == 2020)
                    {
                        var multiplied = entries[i] * entries[j];
                        Console.WriteLine($"Found the entries! They are {entries[i]} and {entries[j]}, and their value when multiplied is {multiplied}");
                        return;
                    }
                }
            }
        }
    }
}
