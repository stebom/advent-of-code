namespace Day9;

public static class Day9
{
    public static void Run()
    {
        var movements = new List<char>();
        foreach (var line in File.ReadAllLines("input.txt").Select(l => l.Split(' ')))
            foreach (var pos in Enumerable.Range(0, int.Parse(line[1])))
                movements.Add(line[0].Single());

        Console.WriteLine($"Number of parsed movements: {movements.Count}");

        var knots = new List<Knot> { new Knot() }; // Add head
        for (var i = 0; i < 9; i++) // Add N tails, each following the last
            knots.Add(new Knot { Next = knots.Last() });

        foreach (var movement in movements)
        {
            knots.First().Move(movement);
            foreach (var knot in knots.Skip(1))
                knot.Follow();
        }

        Console.WriteLine($"Knot 2 visited {knots[1].Visited.Count} positions");
        Console.WriteLine($"Tail visited {knots.Last().Visited.Count} positions");
    }

    class Knot
    {
        public Point Point;
        public Knot? Next;
        public HashSet<Point> Visited = new HashSet<Point>();

        public void Follow() => Follow(Next);

        public void Follow(Knot? k2)
        {
            if (k2 == null) throw new ArgumentNullException(nameof(k2));

            var distance = GetDistance(Point, k2.Point);
            if (distance < 2) return; // Knot is already adjacent to next

            var xd = k2.Point.X - Point.X;
            var yd = k2.Point.Y - Point.Y;

            if (xd > 0) Point.X += 1;
            if (yd > 0) Point.Y += 1;
            if (xd < 0) Point.X -= 1;
            if (yd < 0) Point.Y -= 1;

            Visited.Add(Point);
        }

        public static int GetDistance(Point p1, Point p2)
            => (int)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));

        public int Move(char movement) => movement switch
        {
            'R' => Point.X += 1,
            'U' => Point.Y += 1,
            'L' => Point.X -= 1,
            'D' => Point.Y -= 1,
            _ => throw new ArgumentException(movement.ToString(), nameof(movement))
        };
    }

    struct Point
    {
        public int X;
        public int Y;
    }
}