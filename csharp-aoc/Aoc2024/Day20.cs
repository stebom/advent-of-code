namespace Aoc2024;

public static class Day20
{
    record struct Cell(int R, int C);

    static readonly Cell[] Directions = [new(-1, 0), new(0, 1), new(1, 0), new(0, -1)];

    public static void Solve()
    {
        var grid = File.ReadAllLines(@"input/day20_input.txt").Select(l => l.ToCharArray()).ToArray();
        var (start, end) = FindNamedPositions(grid);

        var steps = Walk(grid, start, end);

        Part1(end, steps);
        Part2(end, steps);
    }

    private static void Part1(Cell end, Dictionary<Cell, int> steps)
    {
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

    private static void Part2(Cell end, Dictionary<Cell, int> steps)
    {
        var cheats = new Dictionary<int, int>();
        foreach (var current in steps.Keys)
        {
            foreach (var (cheat,saved) in GetCheats(steps, current, steps[end], 20))
            {
                if (!cheats.TryAdd(saved, 1))
                {
                    cheats[saved]++;
                }
            }
        }

        var sum = cheats.Where(kvp => kvp.Key >= 100).Sum(kvp => kvp.Value);

        Console.WriteLine($"Part 2: {sum}");
    }

    static IEnumerable<Cell> GetCheats(Dictionary<Cell, int> steps, Cell current, int totalDistance)
    {
        var distance = totalDistance - steps[current];

        foreach (var (dr, dc) in Directions)
        {
            var next = new Cell(current.R + dr, current.C + dc);

            if (!steps.ContainsKey(next))
            {
                var cheat = new Cell(current.R + (dr * 2), current.C + (dc * 2));
                if (steps.TryGetValue(cheat, out int cheatSteps))
                {
                    var cheatDistance = totalDistance - cheatSteps;
                    if (cheatDistance < distance) yield return cheat;
                }
            }
        }
    }

    static IEnumerable<(Cell,int)> GetCheats(Dictionary<Cell, int> steps, Cell current, int totalDistance, int range)
    {
        var distance = totalDistance - steps[current];

        for (var dr = current.R - range; dr <= current.R + range; dr++)
        {
            for (var dc = current.C - range; dc <= current.C + range; dc++)
            {
                var next = new Cell(dr, dc);
                if (current == next) continue;

                var distanceToCheatEnd = Distance(current, next);

                if (range < distanceToCheatEnd) continue;

                if (steps.TryGetValue(next, out int nextSteps))
                {
                    var cheatDistance = totalDistance - nextSteps + distanceToCheatEnd;
                    var saved = distance - cheatDistance;
                    if (saved > 0) yield return (next, saved);
                }
            }
        }
    }

    static Dictionary<Cell, int> Walk(char[][] grid, Cell start, Cell end)
    {
        HashSet<Cell> visited = [];
        Dictionary<Cell, int> steps = [];

        steps.Add(start, 0);

        var distance = 1;
        var current = start;

        while (current != end)
        {
            foreach (var (dr, dc) in Directions)
            {
                var next = new Cell(current.R + dr, current.C + dc);
                if (grid[next.R][next.C] is '#') continue;
                if (visited.Contains(next)) continue;

                visited.Add(current);
                steps.Add(next, distance++);
                current = next;
            }
        }

        return steps;
    }

    static (Cell Start, Cell End) FindNamedPositions(char[][] grid)
    {
        Cell start = default;
        Cell end = default;
        for (var r = 0; r < grid.Length; r++)
        {
            for (var c = 0; c < grid[r].Length; c++)
            {
                if (grid[r][c] == 'S') start = new(r, c);
                if (grid[r][c] == 'E') end = new(r, c);
            }
        }
        return (start, end);
    }

    static int Distance(Cell a, Cell b) => Math.Abs(a.R - b.R) + Math.Abs(a.C - b.C);
}