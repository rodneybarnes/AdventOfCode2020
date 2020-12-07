using System.Collections.Generic;

namespace AdventOfCode2020.Day07
{
    public class Bag
    {
        public string BagType { get; set; }
        public IEnumerable<BagRule> Rules => _rules;

        private List<BagRule> _rules = new List<BagRule>();

        public Bag(string bagType)
        {
            bagType = bagType
                .Replace("bags", string.Empty)
                .Replace("bag", string.Empty)
                .Trim();
            BagType = bagType;
        }

        public void AddRule(string rule)
        {
            var bagRule = BagRule.CreateRule(rule);

            if (bagRule != null)
                _rules.Add(bagRule);
        }

        public class BagRule
        {
            public int NumberOfBags { get; set; } = 0;
            public string BagType { get; set; }

            public static BagRule CreateRule(string rule)
            {
                rule = rule
                    .Replace("bags", string.Empty)
                    .Replace("bag", string.Empty)
                    .Replace(".", string.Empty)
                    .Trim();

                if (int.TryParse(rule.Substring(0, 1), out int numberOfContainedBags))
                {
                    return new BagRule
                    {
                        NumberOfBags = numberOfContainedBags,
                        BagType = rule.Substring(1).Trim()
                    };
                }
                return null;
            }
        }
    }

}
