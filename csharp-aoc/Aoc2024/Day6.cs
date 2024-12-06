namespace Aoc2024;

public static class Day6
{
    public static void Solve()
    {
        var grid = File.ReadAllLines($"day6_input.txt").Select(l => l.ToCharArray()).ToArray();
        var (guard, face) = FindStart(grid);

        Part1(grid, guard, face);
        Part2(grid, guard, face);
    }

    public static void Part1(char[][] grid, (int R, int C) guard, char face)
    {
        var visits = new HashSet<(int R, int C)>();

        while (true)
        {
            var next = Next(guard, face);
            if (next.R < 0 || next.R >= grid.Length ||
                next.C < 0 || next.C >= grid[0].Length)
            {
                break;
            }

            if (grid[next.R][next.C] == '#')
            {
                face = Turn(face);
            }
            else
            {
                guard = next;
            }

            visits.Add(guard);
        }

        Console.WriteLine($"Part 1: {visits.Count}");
    }

    public static void Part2(char[][] grid, (int R, int C) guard, char face)
    {
        var count = 0;
        for (int r = 0; r < grid.Length; r++)
        {
            for (int c = 0; c < grid[r].Length; c++)
            {
                if ((r, c) != guard && grid[r][c] == '.')
                {
                    grid[r][c] = '#';

                    if (DetectLoop(grid, guard, face)) count++;

                    grid[r][c] = '.';
                }
            }
        }

        Console.WriteLine($"Part 1: {count}");
    }

    public static bool DetectLoop(char[][] grid, (int R, int C) guard, char face)
    {
        var visits = new HashSet<((int R, int C), char Face)>();

        while (true)
        {
            var next = Next(guard, face);

            if (next.R < 0 || next.R >= grid.Length ||
                next.C < 0 || next.C >= grid[0].Length)
            {
                break;
            }

            if (grid[next.R][next.C] == '#')
            {
                face = Turn(face);
            }
            else
            {
                guard = next;
            }

            if (!visits.Add((guard, face)))
            {
                return true;
            }
        }

        return false;
    }

    private static ((int R, int C), char Face) FindStart(char[][] grid)
    {
        for (int r = 0; r < grid.Length; r++)
            for (int c = 0; c < grid[r].Length; c++)
                if (grid[r][c] != '.' && grid[r][c] != '#') return ((r, c), grid[r][c]);

        throw new Exception("Guard not found");
    }

    private static (int R, int C) Next((int R, int C) guard, char face)
    {
        var dir = face switch
        {
            '^' => (-1,  0),
            '<' => ( 0, -1),
            '>' => ( 0,  1),
            'v' => ( 1,  0),
            _ => throw new NotImplementedException(),
        };
        return (guard.R + dir.Item1, guard.C + dir.Item2);
    }

    private static char Turn(char face) => face switch
        {
            '^' => '>', '<' => '^', '>' => 'v', 'v' => '<',
            _ => throw new NotImplementedException(),
        };

}
