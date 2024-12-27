namespace Aoc2024;

public static class Day20
{
    record struct Cell(short R, short C);

    static readonly Cell[] Directions = [new(-1, 0), new(0, 1), new(1, 0), new(0, -1)];

    public static void Solve()
    {
        var grid = File.ReadAllLines(@"input/day20_input.txt").Select(l => l.ToCharArray()).ToArray();
        var (start,end) = FindNamedPositions(grid);

        var (connections, steps) = Walk(grid, start, end);

        var cheats = new Dictionary<int, int>();
        foreach (var current in steps.Keys)
        {
            foreach (var cheat in GetCheats(steps, current, steps[end]))
            {
                var saved = steps[cheat] - steps[current] - 2;
                if (!cheats.TryAdd(saved, 1))
                {
                    cheats[saved]++;
                }
            }
        }

        var sum = cheats.Where(kvp => kvp.Key >= 100).Sum(kvp => kvp.Value);

        Console.WriteLine($"Part 1: {sum}");
    }

    static IEnumerable<Cell> GetCheats(Dictionary<Cell, int> steps, Cell current, int totalDistance)
    {
        var distance = totalDistance - steps[current];

        foreach (var (dr, dc) in Directions)
        {
            var next = new Cell((short)(current.R + dr), (short)(current.C + dc));

            // Check if next is a wall
            if (!steps.ContainsKey(next))
            {
                var cheat = new Cell((short)(current.R + dr * 2), (short)(current.C + dc * 2));
                if (steps.TryGetValue(cheat, out int cheatSteps))
                {
                    var cheatDistance = totalDistance - cheatSteps;
                    if (cheatDistance < distance) yield return cheat;
                }
            }
        }
    }

    static (Dictionary<Cell, Cell> Connections, Dictionary<Cell, int> Steps) Walk(char[][] grid, Cell start, Cell end)
    {
        Dictionary<Cell, Cell> connections = [];
        Dictionary<Cell, int> steps = [];

        steps.Add(start, 0);

        var distance = 1;
        var current = start;

        while (current != end)
        {
            foreach (var (dr, dc) in Directions)
            {
                var next = new Cell((short)(current.R + dr), (short)(current.C + dc));
                if (grid[next.R][next.C] is '#' || connections.ContainsKey(next)) continue;

                connections.Add(current, next);
                steps.Add(next, distance++);
                current = next;
            }
        }

        return (connections, steps);
    }

    static (Cell Start, Cell End) FindNamedPositions(char[][] grid)
    {
        Cell start = default;
        Cell end = default;
        for (short r = 0; r < grid.Length; r++)
        {
            for (short c = 0; c < grid[r].Length; c++)
            {
                if (grid[r][c] == 'S') start = new(r, c);
                if (grid[r][c] == 'E') end = new(r, c);
            }
        }
        return (start, end);
    }
}