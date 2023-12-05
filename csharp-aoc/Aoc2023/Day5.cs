namespace Aoc2023;

internal class Day5 {
    record Seed(long Start, long End);
    record Filter(long Destination, long Source, long Length);
    record Map(string Name, IList<Filter> Filters);

    static IEnumerable<long> ParseSeeds(string line) => line.Split().Skip(1).Select(long.Parse);

    static IEnumerable<Map> ParseMaps(IEnumerable<string> sections) {
        foreach (var section in sections) {
            var lines = section.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var filters = lines[1..].Select(line =>
                line.Split() switch {
                    [string d, string s, string l] => new Filter(long.Parse(d), long.Parse(s), long.Parse(l)),
                    _ => throw new InvalidOperationException(line)
                }
            );

            yield return new(lines[0][..^1], filters.ToList());
        }
    }

    internal static void Run() {
        var sections = File.ReadAllText("input_day_5.txt").Split("\n\n");
        var maps = ParseMaps(sections.Skip(1)).ToList();
        var seeds = ParseSeeds(sections.First());

        Part1(seeds.ToList(), maps);
        Part2(seeds.Chunk(2).Select(s => new Seed(s[0], s[0] + s[1])).ToList(), maps);
    }

    static void Part1(IList<long> seeds, IList<Map> maps) {
        for (var i = 0; i < seeds.Count; i++) {
            foreach (var map in maps) {
                var seed = seeds[i];
                var filter = map.Filters.SingleOrDefault(filter => filter.Source <= seed && seed <= filter.Source + filter.Length - 1);
                if (filter != null) {
                    seeds[i] = filter.Destination + seed - filter.Source;
                }
            }
        }

        Console.WriteLine($"Part 1: {seeds.Min()}");
    }

    static void Part2(IList<Seed> seeds, IList<Map> maps) {
        var queue = new Queue<Seed>(seeds);
        foreach (var map in maps) {
            var next = new Queue<Seed>();
            while (queue.Count > 0) {
                var seed = queue.Dequeue();

                var appliedFilter = false;

                foreach (var filter in map.Filters) {
                    var overlapStart = Math.Max(seed.Start, filter.Source);
                    var overlapEnd = Math.Min(seed.End, filter.Source + filter.Length - 1);

                    if (overlapStart < overlapEnd) {
                        var start = overlapStart - filter.Source + filter.Destination;
                        var end = overlapEnd - filter.Source + filter.Destination;
                        next.Enqueue(new(start, end));

                        if (overlapStart > seed.Start) {
                            queue.Enqueue(new(seed.Start, overlapStart));
                        }
                        if (seed.End > overlapEnd) {
                            queue.Enqueue(new(overlapEnd, seed.End));
                        }
                        appliedFilter = true;
                        break;
                    }
                }
                if (!appliedFilter) {
                    next.Enqueue(seed);
                }
            }

            queue = next;
        }

        Console.WriteLine($"Part 2: {queue.Min(s => s.Start)}");
    }
}