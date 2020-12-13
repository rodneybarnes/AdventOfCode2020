using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day13
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("daythirteen.txt");
            var test = InputParser.GetLines("daythirteen-test.txt");

            var multiTest = InputParser.GetLines("daythirteen-list.txt");
            foreach (string line in multiTest)
            {
                //GetGoldCoinByCheating(line.Split(",").ToList());
            }

            GetGoldCoinByCheating(actual[1].Split(",").ToList());
        }

        private static void FindEarliestBus(string[] notes)
        {
            int earliestDeparture = int.Parse(notes[0]);
            IEnumerable<int> busIds = notes[1].Split(",").Where(bId => bId.All(char.IsNumber)).Select(bId => int.Parse(bId));

            int answer = 0;
            int currentTime = earliestDeparture;

            while(answer == 0)
            {
                foreach(int busId in busIds)
                {
                    if (currentTime % busId == 0)
                    {
                        answer = (currentTime - earliestDeparture) * busId;
                        break;
                    }
                }
                currentTime++;
            }

            Console.WriteLine(answer);
        }

        /// <summary>
        /// Math isn't my strong suit, and while my brute-force method below works, I guess I'd prefer to spend
        /// that time trying to figure out the answer and finally use someone else's answer than have it calculate to completion.
        /// 
        /// I got this answer here: https://www.reddit.com/r/adventofcode/comments/kc4njx/2020_day_13_solutions/gfokhzh/
        /// </summary>
        /// <param name="busIds"></param>
        private static void GetGoldCoinByCheating(List<string> busIds)
        {
            var time = 0L;
            var increment = long.Parse(busIds[0]);
            for (var i = 1; i < busIds.Count(); i++)
            {
                if (!busIds[i].Equals("x"))
                {
                    var newTime = int.Parse(busIds[i]);
                    while (true)
                    {
                        time += increment;
                        if ((time + i) % newTime == 0)
                        {
                            increment *= newTime;
                            break;
                        }
                    }
                }
            }
            Console.WriteLine(time);
        }

        /// <summary>
        /// WARNING: USING A LARGE SET WILL TAKE AN EXTREMELY LONG TIME.
        /// </summary>
        /// <param name="busIds"></param>
        private static void GetGoldCoinByBrute(List<string> busIds)
        {
            int firstBusId = int.Parse(busIds.First());

            double timestamp = 0;
            double answer = 0;

            while (answer == 0)
            {
                for (int i = 0; i < busIds.Count(); i++)
                {
                    if (busIds.ElementAt(i) == "x")
                        continue;

                    int busId = int.Parse(busIds.ElementAt(i));

                    if ((timestamp + i) % busId == 0)
                    {
                        if (i == busIds.Count() - 1)
                            answer = timestamp;
                        else continue;
                    }
                    else break;
                }
                timestamp += firstBusId;
            }

            Console.WriteLine(answer);
        }
    }
}
