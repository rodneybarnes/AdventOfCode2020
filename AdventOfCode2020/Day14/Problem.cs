using AdventOfCode2020.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Day14
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("dayfourteen.txt");
            var test = InputParser.GetLines("dayfourteen-test.txt");
            RunEmulator(actual);
        }

        private static void RunEmulator(string[] input)
        {
            IEnumerable<InstructionSet> instructions = ParseInstructions(input);
            string[] memory = WriteToMemory(instructions);
            ulong sum = SumAllValuesInMemory(memory);
            Console.WriteLine(sum);
            
        }
        
        private static IEnumerable<InstructionSet> ParseInstructions(string[] input)
        {
            List<InstructionSet> instructions = new List<InstructionSet>();

            List<string> instructionGroup = new List<string>();
            foreach (string line in input)
            {
                if (line.Contains("mask") && instructionGroup.Count() > 0)
                {
                    instructions.Add(InstructionSet.ParseInstructions(instructionGroup.ToArray()));
                    instructionGroup.Clear();
                }

                instructionGroup.Add(line);
            }

            if (instructionGroup.Any())
            {
                instructions.Add(InstructionSet.ParseInstructions(instructionGroup.ToArray()));
                instructionGroup.Clear();
            }

            return instructions;
        }

        private static string[] WriteToMemory(IEnumerable<InstructionSet> instructions)
        {
            List<ulong> allAddresses = new List<ulong>();

            foreach (InstructionSet instructionSet in instructions)
            {
                foreach (Tuple<int, int> instruction in instructionSet.Instructions)
                {
                    // Find a way to add the masked value to each of the addresses
                    IEnumerable<ulong> newAddresses = ApplyMaskToAddress(instructionSet.Mask, instruction.Item1);
                    allAddresses.AddRange(newAddresses);
                }
            }

            List<Tuple<ulong, string>> memory = new List<Tuple<ulong, string>>();

            foreach (InstructionSet instructionSet in instructions)
            {
                foreach (Tuple<int, int> instruction in instructionSet.Instructions)
                {
                    // Find a way to add the masked value to each of the addresses
                    int memoryAddress = instruction.Item1;
                    IEnumerable<ulong> newAddresses = ApplyMaskToAddress(instructionSet.Mask, instruction.Item1);

                    foreach (ulong address in newAddresses)
                    {
                        var value = Convert.ToString(instruction.Item2, 2);
                        memory.RemoveAll(a => a.Item1 == address);

                        memory.Add(new Tuple<ulong, string>(address, value));
                            
                    }
                }
            }

            return memory.Select(a => a.Item2).ToArray();
        }

        private static IEnumerable<ulong> ApplyMaskToAddress(string mask, int memoryAddress)
        {
            List<ulong> newAddresses = new List<ulong>();


            string newValue = "";
            string reversedMask = new string(mask.Reverse().ToArray());
            string binaryValue = new string(Convert.ToString(memoryAddress, 2).Reverse().ToArray());

            for (int i = 0; i < reversedMask.Length; i++)
            {
                if (reversedMask[i] == '0' && i < binaryValue.Length)
                    newValue += binaryValue[i];

                else
                    newValue += reversedMask[i];
            }

            // For every X that is in the newValue, we need to create a new memory address for every possible combination
            var maxIterations = Math.Pow(2, newValue.Count(c => c.Equals('X')));
            var bitArray = newValue.Where(c => c.Equals('X')).Select(c => 0).ToArray();

            for (int i = 0; i < maxIterations; i++)
            {
                int bitIndex = 0;
                string newAddress = "";
                for (int j = 0; j < newValue.Length; j++)
                {
                    if (newValue[j] == 'X')
                    {
                        newAddress += bitArray[bitIndex].ToString();
                        bitIndex++;
                    }
                    else
                        newAddress += newValue[j];
                }

                newAddresses.Add(Convert.ToUInt64(new string(newAddress.Reverse().ToArray()), 2));
                Increment(bitArray);
            }

            return newAddresses;
        }

        private static void Increment(int[] array)
        {
            for (int i = 0; i< array.Length; i++)
            {
                bool previous = array[i] == 1;
                array[i] = previous ? 0 : 1;
                if (!previous)
                {
                    return;
                }
            }
        }

        private static string ApplyMaskToValue(string mask, int decimalValue)
        {
            string newValue = "";
            string reversedMask = new string(mask.Reverse().ToArray());
            string binaryValue = new string(Convert.ToString(decimalValue, 2).Reverse().ToArray());

            for (int i = 0; i < reversedMask.Length; i++)
            {
                if (reversedMask[i] == 'X' && i < binaryValue.Length)
                    newValue += binaryValue[i];
                else if (reversedMask[i] == 'X' && i >= binaryValue.Length)
                    newValue += '0';
                else
                    newValue += reversedMask[i];
            }

            return new string(newValue.Reverse().ToArray());
        }

        private static ulong SumAllValuesInMemory(string[] memory)
        {
            ulong result = 0;

            IEnumerable<string> validAddresses = memory.Where(a => !string.IsNullOrEmpty(a)).ToList();
            foreach (string value in validAddresses)
            {
                result += Convert.ToUInt64(value, 2);
            }
            return result;
        }
    }
}
