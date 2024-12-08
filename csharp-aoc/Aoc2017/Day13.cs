using System.Diagnostics.Contracts;

namespace Aoc2017;

public static class Day13
{
    public static void Solve()
    {
        Part1();
        Part2();
    }

    public static void Part1()
    {
        var ranges = File.ReadAllLines(@"input_day_13.txt")
                         .Select(l => l.Split(": "))
                         .ToDictionary(k => int.Parse(k[0]), v => int.Parse(v[1]));

        var positions = ranges.ToDictionary(k => k.Key, v => GeneratePendulum(v.Value-1).GetEnumerator());

        foreach (var p in positions) p.Value.MoveNext();

        var duration = ranges.Keys.Max();

        long part1 = 0;
        for (int time = 0; time <= duration; time++)
        {
            if (positions.TryGetValue(time, out var position))
            {
                if (position.Current == 0)
                {
                    part1 += time * ranges[time];
                }
            }

            foreach (var p in positions) p.Value.MoveNext();
        }

        Console.WriteLine($"Part 1: {part1}");
    }

    public static void Part2()
    {
        var ranges = File.ReadAllLines(@"input_day_13.txt")
                         .Select(l => l.Split(": "))
                         .ToDictionary(k => int.Parse(k[0]), v => int.Parse(v[1]));

        var positions = ranges.ToDictionary(k => k.Key, v => GeneratePendulum(v.Value - 1).GetEnumerator());

        foreach (var p in positions)
        {
            for (var offset = 0; offset <= p.Key; offset++) p.Value.MoveNext();
        }

        var duration = ranges.Keys.Max();

        int delay = 1;
        while (true)
        {
            foreach (var p in positions) p.Value.MoveNext();
            if (positions.All(p => p.Value.Current != 0)) break;
            delay++;
        }

        Console.WriteLine($"Part 2: {delay}");
    }

    private static IEnumerable<int> GeneratePendulum(int max)
    {
        var current = 0;
        var step = -1;
        while (true)
        {
            yield return current;
            if (current == 0 || current == max) step = -step;
            current += step;
        }
    }
}