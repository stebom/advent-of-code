namespace Aoc2024;

public static class Day1
{
    public static void Solve()
    {
        (List<int>, List<int>) columns = ([], []);

        foreach (var split in File.ReadAllLines(@"Day1_input.txt").Select(line => line.Split("   ")))
        {
            columns.Item1.Add(int.Parse(split[0]));
            columns.Item2.Add(int.Parse(split[1]));
        }

        var zipped = columns.Item1.Order().Zip(columns.Item2.Order());
        var part1 = zipped.Sum(p => Math.Abs(p.Second - p.First));
        Console.WriteLine($"Part 1: {part1}");

        var part2 = columns.Item1.Sum(first => first * columns.Item2.Count(n => n == first));
        Console.WriteLine($"Part 2: {part2}");
    }
}
