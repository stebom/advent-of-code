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
        var part2 = 0;

        foreach (var design in groups[1].Split("\r\n"))
        {
            // Trim patterns
            var usedPatterns = patterns.Where(design.Contains).OrderByDescending(x => x.Length).ToImmutableArray();

            Console.WriteLine($"Searching {design}");
            part1 += Search(usedPatterns, design);
            Console.WriteLine($" ... part 1 done");
            part2 += SearchAll(usedPatterns, design);
            Console.WriteLine($" ... part 2 done");
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


    static int SearchAll(ImmutableArray<string> patterns, string design)
    {
        var queue = new Queue<string>();
        queue.Enqueue(design);

        var visited = new HashSet<string>();

        var count = 0;
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current == string.Empty)
            {
                count++;
                continue;
            }

            if (visited.Contains(current)) continue;
            visited.Add(current);

            foreach (var pattern in patterns)
            {
                if (current.StartsWith(pattern))
                {
                    queue.Enqueue(current[pattern.Length..]);
                }
            }
        }

        return count;
    }
}