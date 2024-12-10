namespace Aoc2024;

public static class Day10
{
    private static readonly Cell[] Directions = [new(-1, 0), new(0, 1), new(1, 0), new(0, -1)];

    record struct Cell(int R, int C);

    public static void Solve()
    {
        var grid = File.ReadAllLines(@"input/day10_input.txt").Select(l => l.ToCharArray().Select(c => c - '0').ToArray()).ToArray();

        int part1 = 0;
        int part2 = 0;

        for (var r = 0; r < grid.Length; r++)
        {
            for (var c = 0; c < grid[r].Length; c++)
            {
                if (grid[r][c] == 0)
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

    private static (int,int) Search(int[][] grid, Cell start)
    {
        var goals = new List<Cell>();

        var queue = new Queue<Cell>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var cell = queue.Dequeue();
            var height = grid[cell.R][cell.C];

            if (height == 9)
            {
                goals.Add(cell);
                continue;
            }

            foreach (var direction in Directions)
            {
                var next = new Cell(cell.R + direction.R, cell.C + direction.C);
                if (next.R < 0 || next.R == grid.Length || next.C < 0 || next.C == grid[0].Length)
                {
                    continue;
                }

                if (grid[next.R][next.C] == height + 1)
                {
                    queue.Enqueue(next);
                }
            }
        }

        return (goals.Distinct().Count(), goals.Count);
    }
}
