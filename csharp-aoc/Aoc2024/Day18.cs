namespace Aoc2024;

public static class Day18
{
    private const int Size = 70;
    private const int Time = 1024;
    private static readonly Bit Start = new(0, 0);
    private static readonly Bit End = new(Size, Size);

    readonly record struct Bit(int R, int C)
    {
        public readonly IEnumerable<Bit> Walk()
        {
            Bit[] directions = [new Bit(R - 1, C), new Bit(R, C + 1), new Bit(R + 1, C), new Bit(R, C - 1)];
            foreach (var dir in directions)
            {
                if (dir.R < 0 || dir.R > Size) continue;
                if (dir.C < 0 || dir.C > Size) continue;
                yield return dir;
            }
        }
    }

    public static void Solve()
    {
        var slots = File.ReadAllLines(@"input/day18_input.txt")
                        .Select(l => l.Split(','))
                        .Select(g => new Bit(int.Parse(g[1]), int.Parse(g[0])))
                        .ToList();

        Console.WriteLine($"Part 1: {Solve(slots.Take(Time).ToHashSet())}");
        Console.WriteLine($"Part 2: {FindFirstFailing(slots)}");
    }

    static Bit FindFirstFailing(List<Bit> slots)
    {
        var l = 0;
        var r = slots.Count - 1;
        while (l < r)
        {
            var m = (l + r) / 2;

            var s = Solve(slots.Take(m).ToHashSet());
            if (s == -1)
            {
                while (s == -1) s = Solve(slots.Take(m--).ToHashSet());
                return slots[m + 1];
            }
            else
            {
                l = m + 1;
            }
        }

        throw new Exception("No solution found!");
    }

    static int Solve(HashSet<Bit> slots)
    {
        var visited = new HashSet<Bit>();
        var queue = new PriorityQueue<Bit, int>();
        queue.Enqueue(Start, 0);

        while (queue.TryDequeue(out var bit, out var steps))
        {
            if (bit == End) return steps;

            if (visited.Contains(bit)) continue;
            visited.Add(bit);

            foreach (var next in bit.Walk())
            {
                if (!slots.Contains(next)) queue.Enqueue(next, steps + 1);
            }
        }

        return -1;
    }
}