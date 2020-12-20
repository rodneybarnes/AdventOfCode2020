using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day19
{
    /// <summary>
    /// I'm fucking terrible at Regex, so this solution is from 
    /// https://topaz.github.io/paste/#XQAAAQBSCAAAAAAAAAA4HUhobBw1MDjqJOxYffNgSDcy5WPGZLnH/A0c7JCxHK/U//qn3jKACDLxy0i9C+rmJsG2oPBpkEdxwyvQnXzaW2ffo2PlgPsTZK0T1zeofoTH+7RiN2GIEWiwvKaclMKOnE+1tHhDMuN4xcNfsr/Gm1IrKI0Rx1+c1fYGBm+YC+LOChrnAPmTAuQowYbdvSV5uJ4lxNetzJECXv2bwMWHuv5LCQ721SLiq6IJGLA7NjDZGmtMD4onQC76SbS9RfZqdJ1s/fGpLS+fbB+1fYbQAHLhiOStGlxVyMxl4zjiq03oUtyOz7n38BAXmanGSAPYFXolued25TTzOB/QL6qbBGouGjh3IhwkUR3yUbIGzxTlRefLY+PxzxQmaOmutu/0Z/NH0r9MY/WmIpMnktEFgihRy2KBBOOpgQFi2eAZ8lrSDeLPHjh0glLOEwtFLuyDztoPuyOUXjP4syeonmVupmZFtjyVvaRM9pjNR4IrmbZhBb8QFj9mpNoA5Wcx72j8A+KJNQE2F/fHaZmDkb/aQ/wDegiVlvVLQ3vZUrHONdWDJQA39lTS7GOBhkdJRMNeJUEHUHAIgKty0cZnKraJb1AKmuECkaRFeDVqOD9hqcWx+pGgXre/u2qajiGKO7nC+hMmqcJj5rlCO1KRF0w8CTRGCgta12muNttQzQ1bNYUyd/2X/FmpxxeVjkWvzW7Vph+ISEPrHNizMKrWYn0+/kDssbpXTuk4ugStpSeXeU2TamQaj3NHC1w0NGSI4PU8SF/Q3eOqk9cdlUCqjLP7Or2R/wayjk9TbFYU5riqOGVFL+oec1MSDzDbI60cWbC+07LZ9aJ+PMbrJ0S1OGhxNd7AeFWjw4uRYCKCJKht6A41cZwQGruskAnBwqikcTSuBchkAeqgfiiUQJXXdXBy0m72YArCwNC/LVZ5Eav0FInOapMTXPajANSp/Z9VI+HcgNbf/LFWIrkUmjY5PxAlRRjJnSJtF7HfSXtsg+HBKRO2I6odLwplrLZJBLLmed0lW1ya6/AmeREUKn7khoJiBWnVfGMqlj+zCjtm3tPthS6Ue2DoD97rmW+aw/X93695fp99+RpQNJiPrIK/m8DV4vNQKYgCRuCH2rFhif1j8tE=
    /// </summary>
    public class AnotherSolution
    {
        public static int Solve(string[] input)
        {
            var (rules, messages) = Parse(input);
            rules[8] = "c";
            rules[11] = "d";

            SimplifyRules(rules);

            var rule0 = rules[0];
            // Rule8  ==> (Rule42)+  [one or more]
            // Rule11 ==>  Rule42{k}Rule31{k} where k >= 1 [balanced group]
            rule0 = rule0
                .Replace("c", $"(?:{rules[42]})+")
                .Replace("d", $"(?<DEPTH>{rules[42]})+(?<-DEPTH>{rules[31]})+(?(DEPTH)(?!))");

            var rule0Regex = new Regex($"^{rule0}$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
            return messages.Count(rule0Regex.IsMatch);
        }

        private static void SimplifyRules(IDictionary<int, string> dict)
        {
            var done = new HashSet<int>();
            foreach (var (key, val) in dict)
                if (val.Length == 1)
                    done.Add(key);

            while (done.Count != dict.Count)
            {
                foreach (var (key, val) in dict)
                {
                    if (done.Contains(key))
                        continue;

                    var remain = false;
                    dict[key] = Regex.Replace(val, @"\d+", m =>
                    {
                        var mKey = int.Parse(m.Value);
                        if (done.Contains(mKey))
                            return $"(?:{dict[mKey]})";

                        remain = true;
                        return m.Value;
                    });

                    if (!remain)
                        done.Add(key);
                }
            }
        }

        private static (Dictionary<int, string>, List<string>) Parse(IEnumerable<string> input)
        {
            var dict = new Dictionary<int, string>();
            var messages = new List<string>();

            var mode = false;
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    mode = true;
                }
                else if (mode)
                {
                    messages.Add(line);
                }
                else
                {
                    var split = line.Split(':');
                    dict[int.Parse(split[0])] = split[1] switch
                    {
                        var s when s[1] is '"' => s[2..^1],
                        var s => s[1..]
                    };
                }
            }

            return (dict, messages);
        }
    }
}
