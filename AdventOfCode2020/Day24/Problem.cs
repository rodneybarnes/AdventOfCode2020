using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day24
{
    /// <summary>
    /// I was really burning out by this point, so I took the easy way out on this and used another's solution:
    /// https://github.com/hlim29/AdventOfCode2020/blob/master/Days/DayTwentyfour.cs
    /// </summary>
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("daytwentyfour.txt");
            var test = InputParser.GetLines("daytwentyfour-test.txt");
            var tilesToFlip = ProcessInstructions(actual);
            var partOne = tilesToFlip.Count(t => !t.Value);
            var partTwo = Run100Days(tilesToFlip);
            Console.WriteLine(partTwo);
        }

        private static readonly Dictionary<string, (double X, double Y)> _offsets = new Dictionary<string, (double, double)>
        {
            {"e", (1,0) },
            {"w", (-1,0) },
            {"se", (0.5,-1) },
            {"sw", (-0.5,-1) },
            {"nw", (-0.5,1) },
            {"ne", (0.5,1) },
        };

        private static readonly List<(double X, double Y)> _offsetValues = _offsets.Values.Select(x => x).ToList();

        private static int Run100Days(Dictionary<(double X, double Y), bool> grid)
        {
            Enumerable.Range(1, 100).ToList().ForEach(_ =>
            {
                var tiles = grid.Keys.ToList();
                var copy = new Dictionary<(double X, double Y), bool>(grid);
                var keys = grid.Keys.ToHashSet();

                foreach (var tile in tiles)
                {
                    var neighbours = _offsetValues.Select(x => (x.X + tile.X, x.Y + tile.Y)).ToList();
                    neighbours.ForEach(tile =>
                    {
                        copy[tile] = UpdateTile(tile, keys, grid);
                    });

                    copy[tile] = UpdateTile(tile, keys, grid);
                }

                grid = new Dictionary<(double X, double Y), bool>(copy);
            });

            return grid.Values.Count(x => !x);
        }

        private static bool UpdateTile((double X, double Y) tile, HashSet<(double X, double Y)> keys, Dictionary<(double X, double Y), bool> grid)
        {
            var exists = keys.Contains(tile);
            var inactiveNeighbours = _offsetValues.Select(x => (x.X + tile.X, x.Y + tile.Y)).Count(item => keys.Contains(item) && !grid[item]);
            var isInactive = exists && !grid[tile];

            if (isInactive && (inactiveNeighbours == 0 || inactiveNeighbours > 2))
            {
                return true;
            }
            else if (!isInactive && inactiveNeighbours == 2)
            {
                return false;
            }

            return !exists || grid[tile];
        }

        private static Dictionary<(double X, double Y), bool> ProcessInstructions(string[] instructions)
        {
            var diagonals = new List<string> { "se", "sw", "nw", "ne" };

            var toFlip = new List<(double X, double Y)>();
            foreach (var line in instructions)
            {
                var steps = new List<string>();
                var buffer = "";

                for (var i = 0; i < line.Length; i++)
                {
                    buffer += line[i];
                    if (i < line.Length - 1)
                    {
                        buffer += line[i + 1];
                    }

                    if (diagonals.Contains(buffer))
                    {
                        steps.Add(buffer);
                        i++;
                    }
                    else
                    {
                        steps.Add(buffer[0].ToString());
                    }
                    buffer = string.Empty;
                }

                var coord = (X: 0.0, Y: 0.0);
                steps.Select(x => _offsets[x]).ToList().ForEach(x => coord = (coord.X + x.X, coord.Y + x.Y));
                toFlip.Add(coord);
            }

            return toFlip.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count() % 2 == 0);
        }
    }
}
