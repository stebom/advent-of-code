namespace Day8;

public class Day8
{ 
    public static void Run()
    {

        var grid = File.ReadAllLines("input.txt").Select(c => c.Select(u => u - '0').ToArray()).ToArray();

        var numVisible = grid.Length * 2 + (grid.Length - 2) * 2; // sum edges

        var maxScenicScore = 0;

        Console.WriteLine(numVisible);

        for (var x = 1; x < grid.Length - 1; x++)
        {
            for (var y = 1; y < grid.Length - 1; y++)
            {
                var currentScore = ScenicScore(grid, x, y);
                if (currentScore > maxScenicScore)
                    maxScenicScore = currentScore;

                //if (IsVisible(grid, x, y))
                //    numVisible++;
            }
        }

        PrintGrid(grid);
        Console.WriteLine(maxScenicScore);
    }
    bool IsVisible(int[][] g, int x, int y)
    {
        var h = g[x][y];

        var left = AllSmallerThan(g, x - 1, y, -1, 0, h);
        var up = AllSmallerThan(g, x, y - 1, 0, -1, h);
        var right = AllSmallerThan(g, x + 1, y, 1, 0, h);
        var down = AllSmallerThan(g, x, y + 1, 0, 1, h);

        var isVisible = left || up || right || down;
        Console.WriteLine($"{x},{y} {h} left:{left} up:{up} right:{right} down:{down} visible:{isVisible}");

        return isVisible;
    }

    public static bool AllSmallerThan(int[][] g, int x, int y, int x_vec, int y_vec, int height)
    {
        var atEdge = x == 0 || x == g.Length - 1 || y == 0 || y == g.Length - 1;

        if (g[x][y] >= height)
            return false;

        if (!atEdge)
            return AllSmallerThan(g, x += x_vec, y += y_vec, x_vec, y_vec, height);

        return height > g[x][y];
    }

    public static int ScenicScore(int[][] g, int x, int y)
    {
        var h = g[x][y];

        var left = ViewDistance(g, x - 1, y, -1, 0, h);
        var up = ViewDistance(g, x, y - 1, 0, -1, h);
        var right = ViewDistance(g, x + 1, y, 1, 0, h);
        var down = ViewDistance(g, x, y + 1, 0, 1, h);

        var scenicScore = left * up * right * down;
        Console.WriteLine($"{x},{y} {h} left:{left} up:{up} right:{right} down:{down} scenicScore:{scenicScore}");

        return scenicScore;
    }

    public static int ViewDistance(int[][] g, int x, int y, int x_vec, int y_vec, int height, int distance = 1)
    {
        var atEdge = x == 0 || x == g.Length - 1 || y == 0 || y == g.Length - 1;

        if (g[x][y] >= height || atEdge)
            return distance;

        return ViewDistance(g, x += x_vec, y += y_vec, x_vec, y_vec, height, distance + 1);
    }

    public static void PrintGrid(int[][] g)
    {
        foreach (var chunk in g)
        {
            foreach (var c in chunk)
                Console.Write(c);
            Console.WriteLine();
        }
    }
}