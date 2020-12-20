using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day20
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("daytwenty.txt");
            var test = InputParser.GetLines("daytwenty-test.txt");

            var tiles = ParseInputIntoTiles(actual);
            MatchTiles(tiles);

            decimal answer = tiles.Where(t => t.NumberOfMatches == 2).Select(t => t.Id).Aggregate(1M, (prev, next) => prev * next);
            Console.WriteLine(answer);

            //foreach (Tile tile in tiles)
            //{
            //    Console.WriteLine($"Tile {tile.Id} has {tile.NumberOfMatches} matched sides");
            //}
        }

        class Tile
        {
            public int Id { get; set; }
            public string Top { get; set; }
            public string Right { get; set; }
            public string Bottom { get; set; }
            public string Left { get; set; }
            public IEnumerable<string> Sides => new List<string>
            {
                Top,
                Right,
                Bottom,
                Left
            };

            public int NumberOfMatches { get; set; }

            public Tile(string[] input)
            {
                Id = int.Parse(input[0].Substring(5, 4));
                Top = input[1];
                Bottom = input.Last();

                string left = "";

                for (int y = 1; y < input.Length; y++)
                {
                    left += input[y][0];
                }

                Left = left;

                string right = "";

                for (int y = 1; y < input.Length; y++)
                {
                    right += input[y][input[y].Length - 1];
                }

                Right = right;
            }

            public void MatchTile(Tile tile)
            {
                foreach (string side in Sides)
                {
                    var reversed = new string(side.Reverse().ToArray());
                    foreach (string otherTileSide in tile.Sides)
                    {
                        if (side.Equals(otherTileSide) || reversed.Equals(otherTileSide))
                        {
                            NumberOfMatches++;
                            tile.NumberOfMatches++;
                        }
                    }
                }
            }

        }

        private static IEnumerable<Tile> ParseInputIntoTiles(string[] input)
        {
            List<Tile> tiles = new List<Tile>();
            List<string> tileInfo = new List<string>();

            foreach (string line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    tiles.Add(new Tile(tileInfo.ToArray()));
                    tileInfo = new List<string>();
                    continue;
                }

                tileInfo.Add(line);
            }

            if (tileInfo.Any())
            {
                tiles.Add(new Tile(tileInfo.ToArray()));
            }
            return tiles;
        }

        private static void MatchTiles(IEnumerable<Tile> tiles)
        {
            for (int i = 0; i < tiles.Count(); i++)
            {
                Tile thisTile = tiles.ElementAt(i);
                for (int j = i + 1; j < tiles.Count(); j++)
                {
                    Tile toCompare = tiles.ElementAt(j);
                    thisTile.MatchTile(toCompare);
                }
            }
        }


    }
}
