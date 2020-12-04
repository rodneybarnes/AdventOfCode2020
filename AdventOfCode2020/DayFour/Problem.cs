using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.DayFour
{
    public class Problem
    {
        public static void Solve()
        {
            var test = InputParser.GetLines("dayfour-test.txt");
            var testValidation = InputParser.GetLines("dayfour-validation-test.txt");
            var actual = InputParser.GetLines("dayfour.txt");
            var numberOfValidPassports = ParsePassports(actual);
            Console.WriteLine(numberOfValidPassports);
        }

        private static int ParsePassports(string[] lines)
        {
            List<string> passportLines = new List<string>();
            int numberOfValidPassports = 0;

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    var passport = Passport.ParseLinesIntoPassport(passportLines);
                    if (passport.IsValid)
                        numberOfValidPassports++;

                    passportLines.Clear();
                    continue;
                }

                passportLines.Add(line);
            }

            if (passportLines.Any())
            {
                var passport = Passport.ParseLinesIntoPassport(passportLines);
                if (passport.IsValid)
                    numberOfValidPassports++;

                passportLines.Clear();
            }

            return numberOfValidPassports;
        }
    }
}
