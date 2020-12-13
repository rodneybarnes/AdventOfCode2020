using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day12
{
    public static class CardinalDirections
    {
        public const string North = "North";
        public const string South = "South";
        public const string East = "East";
        public const string West = "West";

        public static List<string> Points = new List<string>
        {
            North,
            East,
            South,
            West
        };

        /// <summary>
        /// Find the index of the current direction the ship is facing
        /// If the index + the number of turns is greater than the length of our CardinalDirections,
        /// we can simply subtract the number of points from that sum to get our new index.
        /// </summary>
        /// <param name="degrees"></param>
        /// <param name="currentDirection"></param>
        /// <returns></returns>
        public static string RotateRight(int degrees, string currentDirection)
        {
            var numberOfTurns = degrees / 90;
            var indexAt = Points.FindIndex(d => d == currentDirection);
            int newIndex = indexAt + numberOfTurns;

            if (indexAt + numberOfTurns >= Points.Count())
                newIndex -= Points.Count();

            return Points.ElementAt(newIndex);
        }

        public static string RotateLeft(int degrees, string currentDirection)
        {
            var numberOfTurns = degrees / 90;
            var indexAt = Points.FindIndex(d => d == currentDirection);
            int newIndex = indexAt - numberOfTurns; ;

            if (indexAt - numberOfTurns < 0)
                newIndex = Points.Count() + (indexAt - numberOfTurns);

            return Points.ElementAt(newIndex);
        }
    }
}
