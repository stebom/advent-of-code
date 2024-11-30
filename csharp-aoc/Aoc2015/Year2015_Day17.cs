namespace AdventOfCode;

internal static class Year2015_Day17
{
    static int CountBits(int value)
    {
        int count = 0;
        while (value != 0)
        {
            count++;
            value &= value - 1;
        }
        return count;
    }

    static bool IsSet(int flags, int bit) => (flags & (1 << bit)) != 0;
    static int Set(int flags, int bit) => flags | (1 << bit);

    static readonly Dictionary<int, int> DP = [];

    static void Fringe(int[] containers, HashSet<int> solutions, int current, int target)
    {
        if (DP.TryGetValue(current, out var volume)) {

        } else {
            volume = Enumerable.Range(0, containers.Length).Sum(i => IsSet(current, i) ? containers[i] : 0);
            DP[current] = volume;
        }

        if (volume == target)
        {
            solutions.Add(current);
            return;
        }
        if (volume > target) return;

        for (int i = 0; i < containers.Length; i++)
        {
            if (IsSet(current, i)) continue;
            var next = Set(current, i);

            if (!DP.ContainsKey(next))
            {
                Fringe(containers, solutions, next, target);
            }
        }
    }

    public static void Solve()
    {
        //const int target = 25;
        //int[] containers = [20, 15, 10, 5, 5];

        const int target = 150;
        var containers = File.ReadAllText(@"2015_17_input.txt").Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        HashSet<int> solutions = [];
        Fringe(containers, solutions, 0, target);
        Console.WriteLine($"Part 1: {solutions.Count}");

        var minumumBits = solutions.Min(CountBits);
        Console.WriteLine($"Part 2: {solutions.Count(s => CountBits(s) == minumumBits)}");

    }
}
