using System;

namespace AdventOfCode2020.DayTwo
{
    public class PuzzleOneAndTwo
    {
        public static void Solve()
        {
            var lines = InputParser.GetLines();
            FindNumberOfCorrectPasswords(lines);
        }

        private static void FindNumberOfCorrectPasswords(string[] lines)
        {
            var numberCorrect = 0;

            foreach (string line in lines)
            {
                char[] delimiterChars = {'-', ' ', ':' };
                string[] sections = line.Split(delimiterChars);
                int.TryParse(sections[0], out int firstPosition);
                int.TryParse(sections[1], out int secondPosition);
                var requiredLetter = sections[2];
                var password = sections[4];

                var letterAtFirstPosition = password[firstPosition - 1].ToString();
                var letterAtSecondPosition = password[secondPosition - 1].ToString();

                if (letterAtFirstPosition.Equals(requiredLetter) && letterAtSecondPosition.Equals(requiredLetter))
                    continue;

                if (letterAtFirstPosition.Equals(requiredLetter) || letterAtSecondPosition.Equals(requiredLetter))
                    numberCorrect++;

                //var numberOfTimesRequiredLetterAppearsInPassword = password.Count(letter => letter.ToString() == requiredLetter);

                //if (numberOfTimesRequiredLetterAppearsInPassword >= lowerRange &&
                //    numberOfTimesRequiredLetterAppearsInPassword <= upperRange)
                //    numberCorrect++;
            }

            Console.WriteLine($"Number of correct passwords: {numberCorrect}");
        }
    }
}
