
namespace Aoc2016
{
    internal static class Day9
    {
        private static int Distance(in Point a, in Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        private record struct Point(int X, int Y);

        internal static void Run()
        {
            RunPart1();
            RunPart2();
        }

        internal static void RunPart1()
        {
            var points = File.ReadAllLines("day9.txt")
                             .Select(line => line.Split(", ", StringSplitOptions.RemoveEmptyEntries).ToArray())
                             .Select(tokens => new Point(int.Parse(tokens[0]), int.Parse(tokens[1])))
                             .OrderBy(p => (p.Y, p.X))
                             .ToArray();

            var minY = points.Min(p => p.Y);
            var maxY = points.Max(p => p.Y);
            var minX = points.Min(p => p.X);
            var maxX = points.Max(p => p.X);

            var finite = points.Where(p => p.Y > minY && p.Y < maxY &&
                                           p.X > minX && p.X < maxX)
                               .ToHashSet();

            var nearest = new Dictionary<Point, int>();
            foreach (var point in points)
            {
                nearest[point] = 0;
            }

            for (var y = minY; y <= maxY; ++y)
            {
                for (var x = minX; x <= maxX; ++x)
                {
                    var current = new Point(x, y);
                    var distances = points.ToDictionary(p => p, p => Distance(p, current))
                                          .OrderBy(kvp => kvp.Value)
                                          .ToArray();
                    if (distances[0].Value < distances[1].Value)
                    {
                        nearest[distances[0].Key] += 1;
                    }
                }
            }

            var best = nearest.Where(p => finite.Contains(p.Key)).Max(p => p.Value);
            Console.WriteLine($"Best: {best}");
        }

        internal static void RunPart2()
        {
            var points = File.ReadAllLines("day9.txt")
                             .Select(line => line.Split(", ", StringSplitOptions.RemoveEmptyEntries).ToArray())
                             .Select(tokens => new Point(int.Parse(tokens[0]), int.Parse(tokens[1])))
                             .OrderBy(p => (p.Y, p.X))
                             .ToArray();

            var minY = points.Min(p => p.Y);
            var maxY = points.Max(p => p.Y);
            var minX = points.Min(p => p.X);
            var maxX = points.Max(p => p.X);

            var threshold = 10000;

            var safe = new HashSet<Point>();

            for (var y = minY; y <= maxY; ++y)
            {
                for (var x = minX; x <= maxX; ++x)
                {
                    var current = new Point(x, y);

                    var totalDistance = points.Sum(p => Distance(p, current));
                    if (totalDistance < threshold)
                    {
                        safe.Add(current);
                    }
                }
            }

            Console.WriteLine($"Safe: {safe.Count}");
        }
    }
}
