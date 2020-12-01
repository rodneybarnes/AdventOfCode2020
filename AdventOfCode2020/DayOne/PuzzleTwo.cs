using System;
using System.Linq;

namespace AdventOfCode2020.DayOne
{
    public class PuzzleTwo
    {
        public static void Solve()
        {
            var entries = InputParser.GetEntries();
            ParseEntries(entries);
        }

        private static void ParseEntries(int[] entries)
        {
            for (int i = 0; i < entries.Count(); i++)
            {
                for (int j = i + 1; j < entries.Count(); j++)
                {
                    for (int k = j + 1; k < entries.Count(); k++)
                    {
                        if (entries[i] + entries[j] + entries[k] == 2020)
                        {
                            var multiplied = entries[i] * entries[j] * entries[k];
                            Console.WriteLine($"Found the entries! They are {entries[i]}, {entries[j]} and {entries[k]}, and their value when multiplied is {multiplied}");
                            return;
                        }
                    }
                    
                }
            }
        }
    }
}
