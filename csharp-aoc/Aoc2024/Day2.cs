namespace Aoc2024;

public static class Day2
{
    public static void Solve()
    {
        var records = File.ReadAllLines(@"Day2_input.txt").Select(line => line.Split().Select(int.Parse).ToList());

        Console.WriteLine($"Part 1: {records.Count(Check)}");
        Console.WriteLine($"Part 2: {records.Count(CheckWithRemove)}");
    }

    private static bool CheckWithRemove(List<int> record)
    {
        if (Check(record)) return true;

        // Try to remove any item
        return Enumerable.Range(0, record.Count).Any(i => Check(record.Where((_, e) => e != i).ToList()));
    }

    private static bool Check(List<int> record)
    {
        var sign = Math.Sign(record[0] - record[1]);

        for (var i = 0; i < record.Count - 1; i++)
        {
            var diff = Math.Abs(record[i] - record[i + 1]);
            var safe = sign == Math.Sign(record[i] - record[i + 1]) &&
                       1 <= diff && diff <= 3;

            if (!safe) { return false; }
        }

        return true;
    }
}
