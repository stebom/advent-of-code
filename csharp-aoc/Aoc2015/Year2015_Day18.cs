namespace AdventOfCode
{
    internal class Year2015_Day18
    {
        static int CountOn(char[][] grid, int r, int c)
        {
            var count = 0;
            if (r > 0 && c > 0 && grid[r - 1][c - 1] == '#') count++;
            if (r > 0 && grid[r - 1][c] == '#') count++;
            if (r > 0 && c < grid[0].Length - 1 && grid[r - 1][c + 1] == '#') count++;
            if (c < grid[0].Length - 1 && grid[r][c + 1] == '#') count++;
            if (r < grid.Length - 1 && grid[r + 1][c] == '#') count++;
            if (r < grid.Length - 1 && c < grid[0].Length - 1 && grid[r + 1][c + 1] == '#') count++;
            if (r < grid.Length - 1 && c > 0 && grid[r + 1][c - 1] == '#') count++;
            if (c > 0 && grid[r][c - 1] == '#') count++;
            return count;
        }

        static char[][] Copy(char[][] grid)
        {
            var cols = grid[0].Length;
            var copy = new char[grid.Length][];
            for (var r = 0; r < grid.Length; r++) copy[r] = new char[cols];
            return copy;
        }

        static void LightCorners(char[][] grid)
        {
            grid[0][0] = '#';
            grid[0][grid[0].Length - 1] = '#';
            grid[grid.Length - 1][0] = '#';
            grid[grid.Length - 1][grid[0].Length - 1] = '#';
        }

        public static void Solve()
        {
            const bool part2 = true;
            var grid = File.ReadAllLines(@"2015_18_input.txt").Select(line => line.ToCharArray()).ToArray();
            if (part2) LightCorners(grid);

            for (var step = 0; step < 100;  step++)
            {
                var next = Copy(grid);

                for (var r = 0; r < grid.Length; r++)
                {
                    for (var c = 0; c < grid[r].Length; c++)
                    {
                        var on = grid[r][c] == '#';
                        next[r][c] = CountOn(grid, r, c) switch
                        {
                             2 or 3 when on => '#',
                             3 when !on => '#',
                             _ => '.'
                        };
                    }
                }

                grid = next;
                if (part2) LightCorners(grid);
            }

            var count = 0;
            for (var r = 0; r < grid.Length; r++)
                for (var c = 0; c < grid[r].Length; c++)
                    count += grid[r][c] == '#' ? 1 : 0;

            Console.WriteLine($"Part 1: {count}");
        }
    }
}