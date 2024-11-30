using System.Text.RegularExpressions;

namespace AdventOfCode;

partial class Aoc_2016_Day_15
{
    record Disc(int PositionsCount, int Position);

    internal static void Solve()
    {
        string[] input = [
            //"Disc #1 has 5 positions; at time=0, it is at position 4.",
            //"Disc #2 has 2 positions; at time=0, it is at position 1."
            "Disc #1 has 13 positions; at time=0, it is at position 11.",
            "Disc #2 has 5 positions; at time=0, it is at position 0.",
            "Disc #3 has 17 positions; at time=0, it is at position 11.",
            "Disc #4 has 3 positions; at time=0, it is at position 0.",
            "Disc #5 has 7 positions; at time=0, it is at position 2.",
            "Disc #6 has 19 positions; at time=0, it is at position 17.",
            "Disc #7 has 11 positions; at time=0, it is at position 0.",
        ];

        var regex = MyRegex();
        var discs = input.Select(str => regex.Match(str))
                         .Select(m => new Disc(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)))
                         .ToList();

        var time = 0;
        var pass = false;
        do
        {
            pass = discs.Select((disc, i) => (disc.Position + time + i + 1) % disc.PositionsCount == 0).All(p => p);
            time++;
        } while (!pass);

        Console.WriteLine($"Time {time}");
    }

    [GeneratedRegex(@"Disc #\d+ has (\d+) positions; at time=0, it is at position (\d+).")]
    private static partial Regex MyRegex();
}