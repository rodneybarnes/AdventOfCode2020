using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day18
{
    /// <summary>
    /// From: https://github.com/jbush7401/AdventOfCode/blob/master/AdventOfCode/2020/Day18.cs
    /// </summary>
    public class AnotherSolution
    {
        static Stack<Operation> operations = new Stack<Operation>();
        public static void SolveA(string[] lines)
        {
            long total = 0;
            foreach (string line in lines)
            {
                operations.Push(new Operation());

                foreach (char c in line.Replace(" ", string.Empty))
                {
                    switch (c)
                    {
                        case '+':
                            operations.Peek().operators.Add('+');
                            break;
                        case '*':
                            operations.Peek().operators.Add('*');
                            break;
                        case '(':
                            operations.Push(new Operation());
                            break;
                        case ')':
                            long final = operations.Peek().calculate();
                            operations.Pop();
                            operations.Peek().numbers.Add(final);
                            if (operations.Peek().operators.Count == 0)
                                operations.Peek().operators.Add('+');
                            break;
                        default:
                            operations.Peek().numbers.Add(int.Parse(c.ToString()));
                            if (operations.Peek().operators.Count == 0)
                                operations.Peek().operators.Add('+');
                            break;
                    }
                }

                total += operations.Pop().calculate();
            }

            Console.WriteLine($"Part 1: {total}");
        }

        public static void SolveB(string[] lines)
        {
            operations = null;
            operations = new Stack<Operation>();

            long total = 0;
            foreach (string line in lines)
            {
                operations.Push(new Operation());

                foreach (char c in line.Replace(" ", string.Empty))
                {
                    switch (c)
                    {
                        case '+':
                            operations.Peek().operators.Add('+');
                            break;
                        case '*':
                            operations.Peek().operators.Add('*');
                            break;
                        case '(':
                            operations.Push(new Operation());
                            break;
                        case ')':
                            long final = operations.Peek().calculateNotReallyRealMath();
                            operations.Pop();
                            operations.Peek().numbers.Add(final);
                            if (operations.Peek().operators.Count == 0)
                                operations.Peek().operators.Add('+');
                            break;
                        default:
                            operations.Peek().numbers.Add(int.Parse(c.ToString()));
                            if (operations.Peek().operators.Count == 0)
                                operations.Peek().operators.Add('+');
                            break;
                    }
                }

                total += operations.Pop().calculateNotReallyRealMath();
            }

            Console.WriteLine($"Part 2: {total}");
        }

        class Operation
        {
            public List<long> numbers = new List<long>();
            public List<char> operators = new List<char>();

            public long calculate()
            {
                long total = 0;

                for (int i = 0; i < numbers.Count; i++)
                {
                    if (operators[i] == '+')
                        total += numbers[i];
                    else
                        total *= numbers[i];
                }

                return total;
            }

            public long calculateNotReallyRealMath()
            {
                long total = 1;
                long additionResult = 0;
                List<long> additionsFirstResult = new List<long>();
                for (int i = 0; i < numbers.Count; i++)
                {
                    if (operators[i] == '+')
                        additionResult += numbers[i];
                    else
                    {
                        additionsFirstResult.Add(additionResult);
                        additionResult = 0;
                        additionResult += numbers[i];
                    }
                }

                additionsFirstResult.Add(additionResult);
                foreach (long i in additionsFirstResult)
                    total *= i;

                return total;
            }
        }
    }
}
