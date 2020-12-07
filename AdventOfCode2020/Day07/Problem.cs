using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using static AdventOfCode2020.Day07.Bag;

namespace AdventOfCode2020.Day07
{
    public class Problem
    {
        public static void Solve()
        {
            var test = InputParser.GetLines("dayseven-test.txt");
            var actual = InputParser.GetLines("dayseven.txt");
            //var count = FindValidBags("shiny gold", actual);
            GetSumOfContainedBags("shiny gold", actual);
            Console.WriteLine(Counter);
        }

        private static IEnumerable<Bag> ParseLinesIntoBags(string[] lines)
        {
            List<Bag> allBags = new List<Bag>();

            foreach (string rule in lines)
            {
                var split = rule.Split("contain");
                var bagType = split[0];
                var bagRules = split[1].Split(", ");

                var bag = new Bag(bagType);
                foreach (string bagRule in bagRules)
                {
                    bag.AddRule(bagRule);
                }
                allBags.Add(bag);
            }
            return allBags;
        }

        private static void GetSumOfContainedBags(string colour, string[] rules)
        {
            IEnumerable<Bag> allBags = ParseLinesIntoBags(rules);
            Bag outerBag = allBags.FirstOrDefault(b => b.BagType.Equals(colour));
            GetContainedBagsForThisBag(outerBag, allBags);
        }

        // I know how to do recursion properly, I swear, but I just needed to get the answer and this was the first solution I came up with that worked.
        private static int Counter = 0;

        private static void GetContainedBagsForThisBag(Bag outerBag, IEnumerable<Bag> allBags)
        {
            foreach (BagRule rule in outerBag.Rules)
            {
                Bag innerBag = allBags.FirstOrDefault(b => b.BagType.Equals(rule.BagType));
                for (int i = 0; i < rule.NumberOfBags; i ++)
                {
                    Counter++;
                    GetContainedBagsForThisBag(innerBag, allBags);
                }
            }
        }

        private static int FindValidBags(string colour, string[] rules)
        {
            IEnumerable<Bag> allBags = ParseLinesIntoBags(rules);

            List<Bag> bagCollection = new List<Bag>();
            FindAllBagsThatCanContainThisBag(colour, allBags, bagCollection);

            return bagCollection.Select(b => b.BagType).Distinct().Count();
        }

        private static List<Bag> FindAllBagsThatCanContainThisBag(string bagType, IEnumerable<Bag> allBags, List<Bag> bagCollection)
        {
            foreach (Bag bag in allBags)
            {
                var type = bag.BagType;
                foreach (BagRule rule in bag.Rules)
                {
                    if (rule.BagType.Contains(bagType))
                    {
                        bagCollection.Add(bag);
                        FindAllBagsThatCanContainThisBag(bag.BagType, allBags, bagCollection);
                        break;
                    }
                }
            }
            return bagCollection;
        }
    }
}
