namespace Aoc2024;

public static class Day21
{
    static readonly (int R, int C) Up       = (-1,0);
    static readonly (int R, int C) Right    = (0, 1);
    static readonly (int R, int C) Down     = (1, 0);
    static readonly (int R, int C) Left     = (0, -1);

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

    public static void Solve()
    {
        string[] inputs = ["208A", "586A", "341A", "463A", "593A"];
        //string[] inputs = ["029A", "980A", "179A", "456A", "379A"];

        long sum = 0;

        foreach (var input in inputs)
        {
            List<List<char>> solutions = [];
            foreach (var path in Search(NumericKeypad, new Queue<char>(input.ToCharArray())))
            {
                Search(solutions, path, 2);
            }

            Console.WriteLine($"{solutions.Min(s => s.Count)} * {int.Parse(input[0..3])}");
            sum += solutions.Min(s => s.Count) * int.Parse(input[0..3]);
        }

        Console.WriteLine($"Part 1: {sum}");
    }

    private static void Search(List<List<char>> solutions, List<char> path, int recurse)
    {
        if (recurse == 0)
        {
            solutions.Add(path);
            return;
        }

        foreach (var solution in Search(DirectionalKeypad, new Queue<char>(path)))
        {
            Search(solutions, solution, recurse - 1);
        }
    }

    static List<List<char>> Search(Dictionary<char, (int R, int C)> map, Queue<char> initial)
    {
        List<List<char>> solutions = [];
        var queue = new Queue<(List<char>, Queue<char> Q, char Current)>();
        queue.Enqueue(([], initial, 'A'));

        while (queue.TryDequeue(out var state))
        {
            var (path, q, current) = state;

            if (q.Count == 0)
            {
                solutions.Add(path);
                continue;
            }

            var next = q.Dequeue();
            foreach (var possible in Search([.. map.Values], map[current], map[next]))
            {
                var presses = ConstructPresses([map[current], .. possible]);
                queue.Enqueue(([.. path, .. presses], new Queue<char>(q), next));
            }
        }

        return solutions;
    }

    static List<List<(int R, int C)>> Search(List<(int R, int C)> valid, (int R, int C) from, (int R, int C) to)
    {
        var paths = new List<List<(int R, int C)>>();
        if (from == to) return [[]];

        var directions = new List<(int R, int C)>();
        if (from.R < to.R) directions.Add(Down);
        else if (from.R > to.R) directions.Add(Up);

        if (from.C < to.C) directions.Add(Right);
        else if (from.C > to.C) directions.Add(Left);

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
            current = next;
        }
        presses.Add('A');
        return presses;
    }
}