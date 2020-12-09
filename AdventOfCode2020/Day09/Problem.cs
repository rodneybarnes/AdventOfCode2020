using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day09
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("daynine.txt");
            var test = InputParser.GetLines("daynine-test.txt");

            var invalidNumber = FindInvalidNumber(actual.ToList(), 25);
            if (invalidNumber != null)
            {
                var weakness = FindWeakness(actual, invalidNumber.GetValueOrDefault());
                if (weakness != null)
                    Console.WriteLine(weakness);
                else
                    Console.WriteLine("Could not find weakness");
            }
        }

        private static int? FindInvalidNumber(IEnumerable<string> input, int preambleLength)
        {
            for (int i = preambleLength; i < input.Count(); i++)
            {
                List<string> preamble = new List<string>();
                preamble.AddRange(input.Skip(i - preambleLength).Take(preambleLength));
                IEnumerable<int> sums = GetSumsInPreamble(preamble);

                int currentValue = int.Parse(input.ElementAt(i));

                if (sums.Any(s => s.Equals(currentValue)) is false)
                    return currentValue;
            }

            return null;
        }

        private static IEnumerable<int> GetSumsInPreamble(IEnumerable<string> preamble)
        {
            List<int> sums = new List<int>();
            for (int i = 0; i < preamble.Count(); i++)
            {
                for (int j = i + 1; j < preamble.Count(); j++)
                {
                    var sum = int.Parse(preamble.ElementAt(i)) + int.Parse(preamble.ElementAt(j));
                    sums.Add(sum);
                }
            }

            return sums.Distinct();
        }

        private static int? FindWeakness(IEnumerable<string> input, int invalidNumber)
        {
            List<int> set = new List<int>();
            for (int i = 0; i < input.Count(); i++)
            {
                set.Add(int.Parse(input.ElementAt(i)));
                for (int j = i + 1; j < input.Count(); j++)
                {
                    set.Add(int.Parse(input.ElementAt(j)));
                    var sumOfSet = set.Aggregate(0, (prev, next) => prev + next);
                    if (sumOfSet.Equals(invalidNumber))
                    {
                        var smallestNumber = set.Min();
                        var largestNumber = set.Max();
                        return smallestNumber + largestNumber;
                    }

                    if (sumOfSet > invalidNumber)
                    {
                        set.Clear();
                        break;
                    }
                }
            }
            return null;
        }
    }
}
