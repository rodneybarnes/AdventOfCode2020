using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day08
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("dayeight.txt");
            var test = InputParser.GetLines("dayeight-test.txt");
            RunUntilItDoesNotLoop(actual);
        }

        private static void RunUntilItDoesNotLoop(string[] instructions)
        {
            // Initialize static variables
            FindAccValueBeforeLoop(0, instructions, null, null);

            var jumpsAndNops = new Dictionary<int, string>();
            foreach (var entry in ExecutedInstructionLines)
            {
                if (entry.Value.Contains("jmp") || entry.Value.Contains("nop"))
                {
                    jumpsAndNops.Add(entry.Key, entry.Value);
                }
            }

            foreach (var entry in jumpsAndNops)
            {
                if (StopRunning)
                    return;

                ExecutedInstructionLines.Clear();
                Acc = 0;
                var changeOperation = "";
                if (entry.Value.Contains("jmp"))
                    changeOperation = "nop";
                if (entry.Value.Contains("nop"))
                    changeOperation = "jmp";
                FindAccValueBeforeLoop(0, instructions, entry.Key, changeOperation);
            }
        }

        private static Dictionary<int, string> ExecutedInstructionLines = new Dictionary<int, string>();
        private static int Acc = 0;
        private static bool StopRunning = false;

        private static void FindAccValueBeforeLoop(int lineNumber, string[] instructions, int? changeLineNumber, string changeOperation)
        {
            if (lineNumber >= instructions.Length)
            {
                Console.WriteLine("Attempted to run instruction below last line.");
                Console.WriteLine($"Acc value at this point: {Acc}");
                StopRunning = true;
                return;
            }
            if (ExecutedInstructionLines.Keys.Contains(lineNumber))
            {
                return;
            }

            ExecutedInstructionLines.Add(lineNumber, instructions[lineNumber]);

            var instruction = instructions[lineNumber].Split(" ");

            var operation = instruction[0];
            var argument = int.Parse(instruction[1]);

            if (changeLineNumber != null && lineNumber == changeLineNumber)
                operation = changeOperation;

            switch (operation)
            {
                case "nop":
                    lineNumber++;
                    FindAccValueBeforeLoop(lineNumber, instructions, changeLineNumber, changeOperation);
                    break;
                case "acc":
                    Acc += argument;
                    lineNumber++;
                    FindAccValueBeforeLoop(lineNumber, instructions, changeLineNumber, changeOperation);
                    break;
                case "jmp":
                    lineNumber += argument;
                    FindAccValueBeforeLoop(lineNumber, instructions, changeLineNumber, changeOperation);
                    break;
                default:
                    break;
            }
        }
    }
}
