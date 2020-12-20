using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day18
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("dayeighteen.txt");
            var test = InputParser.GetLines("dayeighteen-test.txt");
            //CalculateAll(test);
            AnotherSolution.SolveB(actual);
        }

        private static void CalculateAll(string[] input)
        {
            List<string> results = new List<string>();

            foreach(string line in input)
            {
                results.Add(CalculateLine(line));
            }

            long sum = results.Aggregate(0, (prev, next) => prev + int.Parse(next));
            Console.WriteLine(sum);
        }

        private static string CalculateLine(string line)
        {
            Queue<string> values = new Queue<string>();

            string[] parsed = line.Split(' ');

            foreach (string value in parsed)
            {
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                if (value.Trim().Equals("*") || value.Trim().Equals("+"))
                    values.Enqueue(value.Trim());
                else if (value.First() == ' ')
                {
                    var split = value.Split(' ');

                    foreach (string v in split)
                    {
                        if (string.IsNullOrEmpty(v))
                            continue;
                        values.Enqueue(v);
                    }
                }
                else if (value.Last() == ' ')
                {
                    var split = value.Split(' ');

                    foreach (string v in split)
                    {
                        if (string.IsNullOrEmpty(v))
                            continue;
                        values.Enqueue(v);
                    }
                }
                else 
                    values.Enqueue(CalculateChunk(value));

            }
            Console.WriteLine("break here");
            return CalculateQueue(values);
        }

        private static string CalculateChunk(string chunk)
        {
            Queue<string> values = new Queue<string>();
            string[] split = chunk.Split(' ');

            foreach (string value in split)
            {
                values.Enqueue(value);
            }

            return CalculateQueue(values);
        }

        private static string CalculateQueue(Queue<string> values)
        {
            long result = 0;

            long currentNumber = 0;
            while (values.Any())
            {
                string value = values.Dequeue();
                if (long.TryParse(value, out long converted))
                    currentNumber = converted;
                else
                {
                    long nextNumber = long.Parse(values.Dequeue().ToString());
                    switch (value)
                    {
                        case "*":
                            result = currentNumber * nextNumber;
                            currentNumber = result;
                            break;
                        case "+":
                            result = currentNumber + nextNumber;
                            currentNumber = result;
                            break;
                    }
                }
            }

            return result.ToString();
        }
    }
}
