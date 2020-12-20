using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day19
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("daynineteen.txt");
            var test = InputParser.GetLines("daynineteen-test.txt");
            //ParseRules(test);
            var sol = AnotherSolution.Solve(actual);
            Console.WriteLine(sol);
        }

        class Rule
        {
            public int Key { get; set; }
            public bool IsBaseRule { get; set; } = false;
            public string SubRules { get; set; }
            public List<string> Combinations { get; set; } = new List<string>();

            public Rule(int key, string subRules)
            {
                Key = key;

                if (subRules.Contains("a") || subRules.Contains("b"))
                    subRules = subRules.Replace("\"", string.Empty);

                SubRules = subRules;
            }
            

            public void BuildCombinations(Dictionary<int, Rule> allRules)
            {
                if (Combinations.Any())
                    return;

                if (SubRules.Contains("a") || SubRules.Contains("b"))
                {
                    IsBaseRule = true;
                    Combinations.Add(SubRules);
                    return;
                }

                if (SubRules.Contains('|'))
                {
                    string[] combos = SubRules.Split('|');
                    foreach (string combo in combos)
                    {
                        BuildCombo(combo, allRules);
                    }
                }
                else
                {
                    BuildCombo(SubRules, allRules);
                }
            }

            private void BuildCombo(string combo, Dictionary<int, Rule> allRules)
            {
                string[] indexes = combo.Trim().Split(' ');

                if (indexes.Length == 1)
                {
                    var rule = allRules[int.Parse(indexes.First())];
                    rule.BuildCombinations(allRules);
                    foreach (string ruleCombo in rule.Combinations)
                    {
                        Combinations.Add(ruleCombo);
                    }
                    return;
                }

                var firstRule = allRules[int.Parse(indexes[0])];
                var secondRule = allRules[int.Parse(indexes[1])];

                firstRule.BuildCombinations(allRules);
                secondRule.BuildCombinations(allRules);

                foreach (string firstRuleCombo in firstRule.Combinations)
                {
                    foreach(string secondRuleCombo in secondRule.Combinations)
                    {
                        Combinations.Add($"{firstRuleCombo}{secondRuleCombo}");
                    }
                }
            }
        }

        private static void ParseRules(string[] input)
        {
            Dictionary<int, Rule> allRules = new Dictionary<int, Rule>();
            List<string> receivedMessages = new List<string>();
            List<string> validMessages = new List<string>();

            bool isRulesSection = true;

            foreach(string line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    isRulesSection = false;
                    continue;
                }

                if (isRulesSection)
                {
                    var split = line.Split(':');
                    allRules.Add(int.Parse(split[0]), new Rule(int.Parse(split[0]), split[1].Trim()));
                }
                else
                {
                    receivedMessages.Add(line);
                }
            }

            allRules[0].BuildCombinations(allRules);

            foreach (string message in receivedMessages)
            {
                if (allRules[0].Combinations.Any(c => c.Equals(message)))
                    validMessages.Add(message);
            }


            Console.WriteLine(validMessages.Count());
        }
    }
}
