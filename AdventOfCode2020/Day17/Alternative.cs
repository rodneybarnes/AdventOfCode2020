using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Day17
{
	/// <summary>
	/// I got frustrated with this one - didn't set myself up great in part A, and burned out trying to think my way through Part B. 
	/// So this is someones else's solution: https://github.com/viceroypenguin/adventofcode/blob/master/2020/day17.original.cs
	/// </summary>
	public class Alternative
    {
        public static void Solve(string[] input)
        {
            //var actual = InputParser.GetLines("dayseventeen.txt");
            //var test = InputParser.GetLines("dayseventeen-test.txt");
            SolveB(input);
        }

        private static void SolveB(string[] input)
        {
            var state = new Dictionary<(int x, int y, int z, int w), bool>(8192);
            int _x = 0, _y = 0;
            foreach (var line in input)
            {
                foreach (var c in line)
                {
                    state[(_x++, _y, 0, 0)] = c == '#';
                }
                _x = 0; _y++;
            }

			var count = new Dictionary<(int x, int y, int z, int w), int>(8192);
			var dirs = Enumerable.Range(-1, 3)
				.SelectMany(x => Enumerable.Range(-1, 3)
					.SelectMany(y => Enumerable.Range(-1, 3)
						.SelectMany(z => Enumerable.Range(-1, 3)
							.Select(w => (x, y, z, w)))))
				.Where(d => d != (0, 0, 0, 0))
				.ToArray();
			for (int i = 0; i < 6; i++)
			{
				count.Clear();

				// so count has everything, and we can rely on that in final foreach
				foreach (var p in state.Keys)
					count[p] = 0;

				foreach (var ((x, y, z, w), alive) in state.Where(kvp => kvp.Value))
					foreach (var (dx, dy, dz, dw) in dirs)
						count[(x + dx, y + dy, z + dz, w + dw)] =
							count.GetValueOrDefault((x + dx, y + dy, z + dz, w + dw)) + 1;

				foreach (var (p, c) in count)
					state[p] = (state.GetValueOrDefault(p), c) switch
					{
						(true, >= 2 and <= 3) => true,
						(false, 3) => true,
						_ => false,
					};
			}

			var sol = state.Where(kvp => kvp.Value).Count().ToString();
			Console.WriteLine(sol);
		}
    }
}
