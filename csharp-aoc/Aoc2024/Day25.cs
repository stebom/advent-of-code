using System.Diagnostics;

namespace Aoc2024;

public static class Day25
{
    public static void Solve()
    {
        List<List<int>> locks = [];
        var groups = File.ReadAllText("input/day25_input.txt").Split("\n\n");
        foreach (var group in groups)
        {

            var rows = group.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Debug.Assert(7 == rows.Length);
            Debug.Assert(rows.All(r => r.Length == 5));

            var current = new List<int>();
            for (var c = 0; c < 5; c++)
            {
                current.Add(Convert.ToInt16(new string(rows.Select(r => r[c] == '#' ? '1' : '0').ToArray()), 2));
            }

            locks.Add(current);
        }

        var fits = 0;
        for (var i = 0; i < locks.Count; i++)
        {
            for (var y = i + 1; y < locks.Count; y++)
            {
                var test = Enumerable.Range(0, 5).All(c => (locks[i][c] & locks[y][c]) == 0);
                fits += test ? 1 : 0;
            }
        }
        Console.WriteLine(fits);
    }
}