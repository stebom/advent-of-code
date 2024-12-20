using System.Reflection.Metadata.Ecma335;

namespace Aoc2024;

public static class Day20
{
    record struct Cell(short R, short C);
    record struct State(Cell Cell, int Steps);
    record struct Cheat(Cell Start, Cell End);

    record struct CheatState(Cell Cell, Cell? CheatStart, Cell? CheatEnd);

    static readonly Cell[] Directions = [new(-1, 0), new(0, 1), new(1, 0), new(0, -1)];

    public static void Solve()
    {
        var grid = File.ReadAllLines(@"input/day20_input.txt").Select(l => l.ToCharArray()).ToArray();

        var start = Find(grid, 'S');
        var end = Find(grid, 'E');

        const int shortcut = 100;
       
        var reference = Solve(grid, start, end);

        Console.WriteLine($"Reference is {reference} steps");

        var cheats = SolveWithCheats(grid, start, end, reference - shortcut + 1);

        Console.WriteLine($"Number of cheats that shortcuts {shortcut} steps is {cheats}");
    }


    static int Solve(char[][] grid, Cell start, Cell end)
    {
        var queue = new PriorityQueue<Cell, int>();
        queue.Enqueue(start, 0);

        var visited = new HashSet<Cell>();

        while (queue.TryDequeue(out var cell, out var steps))
        {
            if (cell == end) return steps;

            if (visited.Contains(cell)) continue;
            visited.Add(cell);

            foreach (var next in Next(grid, cell))
            {
                if (grid[next.R][next.C] == '#') continue;
                queue.Enqueue(next, steps + 1);
            }
        }

        throw new Exception("No solution found");
    }

    static int SolveWithCheats(char[][] grid, Cell start, Cell end, int maxSteps)
    {
        var queue = new PriorityQueue<CheatState, int>();
        queue.Enqueue(new(start, null, null), 0);

        var visited = new HashSet<CheatState>();
        var cheats = new HashSet<Cheat>();

        while (queue.TryDequeue(out var state, out var steps))
        {
            if (steps > maxSteps) continue;
            if (state.Cell == end)
            {
                if (state.CheatStart != null && state.CheatEnd != null)
                {
                    Console.WriteLine($"  Found cheat shortcutting {maxSteps - steps} steps at {state.CheatStart} -> {state.CheatEnd}");
                    cheats.Add(new(state.CheatStart.Value, state.CheatEnd.Value));
                }
                continue;
            }

            if (visited.Contains(state)) continue;
            visited.Add(state);

            foreach (var next in Next(grid, state.Cell))
            {
                if (grid[next.R][next.C] == '#')
                {
                    if (state.CheatStart == null)
                    {
                        queue.Enqueue(new(next, next, state.CheatEnd), steps + 1);
                    }
                }
                else
                {
                    if (state.CheatStart != null && state.CheatEnd == null)
                    {
                        queue.Enqueue(new(next, state.CheatStart, next), steps + 1);
                    }
                    else
                    {
                        queue.Enqueue(new(next, state.CheatStart, next), steps + 1);
                    }
                }
            }
        }
        
        return cheats.Count;
    }

    static IEnumerable<Cell> Next(char[][] grid, Cell cell)
    {
        foreach (var (dr, dc) in Directions)
        {
            short r = (short)(cell.R + dr);
            short c = (short)(cell.C + dc);
            if (r < 0 || r >= grid.Length || c < 0 || c >= grid[0].Length) continue;
            yield return new(r, c);
        }
    }

    static Cell Find(char[][] grid, char needle)
    {
        for (short r = 0; r < grid.Length; r++)
        {
            for (short c = 0; c < grid[r].Length; c++)
            {
                if (grid[r][c] == needle)
                {
                    grid[r][c] = '.';
                    return new(r, c);
                }
            }
        }
        throw new Exception("Could not find needle");
    }
}