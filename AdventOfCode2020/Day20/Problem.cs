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
            var corner = GetTopLeft(tiles);

            var grid = DrawGrid(corner, tiles);

            long topLeft = grid.First().First().Id;
            long topRight = grid.First().Last().Id;
            long bottomLeft = grid.Last().First().Id;
            long bottomRight = grid.Last().Last().Id;

            long answer = topLeft * topRight * bottomLeft * bottomRight;

            Console.WriteLine(answer);
        }

        class Tile
        {
            public int Id { get; set; }
            public string Top { get; set; }
            public string Right { get; set; }
            public string Bottom { get; set; }
            public string Left { get; set; }
            public List<string> FullTile { get; set; } = new List<string>();

            public IEnumerable<(string, string)> Sides => new List<(string, string)>
            {
                (nameof(Top), Top),
                (nameof(Right), Right),
                (nameof(Left), Left),
                (nameof(Bottom), Bottom)
            };

            public List<(string, int)> Neighbours { get; set; } = new List<(string, int)>();

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

                for (int i = 1; i < input.Length; i++)
                {
                    FullTile.Add(input[i]);
                }
            }

            public void MatchTile(Tile otherTile)
            {
                var thisTile = Id;
                var otherTileId = otherTile.Id;
                foreach ((string name, string side) in Sides)
                {
                    
                    foreach ((string otherTileName,  string otherTileSide) in otherTile.Sides)
                    {
                        var reversed = new string(otherTileSide.Reverse().ToArray());
                        if (side.Equals(otherTileSide))
                        {
                            NumberOfMatches++;
                            otherTile.NumberOfMatches++;
                            Neighbours.Add((name, otherTile.Id));
                            otherTile.Neighbours.Add((otherTileName, Id));
                        }
                        else if (side.Equals(reversed))
                        {
                            if (otherTileName.Equals(nameof(Left)) || otherTileName.Equals(nameof(Right)))
                                otherTile.FlipVertically();
                            else if (otherTileName.Equals(nameof(Top)) || otherTileName.Equals(nameof(Bottom)))
                                otherTile.FlipHorizontally();
                            NumberOfMatches++;
                            otherTile.NumberOfMatches++;
                            Neighbours.Add((name, otherTile.Id));
                            otherTile.Neighbours.Add((otherTileName, Id));
                        }
                    }
                }
            }
            
            public void FlipHorizontally()
            {
                var flipped = new List<string>();

                for(int i = FullTile.Count() - 1; i >=0; i--)
                {
                    var row = FullTile[i];
                    flipped.Add(row);
                }

                FullTile = flipped;
                UpdateSides();
            }

            public void FlipVertically()
            {
                var flipped = new List<string>();

                foreach (string row in FullTile)
                {
                    flipped.Add(new string(row.Reverse().ToArray()));
                }

                
                FullTile = flipped;
                UpdateSides();
            }

            public void RotateLeft()
            {
                List<string> newTile = new List<string>();
                for (int i = 0; i < FullTile.Count(); i++)
                {
                    newTile.Add("");
                }

                foreach (string row in FullTile)
                {
                    for (int i = 0, j = row.Length - 1; i < newTile.Count() && j >=0; i++, j--)
                    {
                        newTile[i] += row[j];
                    }
                }
                
                FullTile = newTile;
                UpdateSides();
            }

            public void RotateRight()
            {
                List<string> newTile = new List<string>();
                for (int i = 0; i < FullTile.Count(); i++)
                {
                    newTile.Add("");
                }

                for (int k = FullTile.Count() - 1; k >= 0; k--)
                {
                    var row = FullTile[k];
                    for (int i = 0; i < FullTile.Count(); i++)
                    {
                        newTile[i] += row[i];
                    }
                }

                FullTile = newTile;
                UpdateSides();
            }

            public void UpdateSides()
            {
                Top = FullTile.First();
                Bottom = FullTile.Last();

                string left = "";

                for (int y = 0; y < FullTile.Count(); y++)
                {
                    left += FullTile[y][0];
                }

                Left = left;

                string right = "";

                for (int y = 0; y < FullTile.Count(); y++)
                {
                    right += FullTile[y][FullTile[y].Length - 1];
                }

                Right = right;
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

        private static Tile GetTopLeft(IEnumerable<Tile> tiles)
        {
            foreach (Tile tile in tiles)
            {
                var topCount = tiles.Where(t => !t.Id.Equals(tile.Id))
                    .Count(t => t.Sides.Any(s => s.Item2.Equals(tile.Top) || new string(s.Item2.Reverse().ToArray()).Equals(tile.Top)));

                var leftCount = tiles.Where(t => !t.Id.Equals(tile.Id))
                    .Count(t => t.Sides.Any(s => s.Item2.Equals(tile.Left) || new string(s.Item2.Reverse().ToArray()).Equals(tile.Left)));

                if (topCount == 0 && leftCount == 0)
                    return tile;
            }

            return null;
        }

        private static List<List<Tile>> DrawGrid(Tile cornerTile, IEnumerable<Tile> allTiles)
        {
            List<List<Tile>> grid = new List<List<Tile>>();

            var gridLength = Math.Sqrt(allTiles.Count());

            var firstInRow = cornerTile;
            var tile = firstInRow;

            for (int y = 0; y < gridLength; y++)
            {
                var row = new List<Tile>();
                for (int x = 0; x < gridLength; x++)
                {
                    var tileId = tile.Id;
                    row.Add(tile);

                    var right = allTiles
                        .Where(t => !t.Id.Equals(tile.Id) &&
                        !row.Any(x => x.Id.Equals(t.Id)) &&
                        !grid.Any(r => r.Any(i => !i.Id.Equals(t.Id))))
                        .FirstOrDefault(t => t.Sides.Any(s => s.Item2.Equals(tile.Right) || new string(s.Item2.Reverse().ToArray()).Equals(tile.Right)));

                    if (right is null)
                        continue;

                    PrintTile(tile);
                    PrintTile(right);

                    var side = right.Sides.First(s => s.Item2.Equals(tile.Right) || new string(s.Item2.Reverse().ToArray()).Equals(tile.Right));
                    if (side.Item2.Equals(tile.Right))
                    {
                        switch (side.Item1)
                        {
                            case "Left":
                                break;
                            case "Right":
                                right.FlipVertically();
                                break;
                            case "Top":
                                right.RotateLeft();
                                right.FlipHorizontally();
                                break;
                            case "Bottom":
                                right.RotateRight();
                                break;
                        }
                    }
                    else if (new string(side.Item2.Reverse().ToArray()).Equals(tile.Right))
                    {
                        switch (side.Item1)
                        {
                            case "Right":
                                right.FlipVertically();
                                right.FlipHorizontally();
                                break;
                            case "Left":
                                right.FlipVertically();
                                break;
                            case "Top":
                                right.RotateLeft();
                                break;
                            case "Bottom":
                                right.RotateRight();
                                right.FlipHorizontally();
                                break;
                        }
                    }

                    Console.WriteLine("Updated:");
                    PrintTile(right);

                    tile = right;
                }
                grid.Add(row);

                var above = allTiles
                        .FirstOrDefault(t => !t.Id.Equals(firstInRow.Id) &&
                        !grid.Any(r => r.Any(i => i.Equals(t.Id.ToString()))) &&
                        t.Sides.Any(s => s.Item2.Equals(firstInRow.Bottom) || new string(s.Item2.Reverse().ToArray()).Equals(firstInRow.Bottom)));

                if (above is null)
                    continue;

                PrintTile(firstInRow);
                PrintTile(above);

                var aboveSide = above.Sides.First(s => s.Item2.Equals(firstInRow.Bottom) || new string(s.Item2.Reverse().ToArray()).Equals(firstInRow.Bottom));
                if (aboveSide.Item2.Equals(firstInRow.Bottom))
                {
                    switch (aboveSide.Item1)
                    {
                        case "Top":
                            break;
                        case "Bottom":
                            above.FlipHorizontally();
                            break;
                        case "Left":
                            above.RotateRight();
                            above.FlipVertically();
                            break;
                        case "Right":
                            above.RotateLeft();
                            break;
                    }
                }
                else if (new string(aboveSide.Item2.Reverse().ToArray()).Equals(firstInRow.Bottom))
                {
                    switch (aboveSide.Item1)
                    {
                        case "Bottom":
                            above.FlipVertically();
                            above.FlipHorizontally();
                            break;
                        case "Right":
                            above.RotateRight();
                            above.FlipHorizontally();
                            break;
                        case "Top":
                            above.FlipVertically();
                            break;
                        case "Left":
                            above.RotateRight();
                            break;
                    }
                }

                firstInRow = above;
                tile = firstInRow;
            }

            return grid;
        }

        private static void PrintTile(Tile tile)
        {
            Console.WriteLine($"Tile {tile.Id}");
            foreach(string line in tile.FullTile)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
    }
}
