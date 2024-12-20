using System.Collections.Immutable;
using System.Diagnostics;

namespace Aoc2024;

public static class Day19
{
    public static void Solve()
    {
        var groups = File.ReadAllText(@"input/day19_input.txt").Split("\r\n\r\n");
        Debug.Assert(groups.Length == 2);

        var patterns = groups[0].Split(", ").OrderByDescending(s => s.Length).ToImmutableArray();

        var part1 = 0;
        long part2 = 0;

        foreach (var design in groups[1].Split("\r\n"))
        {
            // Trim patterns to matching only
            var trimmedPatterns = patterns.Where(design.Contains).OrderByDescending(x => x.Length).ToImmutableArray();
            part1 += Search(trimmedPatterns, design);
            part2 += SearchAll(trimmedPatterns, design);
        }

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    static int Search(ImmutableArray<string> patterns, string design)
    {
        var queue = new Queue<string>();
        queue.Enqueue("");

        var visited = new HashSet<string>();
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (visited.Contains(current)) continue;
            visited.Add(current);

            if (current.Length > design.Length) continue;
            if (!design.StartsWith(current)) continue;
            if (current == design) return 1;

            foreach (var pattern in patterns)
            {
                var combined = current + pattern;
                if (combined.Length <= design.Length)
                {
                    queue.Enqueue(combined);
                }
            }
        }

        return 0;
    }


    static long SearchAll(ImmutableArray<string> patterns, string design) => SearchRecursive(patterns, [], design);

    static long SearchRecursive(ImmutableArray<string> patterns, Dictionary<string, long> lut, string current)
    {
        if (lut.TryGetValue(current, out var solutions)) return solutions;

        if (current == string.Empty) return 1;

        solutions = patterns.Where(current.StartsWith).Sum(p => SearchRecursive(patterns, lut, current[p.Length..]));

        lut[current] = solutions;
        return solutions;
    }
}