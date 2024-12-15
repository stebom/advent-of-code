
using System.Runtime.CompilerServices;

namespace Aoc2024;

public static class Day15
{
    const char Robot = '@';
    const char Wall = '#';
    const char Box = 'O';
    const char LeftBox = '[';
    const char RightBox = ']';
    const char Free = '.';
    const char Up = '^';
    const char Right = '>';
    const char Down ='v';
    const char Left = '<';

    record struct Cell(int R, int C);

    static readonly Cell GoUp       = new(-1,  0);
    static readonly Cell GoRight    = new( 0,  1);
    static readonly Cell GoDown     = new( 1,  0);
    static readonly Cell GoLeft     = new( 0, -1);

    static readonly Cell[] Directions = [GoUp, GoRight, GoDown, GoLeft];

    public static void Solve()
    {
        var input = File.ReadAllLines(@"input/day15_input.txt");
        var grid = input[0 .. 50].Select(l => l.ToCharArray()).ToArray();
        var directions = input[51..].SelectMany(l => l).Select(ToDirection).ToArray();

        Console.WriteLine($"Part 1: {Part1(grid.Copy(), directions)}");
        Console.WriteLine($"Part 2: {Part2(grid.Expand(), directions)}");
    }

    static long Part1(char[][] grid, Cell[] directions)
    {
        var robot = FindRobot(grid);

        foreach (var direction in directions)
        {
            if (FindSpace(grid, robot, direction, out var boxes))
            {
                var next = robot.Move(direction);
                grid[robot.R][robot.C] = Free;
                grid[next.R][next.C] = Robot;

                var box = next.Move(direction);
                for (var i = 1; i <= boxes; i++)
                {
                    grid[box.R][box.C] = Box;
                    box = box.Move(direction);
                }

                robot = next;
            }
        }

        return grid.Sum();
    }

    static long Part2(char[][] grid, Cell[] directions)
    {
        var robot = FindRobot(grid);

        foreach (var direction in directions)
        {
            HashSet<Cell> move = [];
            FindMovableCells(grid, move, robot, direction);

            if (move.Any(m => grid.What(m) is Wall))
            {
                continue;
            }

            var objects = move.ToDictionary(k => k, v => grid.What(v));

            foreach (var cell in move) grid[cell.R][cell.C] = '.';

            foreach (var cell in move)
            {
                var next = cell.Move(direction);
                grid[next.R][next.C] = objects[cell];
            }

            robot = robot.Move(direction);    
        }

        return grid.Sum();
    }

    static char[][] Copy(this char[][] grid) => grid.Select(g => g.ToArray()).ToArray();

    static char[][] Expand(this char[][] grid)
    {
        var expanded = new char[grid.Length][];
        
        for (var r = 0; r < grid.Length; r++)
        {
            expanded[r] = new char[grid[r].Length * 2];

            for (var c = 0; c < grid[r].Length; c++)
            {
                char[] cells = grid[r][c] switch
                {
                    '#' => ['#', '#'],
                    'O' => ['[', ']'],
                    '.' => ['.', '.'],
                    '@' => ['@', '.'],
                    _ => throw new Exception($"Can't expand {grid[r][c]}")
                };

                (expanded[r][c * 2], expanded[r][(c * 2) + 1]) = (cells[0], cells[1]);
            }
        }

        return expanded;
    }

    static void PrintGrid(char[][] grid)
    {
        for (var r = 0; r < grid.Length; r++)
        {
            for (var c = 0; c < grid[r].Length; c++)
            { 
                Console.Write(grid[r][c]);
            }
            Console.WriteLine();
        }
    }

    static long Sum(this char[][] grid)
    {
        long sum = 0;
        for (var r = 0; r < grid.Length; r++)
            for (var c = 0; c < grid[r].Length; c++)
                if (grid[r][c] is Box or LeftBox) sum += r * 100 + c;

        return sum;
    }

    static Cell FindRobot(char[][] grid)
    {
        for (var r = 0; r < grid.Length; r++)
            for (var c = 0; c < grid[r].Length; c++)
                if (grid[r][c] == Robot) return new(r, c);

        throw new Exception("Robot not found!");
    }

    static bool FindSpace(char[][] grid, Cell cell, Cell direction, out int boxes)
    {
        boxes = 0;
        var obj = grid[cell.R][cell.C];

        while (obj is not Free)
        {
            cell = cell.Move(direction);
            obj = grid.What(cell);

            if (obj is Wall) return false;
            if (obj is Box) boxes++;
        }

        return true;
    }

    static void FindMovableCells(char[][] grid, HashSet<Cell> cells, Cell current, Cell direction)
    {
        if (cells.Contains(current) || grid.What(current) is Free) return;

        cells.Add(current);

        if (grid.What(current) is Box or Robot)
        {
            FindMovableCells(grid, cells, current.Move(direction), direction);
        }

        var verticalMove = direction is { R: -1 } or { R: 1 };

        if (grid.What(current) is LeftBox)
        {
            FindMovableCells(grid, cells, current.Move(direction), direction);
            if (verticalMove) FindMovableCells(grid, cells, current.Move(GoRight), direction);
        }
        if (grid.What(current) is RightBox)
        {
            FindMovableCells(grid, cells, current.Move(direction), direction);
            if (verticalMove) FindMovableCells(grid, cells, current.Move(GoLeft), direction);
        }
    }

    static char What(this char[][] grid, Cell cell) => grid[cell.R][cell.C];

    static Cell Move(this Cell cell, Cell dir) => new(cell.R + dir.R, cell.C + dir.C);

    static Cell ToDirection(char c) => c switch {
        Up => Directions[0], Right => Directions[1], Down => Directions[2], Left => Directions[3],
        _ => throw new Exception($"Can't convert {c} to direction!")
    };
}