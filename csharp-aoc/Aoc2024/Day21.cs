namespace Aoc2024;

public static class Day21
{
    static readonly (int R, int C) Up       = (-1,  0);
    static readonly (int R, int C) Right    = ( 0,  1);
    static readonly (int R, int C) Down     = ( 1,  0);
    static readonly (int R, int C) Left     = ( 0, -1);

    static readonly Dictionary<char, (int R, int C)> NumericKeypad = new()
    {
        { '7', (0,0) }, { '8', (0,1) }, { '9', (0,2) },
        { '4', (1,0) }, { '5', (1,1) }, { '6', (1,2) },
        { '1', (2,0) }, { '2', (2,1) }, { '3', (2,2) },
                        { '0', (3,1) }, { 'A', (3,2) },
    };

    static readonly Dictionary<char, (int R, int C)> DirectionalKeypad = new()
    {
                        { '^', (0,1) }, { 'A', (0,2) },
        { '<', (1,0) }, { 'v', (1,1) }, { '>', (1,2) },
    };

    static readonly List<(int R, int C)> ValidNumericKeyPositions = [.. NumericKeypad.Values];
    static readonly List<(int R, int C)> ValidDirectionalKeyPositions = [.. DirectionalKeypad.Values];

    private static readonly Dictionary<(Dictionary<char, (int R, int C)>, (int R, int C), (int R, int C), int), long> Cache = [];

    public static void Solve()
    {
        string[] inputs = ["208A", "586A", "341A", "463A", "593A"];

        Console.WriteLine($"Part 1: {inputs.Sum(input => GetComplexity(input, 2))}");
        Console.WriteLine($"Part 2: {inputs.Sum(input => GetComplexity(input, 25))}");
    }

    static long GetComplexity(string sequence, int depth)
    {
        Console.Write($"Calculating the level {depth} complexity of {sequence}... ");

        var total = 0L;
        var start = NumericKeypad['A'];
        var number = int.Parse(sequence[0..3]);

        foreach (var end in sequence.Select(key => NumericKeypad[key]))
        {
            total += GetSequenceCost(start, end, NumericKeypad, depth + 1);
            start = end;            
        }

        Console.WriteLine($"{total} * {number}");

        return total * number;
    }

    static long GetSequenceCost((int R, int C) start, (int R, int C) end, Dictionary<char, (int R, int C)> map, int depth)
    {
        if (Cache.TryGetValue((map, start, end, depth), out var cached))
        {
            return cached;
        }

        var validPositions = map == NumericKeypad ? ValidNumericKeyPositions : ValidDirectionalKeyPositions;

        var cost = long.MaxValue;

        var queue = new Queue<((int R, int C) Pos, string Sequence)>();
        queue.Enqueue((start, ""));

        while (queue.TryDequeue(out var state))
        {
            var (pos, sequence) = state;

            if (pos == end)
            {
                cost = Math.Min(cost, GetCost(sequence + 'A', depth - 1));
                continue;
            }

            foreach (var possible in Search(validPositions, pos, end))
            {
                var presses = ConstructPresses([pos, .. possible]);
                queue.Enqueue((end, string.Concat(sequence, string.Concat(presses))));
            }
        }

        return Cache[(map, start, end, depth)] = cost;
    }

    static long GetCost(string sequence, int depth)
    {
        if (depth == 0) { return sequence.Length; }

        long cost = 0;
        var current = DirectionalKeypad['A'];

        foreach (var next in sequence.Select(n => DirectionalKeypad[n]))
        {
            cost += GetSequenceCost(current, next, DirectionalKeypad, depth);
            current = next;
        }

        return cost;
    }

    static List<List<(int R, int C)>> Search(List<(int R, int C)> valid, (int R, int C) from, (int R, int C) to)
    {
        if (from == to)
        {
            throw new ArgumentException($"from and to must be different positions");
        }

        var directions = new List<(int R, int C)>();
        if (from.R < to.R) directions.Add(Down);
        else if (from.R > to.R) directions.Add(Up);

        if (from.C < to.C) directions.Add(Right);
        else if (from.C > to.C) directions.Add(Left);

        var paths = new List<List<(int R, int C)>>();

        var queue = new Queue<(List<(int R, int C)>, (int R, int C))>();
        queue.Enqueue(([], from));

        while (queue.TryDequeue(out var state))
        {
            var (path, current) = state;
            if (current == to)
            {
                paths.Add(path);
                continue;
            }

            foreach (var (r, c) in directions)
            {
                (int R, int C) next = (current.R + r, current.C + c);
                if (!valid.Contains(next)) continue;

                queue.Enqueue(([.. path, next], next));
            }
        }

        return paths;
    }

    static List<char> ConstructPresses(List<(int R, int C)> path)
    {
        List<char> presses = [];

        var current = path.First();
        foreach (var next in path.Skip(1))
        {
            if (current.R < next.R) presses.Add('v');
            else if (current.R > next.R) presses.Add('^');
            else if (current.C < next.C) presses.Add('>');
            else if (current.C > next.C) presses.Add('<');
            else throw new Exception($"Bad direction {current} -> {next}");

            current = next;
        }

        return presses;
    }
}