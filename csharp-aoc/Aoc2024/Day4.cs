namespace Aoc2024;

public static class Day4
{
    public static void Solve()
    {
        char[][] grid = File.ReadAllLines(@"Day4_input.txt").Select(x => x.ToCharArray()).ToArray();

        Part1(grid);
        Part2(grid);
    }

    private static void Part1(char[][] grid) => Console.WriteLine($"Part 1: {FindXmas(grid)}");

    private static void Part2(char[][] grid) => Console.WriteLine($"Part 2: {FindCrossMas(grid)}");

    private static int FindXmas(char[][] grid)
    {
        char[] needle = ['X', 'M', 'A', 'S'];
        int[][] scanDirections = [ [-1, 0], [-1, -1], [-1, 1], [ 0, -1],[ 0,  1],[ 1, 0], [ 1, -1], [ 1, 1] ];

        int rows = grid.Length;
        int cols = grid[0].Length;

        int count = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                foreach (var direction in scanDirections)
                {
                    count += ScanDirection(direction, grid, r, c, needle);
                }
            }
        }

        return count;
    }

    private static int ScanDirection(int[] direction, char[][] grid, int r, int c, char[] needle)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;
        int numSteps = needle.Length;
        var steps = 0;

        var buffer = new List<char>(numSteps);

        while (steps < numSteps)
        {
            var rd = r + steps * direction[0];
            var cd = c + steps * direction[1];

            if (rd < 0 || rd >= rows || cd < 0 || cd >= cols) { break; }

            buffer.Add(grid[rd][cd]);
            steps++;
        }

        return steps == numSteps && buffer.SequenceEqual(needle) ? 1 : 0;
    }

    private static int FindCrossMas(char[][] grid)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;

        var count = 0;
        for (int r = 1; r < rows - 1; r++)
        {
            for (int c = 1; c < cols - 1; c++)
            {
                if (grid[r][c] != 'A') continue;

                var cross = 0;
                cross += (grid[r - 1][c - 1] == 'M' && grid[r + 1][c + 1] == 'S') ? 1 : 0;
                cross += (grid[r + 1][c - 1] == 'M' && grid[r - 1][c + 1] == 'S') ? 1 : 0;
                cross += (grid[r - 1][c - 1] == 'S' && grid[r + 1][c + 1] == 'M') ? 1 : 0;
                cross += (grid[r - 1][c - 1] == 'S' && grid[r + 1][c + 1] == 'M') ? 1 : 0;
                if (cross > 1) count++;
            }
        }

        return count;
    }
}
