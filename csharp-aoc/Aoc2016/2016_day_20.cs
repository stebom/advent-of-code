using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AdventOfCode;

public class Aoc_2016_Day_20
{
    record struct Range(uint Start, uint End);

    internal static void Solve()
    {
        var ranges = File.ReadAllLines(@"2016_day_20.txt").Select(ParseRange).OrderBy(r => r.Start).ThenBy(r => r.End).ToList();

        var initialCount = ranges.Count;

        var passes = 0;
        while (Condense(ranges)) passes++;

        Console.WriteLine($"Done condenseding {initialCount} ranges into {ranges.Count} after {passes} passes");

        uint sum = 0;
        foreach (var range in ranges.OrderBy(r => r.Start).ThenBy(r => r.End))
        {
            Console.WriteLine($"Range: {range} {range.End - range.Start + 1}");

            sum += range.End - range.Start + 1;
        }

        long max = (long)uint.MaxValue + 1;
        Console.WriteLine(max - sum);

        var totalBanned = (uint)ranges.OrderBy(r => r.Start).ThenBy(r => r.End).Sum(r => r.End - r.Start + 1);
        Console.WriteLine($"Number of banned addresses: {totalBanned} ({uint.MaxValue - totalBanned} allowed)");
    }

    static bool Condense(List<Range> ranges)
    {
        for (var i = 0; i < ranges.Count; i++)
        {
            var range = ranges[i];

            for (var y = 0; y < ranges.Count; y++)
            {
                var r = ranges[y];
                if (range == r) continue;

                if (r.Start <= range.Start && range.End <= r.End)
                {
                    Console.WriteLine($"Deduplicating: {range} -> {r}");
                    ranges.Remove(range);
                    return true;
                }

                if (range.Start < r.Start && r.Start <= range.End && range.End <= r.End)
                {
                    var expanded = new Range(range.Start, r.End);
                    ranges.Remove(r);
                    ranges.Remove(range);
                    ranges.Add(expanded);

                    Console.WriteLine($"Expand left: {range} -> {r} => {expanded}");
                    return true;
                }

                if (r.Start <= range.Start && range.End <= r.End && r.End >= range.End)
                {
                    var expanded = new Range(r.Start, range.End);
                    ranges.Remove(r);
                    ranges.Remove(range);
                    ranges.Add(expanded);
                    
                    Console.WriteLine($"Expand right: {range} -> {r} => {expanded}");
                    return true;
                }

                if (range.End < uint.MaxValue && (range.End + 1) == r.Start)
                {
                    var joined = new Range(range.Start, r.End);
                    ranges.Remove(r);
                    ranges.Remove(range);
                    ranges.Add(joined);
                    Console.WriteLine($"Left join: {range} -> {r} => {joined}");
                    return true;
                }
            }
        }
        return false;
    }

    static Range ParseRange(string line) => new(uint.Parse(line[..line.IndexOf('-')]), uint.Parse(line[(line.IndexOf('-') + 1)..]));
}