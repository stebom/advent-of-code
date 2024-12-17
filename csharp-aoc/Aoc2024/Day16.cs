using System.Data;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Aoc2024;

public static class Day16
{
    public static void Solve()
    {
        var grid = File.ReadAllLines(@"input/day16_input.txt").Select(l => l.ToCharArray()).ToArray();

        var start = Find(grid, 'S');
        var end = Find(grid, 'E');

        Console.WriteLine($"Part 1: {FindBestScore(grid, start, end)}");
        //Console.WriteLine($"Part 2: {FindPaths(grid, start, end, FindBestScore(grid, start, end)).Count()}");
    }

    record struct Cell(int R, int C);
    record State(Cell Position, Cell Direction);

    static readonly Cell North  = new(-1, 0);
    static readonly Cell East   = new( 0, 1);
    static readonly Cell South  = new( 1, 0);
    static readonly Cell West   = new( 0,-1);

    static long FindBestScore(char[][] grid, Cell start, Cell end)
    {
        var queue = new PriorityQueue<State, long>();
        queue.Enqueue(new State(start, East), 0);

        var distances = new Dictionary<Cell, long> { { start, 0} };
        var prev = new Dictionary<Cell,HashSet<Cell>>();

        while (queue.TryDequeue(out var state, out var score))
        {
            if (state.Position == end)
            {
                var paths = GetPaths(start, end, prev);
                Console.WriteLine($"Unique cells: {paths.Count}");

                for (var r = 0; r < grid.Length; r++)
                {
                    for (var c = 0; c < grid[r].Length; c++)
                    {
                        Console.Write(paths.Contains(new(r,c)) ? 'O' : grid[r][c]);
                    }
                    Console.WriteLine();
                }
                return score;
            }

            Console.WriteLine($"Walking {state.Position}");

            foreach (var next in state.Position.Walk(North, East, South, West))
            {
                if (grid[next.Position.R][next.Position.C] == '#') continue;
                if (!distances.ContainsKey(next.Position)) distances[next.Position] = long.MaxValue;
                if (!prev.ContainsKey(next.Position)) prev[next.Position] = [];

                var nextScore = Score(state.Direction, next.Direction) + score;

                Console.WriteLine($"  -> {next.Position} ({nextScore})");

                if (nextScore <= distances[next.Position])
                {
                    distances[next.Position] = nextScore;
                    prev[next.Position].Add(state.Position);
                    queue.Enqueue(next, nextScore);
                }
            }
        }

        return -1;
    }

    private static HashSet<Cell> GetPaths(Cell start, Cell end, Dictionary<Cell, HashSet<Cell>> prev)
    {
        var queue = new Queue<Cell>();
        queue.Enqueue(end);

        var visited = new HashSet<Cell>();
        
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            visited.Add(current);

            if (current == start) continue;

            Console.WriteLine($"{current} -> {prev[current].Count}");

            foreach (var parent in prev[current])
            {
                Console.WriteLine($"{current} -> {parent}");
                queue.Enqueue(parent);
            }
        }

        return visited;
    }

    static long Score(Cell A, Cell B)
    {
        if (A == B) return 1;
        if ((A.R + B.R, A.C + B.C) == (0, 0)) return 2001;
        return 1001;
    }

    static IEnumerable<State> Walk(this Cell cell, params Cell[] directions) => directions.Select(direction => cell.Walk(direction));

    static State Walk(this Cell cell, Cell direction) => new(new(cell.R + direction.R, cell.C + direction.C),direction);

    static Cell Find(char[][] grid, char needle)
    {
        for (var r = 0; r < grid.Length; r++)
        {
            for (var c = 0; c < grid[r].Length; c++)
            {
                if (grid[r][c] == needle) return new Cell(r, c);
            }
        }
        throw new Exception($"Can't find in grid {needle}");
    }
}