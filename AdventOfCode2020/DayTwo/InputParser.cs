using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.DayTwo
{
    public class InputParser
    {
        public static string[] GetTestLines()
        {
            return new string[] { "1-3 a: abcde", "1-3 b: cdefg", "2-9 c: ccccccccc" };
        }

        public static string[] GetLines()
        {
            var filePath = Directory.EnumerateFiles($"{Directory.GetCurrentDirectory()}/Resources").FirstOrDefault(f => f.Contains("daytwo.txt"));

            List<string> lines = new List<string>();
            using (var sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    lines.Add(sr.ReadLine().Trim());
                }
            }
            return lines.ToArray();
        }
    }
}
