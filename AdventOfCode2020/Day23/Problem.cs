using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2020.Day23
{
    public class Problem
    {
        public static void Solve()
        {
            const string actual = "871369452";
            const string test = "389125467";
            PlayCrabGame(actual, 10000000);
        }

        private static void PlayCrabGame(string input, int moves)
        {
            List<int> inputIntList = input.ToIntList();
            LinkedList<int> cups = new LinkedList<int>();
            Dictionary<int, LinkedListNode<int>> cupsDictionary = new Dictionary<int, LinkedListNode<int>>();
            Dictionary<int, bool> isIn = new Dictionary<int, bool>();

            foreach (int value in inputIntList)
            {
                cupsDictionary[value] = cups.AddLast(value);
                isIn[value] = true;
            }

            int highest = inputIntList.Max() + 1;

            while (highest <= 1000000)
            {
                cupsDictionary[highest] = cups.AddLast(highest);
                isIn[highest] = true;
                highest++;
            }

            LinkedListNode<int> currentCup = cups.First;
            int highestValue = cups.Max();
            int lowestValue = cups.Min();

            for (int i = 0; i < moves; i++)
            {
                // Pick up the three cups next to the current cup
                List<int> pickedUp = new List<int>();
                var nextCup = currentCup.Next ?? currentCup.List.First;

                for (int j = 0; j < 3; j++)
                {
                    pickedUp.Add(nextCup.Value);
                    var cupToRemove = nextCup;
                    nextCup = nextCup.Next ?? nextCup.List.First;
                    cups.Remove(cupToRemove);
                }

                // Select the destination cup
                int cupLabel = currentCup.Value;
                
                LinkedListNode<int> destinationCup = null;

                while (destinationCup is null)
                {
                    cupLabel--;
                    if (pickedUp.Contains(cupLabel))
                        continue;

                    if (cupLabel < lowestValue)
                    {
                        destinationCup = cupsDictionary[highestValue];
                        break;
                    }

                    if (cupsDictionary.TryGetValue(cupLabel, out LinkedListNode<int> success))
                    {
                        destinationCup = success;
                        break;
                    }
                }

                // Return the picked-up cups
                foreach (int cup in pickedUp)
                {
                    cupsDictionary[cup] = cups.AddAfter(destinationCup, cup);
                    destinationCup = destinationCup.Next;
                }

                currentCup = currentCup.Next ?? currentCup.List.First;
            }

            LinkedListNode<int> thisCup = cupsDictionary[1];
            //cups.Remove(1);
            long cupAfter = thisCup.Next.Value;
            long cupAfterThat = thisCup.Next.Next.Value;
            long solution = cupAfter * cupAfterThat;

            //while (cups.Any())
            //{
            //    solution += thisCup.Value.ToString();
            //    var cupToRemove = thisCup;
            //    thisCup = thisCup.Next ?? thisCup.List.First;
            //    cups.Remove(cupToRemove);
            //}

            Console.WriteLine(solution);
        }
    }

    public static class Helper
    {
        public static List<int> ToIntList(this string input)
        {
            List<int> intList = new List<int>();
            foreach (char c in input)
            {
                if (int.TryParse(c.ToString(), out int parsed))
                    intList.Add(parsed);
            }

            return intList;
        }
    }
}
