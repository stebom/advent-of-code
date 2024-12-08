namespace Aoc2024;

public static class Day8
{
    public record Cell(int R, int C);

    public static void Solve()
    {
        char[][] grid = File.ReadAllLines(@"input/day8_input.txt").Select(l => l.ToCharArray()).ToArray();
        int rows = grid.Length;
        int cols = grid[0].Length;

        Dictionary<char, List<Cell>> frequenies = [];

        for (int r  = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var frequency = grid[r][c];
                if (frequency != '.')
                {
                    if (frequenies.TryGetValue(frequency, out var list))
                    {
                        list.Add(new(r, c));
                    }
                    else
                    {
                        frequenies[frequency] = [new(r,c)];
                    }
                }
            }
        }

        HashSet<Cell> antinodes = [];
        HashSet<Cell> repeaters = [];

        foreach (var nodes in frequenies.Values)
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                for (var y = i + 1; y < nodes.Count; y++)
                {
                    var rx = nodes[y].R - nodes[i].R;
                    var cx = nodes[y].C - nodes[i].C;

                    Cell pantinode = new(nodes[i].R - rx, nodes[i].C - cx);

                    if (0 <= pantinode.R && pantinode.R < rows &&
                        0 <= pantinode.C && pantinode.C < cols)
                    {
                        antinodes.Add(pantinode);
                    }

                    Cell nantinode = new(nodes[y].R + rx, nodes[y].C + cx);

                    if (0 <= nantinode.R && nantinode.R < rows &&
                        0 <= nantinode.C && nantinode.C < cols)
                    {
                        antinodes.Add(nantinode);
                    }

                    var step = 0;
                    while (true)
                    {
                        Cell r = new(nodes[i].R - rx * step, nodes[i].C - cx * step);
                        if (0 <= r.R && r.R < rows && 0 <= r.C && r.C < cols)
                        {
                            repeaters.Add(r);
                        }
                        else break;
                        step++;
                    }

                    step = 0;
                    while (true)
                    {
                        Cell r = new(nodes[y].R + rx*step, nodes[y].C + cx*step);
                        if (0 <= r.R && r.R < rows && 0 <= r.C && r.C < cols)
                        {
                            repeaters.Add(r);
                        }
                        else break;
                        step++;
                    }
                }
            }
        }

        Console.WriteLine($"Part 1: {antinodes.Count}");
        Console.WriteLine($"Part 2: {antinodes.Union(repeaters).Count()}");
    }
}
