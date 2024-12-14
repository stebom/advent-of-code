using System.Drawing.Imaging;
using System.Linq;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;

namespace Aoc2024;

public static class Day12
{
    public record Cell(int R, int C);

    public static void Solve()
    {
        var grid = File.ReadAllLines(@"input/day12_input.txt").Select(l => l.ToCharArray()).ToArray();

        var visited = new HashSet<Cell>();
        var regions = new List<HashSet<Cell>>();

        long part1 = 0;
        long part2 = 0;
        for (var r = 0; r < grid.Length; r++)
        {
            for (var c = 0; c < grid[0].Length; c++)
            {
                var cell = new Cell(r, c);

                if (visited.Contains(cell)) continue;

                var region = GetRegion(grid, cell);
                foreach (var plot in region) visited.Add(plot);
                regions.Add(region);

                var area = region.Count;
                var perimeter = CalculatePerimeter(region);
                var sides = CalculateSides(region);
                part1 += area * perimeter;
                part2 += area * sides;

                //Console.WriteLine($"(Part 1) A region of {grid[cell.R][cell.C]} plants with price {area} * {perimeter} = {area * perimeter}");
                //Console.WriteLine($"(Part 2) A region of {grid[cell.R][cell.C]} plants with price {area} * {sides} = {area * sides}");
            }
        }

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    private static HashSet<Cell> GetRegion(char[][] grid, Cell cell)
    {
        var crop = grid[cell.R][cell.C];
        var region = new HashSet<Cell>();
        GetAllAdjacent(grid, crop, region, cell.R, cell.C);
        return region;
    }

    private static void GetAllAdjacent(char[][] grid, char crop, HashSet<Cell> plots, int r, int c)
    {
        if (r < 0 || r >= grid.Length || c < 0 || c >= grid[0].Length) return;
        if (grid[r][c] != crop) return;

        var cell = new Cell(r, c);
        if (plots.Contains(cell)) return;

        plots.Add(cell);
        GetAllAdjacent(grid, crop, plots, r + 1, c);
        GetAllAdjacent(grid, crop, plots, r, c + 1);
        GetAllAdjacent(grid, crop, plots, r - 1, c);
        GetAllAdjacent(grid, crop, plots, r, c - 1);
    }

    private static int CalculatePerimeter(HashSet<Cell> plots)
    {
        var count = 0;

        foreach (var plot in plots)
        {
            count += (plots.Contains(new(plot.R + 1, plot.C)) ? 0 : 1) +
                     (plots.Contains(new(plot.R, plot.C + 1)) ? 0 : 1) +
                     (plots.Contains(new(plot.R - 1, plot.C)) ? 0 : 1) +
                     (plots.Contains(new(plot.R, plot.C - 1)) ? 0 : 1);
        }
        return count;
    }

    record class Neighbour(int R, int C, int S)
    {
        public bool Visited;
    }

    private static int CalculateSides(HashSet<Cell> plots)
    {
        var neighbours = plots
            .SelectMany(plot => (Neighbour[])[ new Neighbour(plot.R - 1, plot.C, 0),
                                               new Neighbour(plot.R + 1, plot.C, 1),
                                               new Neighbour(plot.R, plot.C + 1, 2),
                                               new Neighbour(plot.R, plot.C - 1, 3)
                                             ])
            .Where(n => !plots.Contains(new(n.R,n.C)))
            .OrderBy(n => n.R)
            .ThenBy(n => n.C)
            .ToList();

        var sides = 0;
        while (neighbours.Any(n => !n.Visited))
        {
            var neighbour = neighbours.First(n => !n.Visited);
            neighbour.Visited = true;
            sides++;

            if (neighbour.S is 0 or 1) Grow(neighbours, neighbour, 0, 1);
            else if (neighbour.S is 2 or 3) Grow(neighbours, neighbour, 1, 0);
        }

        return sides;
    }

    private static void Grow(List<Neighbour> all, Neighbour current, int r, int c)
    {
        current.Visited = true;

        var next = all.FirstOrDefault(n => n.R == current.R + r && n.C == current.C + c && n.S == current.S);
        if (next != null) Grow(all, next, r, c);
    }
}