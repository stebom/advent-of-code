using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Day12;

public static class Day12
{ 
    public static void Run()
    {
        var grid = File.ReadAllLines("input.txt").Select(l => l.Select(c => c).ToArray()).ToArray();

        var start = Find(grid, 'S');
        var end = Find(grid, 'E');

        grid[start.Y][start.X] = 'a'; // Your current position (S) has elevation a
        grid[end.Y][end.X] = 'z'; // (E) has elevation z

        var sp = new Stopwatch();
        sp.Start();
        Console.WriteLine("Part 1: " + ShortestDistance(grid, start, end));
        sp.Stop();
        Console.WriteLine($"{sp.ElapsedMilliseconds} ms");

        sp.Restart();
        var possibleStarts = FindAll(grid, 'a');
        Console.WriteLine("Part 2: " + ShortestDistance(grid, possibleStarts, end));
        sp.Stop();
        Console.WriteLine($"{sp.ElapsedMilliseconds} ms");
    }

    public static int ShortestDistance(char[][] g, IEnumerable<Point> starts, Point end)
    {
        var distances = starts.Select(start => ShortestDistance(g, start, end));
        return distances.Min();
    }

    public static int ShortestDistance(char[][] g, Point start, Point end)
    {
        //Console.WriteLine($"Find path {start} -> {end}");

        var q = new Queue<Point>();
        var visited = new HashSet<Point>();
        var distance = new Dictionary<Point, int>();

        visited.Add(start);
        distance[start] = 0;
        q.Enqueue(start);

        while (q.Any())
        {
            var s = q.Dequeue();

            foreach (var adjacent in Adjacents(s, g).FilterHeight(s, g))
            {
                if (visited.Contains(adjacent)) continue;
                visited.Add(adjacent);
                distance[adjacent] = distance[s] + 1;
                q.Enqueue(adjacent);
            }
        }

        return distance.ContainsKey(end) ? distance[end] : 999999;
    }

    public static IEnumerable<Point> Adjacents(Point p, char[][] g)
    {
        var ybounds = g.Length - 1;
        var xbounds = g[p.Y].Length - 1;

        if (p.X > 0) yield return new Point(p.X - 1, p.Y); // left
        if (p.Y > 0) yield return new Point(p.X, p.Y - 1); // up
        if (p.X < xbounds) yield return new Point(p.X + 1, p.Y); // right
        if (p.Y < ybounds) yield return new Point(p.X, p.Y + 1); // down
    }

    public static IEnumerable<Point> FilterHeight(this IEnumerable<Point> adj, Point p, char[][] g)
    {
        foreach (var adjacent in adj)
        {
            var aH = g[adjacent.Y][adjacent.X];
            var pH = g[p.Y][p.X];
            if (aH - pH < 2) yield return adjacent;
        }
    }

    public static Point Find(char[][] g, char find)
    {
        if (g == null) throw new ArgumentException(nameof(g));

        for (int y = 0; y < g.Length; y++)
            for (int x = 0; x < g[y].Length; x++)
                if (g[y][x] == find) return new Point(x, y);

        throw new InvalidOperationException("No S in grid");
    }

    public static IEnumerable<Point> FindAll(char[][] g, char find)
    {
        for (int y = 0; y < g.Length; y++)
            for (int x = 0; x < g[y].Length; x++)
                if (g[y][x] == find) yield return new Point(x, y);
    }

    public struct Point
    {
        public Point(int x, int y) => (X, Y) = (x, y);

        public int X;
        public int Y;

        public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Point a, Point b) => a.X == b.X && a.Y == b.Y;


        public override bool Equals([NotNullWhen(true)] object? obj)
         => (obj is Point p) && (X == p.X || Y == p.Y);

        public override int GetHashCode() => (X, Y).GetHashCode();

        public override string ToString() => $"{X},{Y}";
    }
}