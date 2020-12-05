using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;

namespace AdventOfCode2020.Day05
{
    public class Problem
    {
        public static void Solve()
        {
            string[] lines = InputParser.GetLines("dayfive.txt");
            string[] test = new string[] { "FBFBBFFRLR", "BFFFBBFRRR", "FFFBBBFRRR", "BBFFBBFRLL" };
            //var seatId = GetHighestSeatId(lines);
            //Console.WriteLine(seatId);
            PrintListOfMissingSeats(lines);
        }

        private static int RowMax = 127;
        private static int RowMin = 0;
        private static int ColMax = 7;
        private static int ColMin = 0;

        private static int GetHighestSeatId(string[] lines)
        {
            int highestSeatId = 0;
            foreach(string line in lines)
            {
                var rowInput = line.Substring(0, line.Length - 3);
                var colInput = line.Substring(line.Length - 3);
                var rowNumber = GetRowNumber(rowInput);
                var colNumber = GetColumnNumber(colInput);

                var seatId = rowNumber * 8 + colNumber;

                if (seatId > highestSeatId)
                    highestSeatId = seatId;
            }
            return highestSeatId;
        }

        private static void PrintListOfMissingSeats(string[] lines)
        {
            List<int> seats = new List<int>();

            foreach (string line in lines)
            {
                var rowInput = line.Substring(0, line.Length - 3);
                var colInput = line.Substring(line.Length - 3);
                var rowNumber = GetRowNumber(rowInput);
                var colNumber = GetColumnNumber(colInput);

                var seatId = rowNumber * 8 + colNumber;
                seats.Add(seatId);
            }

            seats.Sort();

            List<int> missingSeats = new List<int>();
            
            for (int i = 0; i < seats.Count - 1; i++)
            {
                if (seats[i + 1] - seats[i] > 1)
                    missingSeats.Add(seats[i] + 1);
            }

            foreach (int seat in missingSeats)
                Console.WriteLine(seat);
        }

        private static int GetRowNumber(string input)
        {
            int rowNumber = GetSeatNumber(input, RowMax, RowMin, 'B', 'F');
            return rowNumber;
        }

        private static int GetColumnNumber(string input)
        {
            int colNumber = GetSeatNumber(input, ColMax, ColMin, 'R', 'L');
            return colNumber;
        }

        private static int GetSeatNumber(string input, int max, int min, char upper, char lower)
        {
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                int halfRoundedUp = (int)Math.Ceiling((double)(max - min) / 2);

                if (c == lower)
                    max -= halfRoundedUp;

                if (c == upper)
                    min += halfRoundedUp;
            }

            int rowNumber = input[^1] == lower ? min : max;

            return rowNumber;
        }
    }
}
