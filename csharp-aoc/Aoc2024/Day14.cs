using System.Diagnostics;
using System.Text;

namespace AdventOfCode;

internal class Day14
{
    record Direction(int R, int C);

    static readonly Direction North     = new (-1, +0);
    static readonly Direction East      = new (+0, +1);
    static readonly Direction South     = new (+1, +0);
    static readonly Direction West      = new (+0, -1);

    static readonly Direction[] Directions = [ North, West, South, East ];

    static readonly string[] Input = [
        "O....#....",
        "O.OO#....#",
        ".....##...",
        "OO.#O....O",
        ".O.....O#.",
        "O.#..O.#.#",
        "..O..#O..O",
        ".......O..",
        "#....###..",
        "#OO..#....",
    ];

    static void TiltNorth(char[][] grid)
    {
        Debug.Assert(grid.All(r => r.Length == grid[0].Length), "All rows must be of equal size");

        for (var r = 1; r < grid.Length; r++)
        {
            for (var c = 0; c < grid[r].Length; c++)
            {
                if (grid[r][c] == 'O')
                {
                    var row = r;
                    while (row > 0 && grid[row - 1][c] == '.')
                    {
                        grid[row - 1][c] = 'O';
                        grid[row][c] = '.';
                        row--;
                    }
                }
            }
        }
    }

    static void Tilt(char[][] grid, Direction direction)
    {
        Debug.Assert(grid.All(r => r.Length == grid[0].Length), "All rows must be of equal size");

        restart:
        for (var r = 0; r < grid.Length; r++)
        {
            for (var c = 0; c < grid[r].Length; c++)
            {
                if (grid[r][c] == 'O')
                {
                    var deltaR = r + direction.R;
                    var deltaC = c + direction.C;

                    if (deltaR >= 0 && deltaR < grid.Length &&
                        deltaC >= 0 && deltaC < grid[r].Length &&
                        grid[deltaR][deltaC] == '.')
                    {
                        grid[deltaR][deltaC] = 'O';
                        grid[r][c] = '.';
                        goto restart;
                    }
                }
            }
        }
    }

    static int ComputeLoad(char[][] grid)
    {
        var load = 0;
        for (var r = 0; r < grid.Length; r++)
        {
            var factor = grid.Length - r;
            load += factor * grid[r].Count(c => c == 'O');
        }

        return load;
    }

    static int Hash(char[][] grid)
    {
        var sb = new StringBuilder();
        for (var r = 0; r < grid.Length; r++)
        {
            sb.Append(grid[r]);
        }
        return sb.ToString().GetHashCode();
    }

    static void PrintGrid(char[][] grid)
    {
        foreach (var row in grid)
        {
            Console.WriteLine(new string(row));
        }
    }
    
    public static void SolvePart1()
    {
        //var grid = Input.Select(r => r.ToCharArray()).ToArray();
        var grid = File.ReadAllLines(@"2023_14_input.txt").Select(r => r.ToCharArray()).ToArray();

        TiltNorth(grid);
        var load = ComputeLoad(grid);
        PrintGrid(grid);

        Console.WriteLine($"Part 1: {load}");
    }

    public static void SolvePart2()
    {
        //var grid = Input.Select(r => r.ToCharArray()).ToArray();
        var grid = File.ReadAllLines(@"2023_14_input.txt").Select(r => r.ToCharArray()).ToArray();

        var seen = new Dictionary<int,int>();
        var hashes = new int[4];

        const int cycles = 1_000_000_000;
        for (var i = 0; i < cycles; i++)
        {
            Tilt(grid, North);
            hashes[0] = Hash(grid);
            Tilt(grid, West);
            hashes[1] = Hash(grid);
            Tilt(grid, South);
            hashes[2] = Hash(grid);
            Tilt(grid, East);
            hashes[3] = Hash(grid);

            var hash = (hashes[0], hashes[1], hashes[2], hashes[3]).GetHashCode();

            if (ComputeLoad(grid) == 102509)
            {
                Console.WriteLine($"Cycle {i} has the same load as {cycles}");
            }

            if (seen.ContainsKey(hash))
            {
                Console.WriteLine($"Cycle at {i} last seen {seen[hash]} ({hash}) with a load of {ComputeLoad(grid)}");
                i = cycles - (cycles - i) % (i - seen[hash]);
                Console.WriteLine($"Jumping to {i}");
            }
            else
            {
                seen[hash] = i;
            }
        }

        var load = ComputeLoad(grid);
        PrintGrid(grid);

        Console.WriteLine($"Part 2: {load}");
    }
}
