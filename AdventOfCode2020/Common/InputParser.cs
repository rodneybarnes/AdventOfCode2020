using System.IO;
using System.Linq;

namespace AdventOfCode2020.Common
{
    public class InputParser
    {
        public static string[] GetLines(string fileName) =>
            File.ReadAllLines(Path.Combine("Resources", fileName)).ToArray();
    }
}
