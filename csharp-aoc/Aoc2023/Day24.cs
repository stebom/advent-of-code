using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;

namespace AdventOfCode;

internal static partial class Day24
{
    static readonly string[] TestInput = [
        "19, 13, 30 @ -2, 1, -2",
        "18, 19, 22 @ -1, -1, -2",
        "20, 25, 34 @ -2, -2, -4",
        "12, 31, 28 @ -1, -2, -1",
        "20, 19, 15 @ 1, -5, -3",
    ];

    record Hailstone(long X, long Y, long Z, long DX, long DY, long DZ);

    [GeneratedRegex(@"(-?\d+), (-?\d+), (-?\d+) @ (-?\d+), (-?\d+), (-?\d+)", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex HailstoneGeneratedRegex();

    static Hailstone Parse(string input)
    {
        var match = HailstoneGeneratedRegex().Match(input);
        Debug.Assert(match.Success);
        var numbers = match.Groups.Values.Skip(1).Select(g => long.Parse(g.Value)).ToList();
        return new(numbers[0], numbers[1], numbers[2], numbers[3], numbers[4], numbers[5]);
    }

    record struct Point(long x, long y);

    public static void Solve()
    {
        //var lowerLimit = 7;
        //var upperLimit = 27;
        //var lines = TestInput;

        var lines = File.ReadAllLines(@"2023_24_input.txt");
        var lowerLimit = 200_000_000_000_000;
        var upperLimit = 400_000_000_000_000;

        var hailstones = lines.Select(Parse).ToList();

        long count = 0;

        for (var x = 0; x < hailstones.Count; x++)
        {
            for (var y = x + 1; y < hailstones.Count; y++)
            {
                var a = hailstones[x];
                var b = hailstones[y];

                var det = (b.DX * a.DY - b.DY * a.DX);

                if (det == 0) continue;

                var u = ((b.Y - a.Y) * b.DX - (b.X - a.X) * b.DY) / det;
                var v = ((b.Y - a.Y) * a.DX - (b.X - a.X) * a.DY) / det;

                if (u < 0 || v < 0) continue;

                var xi = b.X + b.DX * v;
                var yi = b.Y + b.DY * v;

                if (lowerLimit <= xi && xi <= upperLimit &&
                    lowerLimit <= yi && yi <= upperLimit)
                {
                    count++;
                }
            }
        }

        Console.WriteLine($"{count} hailstones' future paths cross inside the boundaries of the test area.");
    }
}
