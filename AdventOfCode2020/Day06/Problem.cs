using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day06
{
    public class Problem
    {
        public static void Solve()
        {
            var lines = InputParser.GetLines("daysix.txt");
            var test = InputParser.GetLines("daysix-test.txt");

            int sum = GetSumOfGroupCounts(lines);
            Console.WriteLine(sum);
        }

        private static int GetSumOfGroupCounts(string[] lines)
        {
            List<string> group = new List<string>();
            int sum = 0;

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    //sum += GetGroupCountOfDistinctAnswers(group);
                    sum += GetGroupCountOfOnlyCommonAnswers(group);
                    group.Clear();
                    continue;
                }

                group.Add(line);
            }

            if (group.Any())
            {
                //sum += GetGroupCountOfDistinctAnswers(group);
                sum += GetGroupCountOfOnlyCommonAnswers(group);
            }

            return sum;
        }

        private static int GetGroupCountOfDistinctAnswers(IEnumerable<string> group)
        {
            return string.Concat(group).Distinct().Count();
        }

        private static int GetGroupCountOfOnlyCommonAnswers(IEnumerable<string> group)
        {
            return group.Aggregate((prev, next) => String.Concat(prev.Intersect(next))).Count();
        }
    }
}
