using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day16
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("daysixteen.txt");
            var test = InputParser.GetLines("daysixteen-test.txt");
            var test2 = InputParser.GetLines("daysixteen-test-two.txt");
            var data = ParseLines(actual);
            
            PrintSumOfInvalidNumbers(data);
        }

        private class Field
        {
            public string Name { get; set; }
            public KeyValuePair<int, int> FirstRange { get; set; }
            public KeyValuePair<int, int> SecondRange { get; set; }
            public List<int> ValidIndexes { get; set; } = new List<int>();

            public bool IsInRange(int value)
            {
                return value >= FirstRange.Key && value <= FirstRange.Value ||
                    value >= SecondRange.Key && value <= SecondRange.Value;
            }

        }

        private static void PrintSumOfInvalidNumbers(List<List<string>> data)
        {
            List<Field> fields = ParseFields(data[0]);
            List<int> myTicket = ParseMyTicket(data[1]);
            List<List<int>> nearbyTickets = ParseNearbyTickets(data[2]);

            List<List<int>> validTickets = new List<List<int>>();

            for (int i = 0; i < nearbyTickets.Count(); i++)
            {
                var ticket = nearbyTickets[i];
                bool allValid = true;
                foreach (int value in ticket)
                {
                    bool isValid = false;
                    foreach(Field field in fields)
                    {
                        if (field.IsInRange(value))
                        {
                            isValid = true;
                            break;
                        }

                    }

                    if (isValid is false)
                    {
                        allValid = false;
                        //invalidTickets.Add(value);
                        break;
                    }
                }

                if (allValid)
                {
                    validTickets.Add(nearbyTickets.ElementAt(i));
                }
            }

            //var sum = invalidTickets.Aggregate(0, (prev, next) => prev + next);

            validTickets.Add(myTicket);

            foreach (Field field in fields)
            {
                Dictionary<int, bool> validFieldIndexes = new Dictionary<int, bool>();
                for (int i = 0; i < myTicket.Count(); i++)
                    validFieldIndexes[i] = true;

                foreach (List<int> ticket in validTickets)
                {
                    for (int i = 0; i < ticket.Count(); i++)
                    {
                        var value = ticket[i];

                        if (!field.IsInRange(value))
                        {
                            validFieldIndexes[i] = false;
                        }
                    }
                }

                var validIndexes = validFieldIndexes.Where(i => i.Value == true).Select(i => i.Key);
                field.ValidIndexes.AddRange(validIndexes);
            }

            var orderedFields = fields.OrderBy(f => f.ValidIndexes.Count()).ToList();

            for (int i = 0; i < fields.Count(); i++)
            {
                if (orderedFields[i].ValidIndexes.Count() == 1)
                {
                    var indexValue = orderedFields[i].ValidIndexes.First();
                    for (int j = i + 1; j < fields.Count(); j++)
                    {
                        orderedFields[j].ValidIndexes.RemoveAll(i => i.Equals(indexValue));
                    }
                }
            }

            List<int> selectedIndexes = orderedFields.Where(f => f.Name.Contains("departure")).Select(f => f.ValidIndexes.First()).ToList();

            long answer = 1;

            foreach (int index in selectedIndexes)
            {
                answer *= myTicket[index];
            }

            Console.WriteLine(answer);

        }

        private static List<Field> ParseFields(List<string> data)
        {
            List<Field> fields = new List<Field>();
            foreach (string line in data)
            {
                var sections = line.Split(':');
                var fieldName = sections[0].Trim();

                Field field = new Field { Name = fieldName };


                var numbers = sections[1].Trim().Split("or");
                var firstRange = numbers[0].Trim().Split('-');
                var secondRange = numbers[1].Trim().Split('-');

                field.FirstRange = new KeyValuePair<int, int>(int.Parse(firstRange[0].Trim()), int.Parse(firstRange[1].Trim()));
                field.SecondRange = new KeyValuePair<int, int>(int.Parse(secondRange[0].Trim()), int.Parse(secondRange[1].Trim()));

                fields.Add(field);

            }
            return fields;
        }

        private static List<List<int>> ParseNearbyTickets(List<string> data)
        {
            List<List<int>> nearbyTickets = new List<List<int>>();

            data.RemoveAt(0);

            foreach (string line in data)
            {
                List<int> thisTicket = new List<int>();
                string[] fieldValues = line.Split(',');
                foreach (string value in fieldValues)
                {
                    thisTicket.Add(int.Parse(value));
                }

                nearbyTickets.Add(thisTicket);
            }

            return nearbyTickets;
        }

        private static List<int> ParseMyTicket(List<string> data)
        {
            List<int> numbers = new List<int>();

            var stringNumbers = data[1].Split(',');
            foreach (string stringNumber in stringNumbers)
            {
                numbers.Add(int.Parse(stringNumber));
            }

            return numbers;
        }

        private static List<List<string>> ParseLines(string[] input)
        {
            List<List<string>> groups = new List<List<string>>();

            List<string> tempList = new List<string>();

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    groups.Add(tempList);
                    tempList = new List<string>();
                    continue;
                }

                tempList.Add(line);
            }

            if (tempList.Any())
                groups.Add(tempList);

            return groups;
        }
    }
}
