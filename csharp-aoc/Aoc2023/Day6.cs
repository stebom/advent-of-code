namespace Aoc2023;

internal class Day6 {
    record struct Goal(long Time, long Distance);

    internal static void Run() {
        var lines = File.ReadAllLines("input_day_6.txt").Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        Part1(lines);
        Part2(lines);
    }

    static void Part1(string[] lines) {
        var timeLine = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();
        var distanceLine = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();

        var product = timeLine.Zip(distanceLine)
                              .Select(goal => CountWays(new(goal.First, goal.Second)))
                              .Aggregate((a, b) => a * b);

        Console.WriteLine($"Part 1: {product}");
    }

    static void Part2(string[] lines) {
        var time = long.Parse(lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Aggregate((a, b) => a + b));
        var distance = long.Parse(lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Aggregate((a, b) => a + b));
        var product = CountWays(new Goal(time, distance));
        Console.WriteLine($"Part 2: {product}");
    }

    static long CountWays(Goal goal) => Range(goal.Time).Count(t => (goal.Time - t) * t > goal.Distance);

    static IEnumerable<long> Range(long end) { for (var i = 0; i <= end; i++) { yield return i; } }
}