using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Day14
{
    public class InstructionSet
    {
        public string Mask { get; set; }

        private List<Tuple<int, int>> _instructions = new List<Tuple<int, int>>();

        /// <summary>
        /// A list of an instruction set's memory address designations and values.
        /// The first int in the Tuple is the memory address that this instruction will attempt to write to,
        /// and the second int is the decimal value.
        /// </summary>
        public IEnumerable<Tuple<int, int>> Instructions => _instructions;

        public void AddInstruction(int memoryAddress, int value)
        {
            _instructions.Add(new Tuple<int, int>(memoryAddress, value));
        }

        public static InstructionSet ParseInstructions(string[] input)
        {
            var instructionSet = new InstructionSet();
            foreach (string line in input)
            {
                var split = line.Split('=');
                var meta = split[0].Trim();
                if (meta == "mask")
                {
                    instructionSet.Mask = split[1].Trim();
                }
                else
                {
                    int firstBracket = meta.IndexOf('[');
                    int lastBracket = meta.IndexOf(']');
                    int memoryAddress = int.Parse(meta.Substring(firstBracket + 1, lastBracket - firstBracket - 1));
                    int value = int.Parse(split[1]);
                    instructionSet.AddInstruction(memoryAddress, value);
                }
            }

            return instructionSet;
        }
    }
}
