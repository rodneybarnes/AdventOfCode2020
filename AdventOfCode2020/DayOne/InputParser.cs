using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.DayOne
{
    public static class InputParser
    {
        public static int[] GetTestEntries()
        {
            return new int[] { 1721, 979, 366, 299, 675, 1456 };
        }

        public static int[] GetEntries()
        {
            var filePath = Directory.EnumerateFiles($"{Directory.GetCurrentDirectory()}/Resources").FirstOrDefault(f => f.Contains("puzzle-one.txt"));

            List<int> entries = new List<int>();
            using (var sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    bool tryParse = int.TryParse(line, out int entry);
                    if (tryParse)
                    {
                        entries.Add(entry);
                    }
                }
            }
            return entries.ToArray();
        }
    }
}
