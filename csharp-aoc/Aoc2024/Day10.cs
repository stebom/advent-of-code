
using System.Data;
using System.IO;
using System.Threading.Tasks.Sources;
using static System.Net.Mime.MediaTypeNames;

namespace Aoc2024;

public static class Day10
{
    record struct Cell(int R, int C);

    public static void Solve()
    {
        var grid = File.ReadAllLines(@"input/day10_input.txt").Select(l => l.ToCharArray()).ToArray();

        var part1 = 0;
        var part2 = 0;

        for (var r = 0; r < grid.Length; r++)
        {
            for (var c = 0; c < grid[r].Length; c++)
            {
                if (grid[r][c] == '0')
                {
                    var (score, rating) = Search(grid, new(r, c));
                    part1 += score;
                    part2 += rating;
                }
            }
        }

        Console.WriteLine(part1);
        Console.WriteLine(part2);
    }
    private static readonly Cell[] Directions = [new(-1,0), new(0,1), new(1,0), new(0,-1)];

    private static (int,int) Search(char[][] grid, Cell start)
    {
        var queue = new Queue<Cell>();
        queue.Enqueue(start);

        Console.WriteLine($"Starting from {start}");

        var goals = new HashSet<Cell>();
        var score = 0;

        while (queue.Count > 0)
        {
            var cell = queue.Dequeue();
            var height = grid[cell.R][cell.C] - '0';
            if (height == 9)
            {
                goals.Add(cell);
                score++;
                continue;
            }

            foreach (var direction in Directions)
            {
                try
                {
                    var next = new Cell(cell.R + direction.R, cell.C + direction.C);
                    var nextHeight = grid[cell.R + direction.R][cell.C + direction.C] - '0';
                    if (nextHeight == height + 1)
                    {
                        queue.Enqueue(next);
                    }
                }
                catch { }
            }
        }

        return (goals.Count, score);
    }
}
