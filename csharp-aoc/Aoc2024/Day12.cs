using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using static System.Net.Mime.MediaTypeNames;

namespace Aoc2024;

public static class Day12
{
    public record Cell(int R, int C);

    public static void Solve()
    {
        var grid = File.ReadAllLines(@"input/day12_input.txt").Select(l => l.ToCharArray()).ToArray();
        var printRegion = false;

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
                Console.WriteLine($"(Part 2) A region of {grid[cell.R][cell.C]} plants with price {area} * {sides} = {area * sides}");

                if (printRegion) PrintRegion(region, grid[cell.R][cell.C]);
            }
        }

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    private static void PrintRegion(HashSet<Cell> region, char plant)
    {
        var (rMin, rMax) = (region.Min(r => r.R), region.Max(r => r.R));
        var (cMin, cMax) = (region.Min(r => r.C), region.Max(r => r.C));

        Console.WriteLine($"---- Region of {plant} ----");

        for (var r = rMin; r <= rMax; r++)
        {
            for (var c = cMin; c <= cMax; c++)
            {
                Console.Write(region.Contains(new Cell(r, c)) ? plant : ' ');
            }
            Console.WriteLine();
        }
        Console.WriteLine("-----------------");
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

    private static Cell[] Directions = [new Cell(1, 0), new Cell(0, 1), new Cell(-1, 0), new Cell(0, -1)];

    record class Neighbour(int R, int C)
    {
        public bool Visited;
    }

    private static int CalculateSides(HashSet<Cell> plots)
    {
        var neighbours = new List<Neighbour>();
        
        foreach (var plot in plots)
        {
            foreach (var direction in Directions)
            {
                var neighbour = new Cell(plot.R + direction.R, plot.C + direction.C);
                if (!plots.Contains(neighbour)) neighbours.Add(new(neighbour.R, neighbour.C));
            }
        }

        neighbours = neighbours.OrderBy(n => n.R).OrderBy(n => n.C).ToList();
        
        var count = 0;
        while (!neighbours.All(n => n.Visited))
        {
            for (var i = 0; i < neighbours.Count; i++)
            {
                var neighbour = neighbours[i];
                if (neighbour.Visited) continue;

                neighbour.Visited = true;
                count++;

                var current = neighbour;

                // Try merge horizontally
                while (current != null)
                {
                    var next = neighbours.FirstOrDefault(n => n.R == current.R && n.C == current.C + 1 && !n.Visited);
                    if (next != null)
                    {
                        next.Visited = true;
                    }
                    current = next;
                }

                // Try merge vertically
                current = neighbour;
                while (current != null)
                {
                    var next = neighbours.FirstOrDefault(n => n.R == current.R + 1 && n.C == current.C && !n.Visited);
                    if (next != null)
                    {
                        next.Visited = true;
                    }
                    current = next;
                }
            }
        }

        return count;
    }

    private static bool Touches(Cell a, Cell b) =>
        new Cell(a.R + 1, a.C) == b || new Cell(a.R, a.C + 1) == b ||
        new Cell(a.R - 1, a.C) == b || new Cell(a.R, a.C - 1) == b;
}