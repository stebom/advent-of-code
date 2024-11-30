namespace AdventOfCode;

internal class Day21
{
    record struct Position(int R, int C);
    record State(Position Position, int Steps);

    static readonly string[] TestInput = [
        "...........",
        ".....###.#.",
        ".###.##..#.",
        "..#.#...#..",
        "....#.#....",
        ".##..S####.",
        ".##..#...#.",
        ".......##..",
        ".##.#.####.",
        ".##..##.##.",
        "...........",
    ];

    static Position FindStart(string[] input)
    {
        for (int r = 0; r < input.Length; r++)
        for (int c = 0; c < input[0].Length; c++)
        if (input[r][c] == 'S') return new(r, c);

        throw new Exception("OOOPS...");
    }

    static char Map(string [] input, Position position)
    {
        var row = position.R % input.Length;
        if (row < 0) { row += input.Length; }
        var col = position.C % input.Length;
        if (col < 0) { col += input.Length; }

        return input[row][col];
    }

    static IEnumerable<Position> Adjacents(string[] input, Position position)
    {
        Position north = new(position.R - 1, position.C);
        if (Map(input, north) != '#') yield return north;

        Position east = new(position.R, position.C + 1);
        if (Map(input, east) != '#') yield return east;

        Position south = new(position.R + 1, position.C);
        if (Map(input, south) != '#') yield return south;

        Position west = new(position.R, position.C - 1);
        if (Map(input, west) != '#') yield return west;
    }

    static int Walk(string[] input, int steps, bool print = false)
    {
        Position start = FindStart(input);

        HashSet<Position> reaches = [start];

        for (var i = 0; i < steps; i++)
        {
            var states = reaches.ToList();
            reaches.Clear();

            foreach (var state in states)
            {
                foreach (var adjacent in Adjacents(input, state))
                {
                    reaches.Add(adjacent);
                }
            }
        }

        if (print)
        {
            Print(input, reaches, start, steps);
            Console.WriteLine($"In exactly {steps} steps, he can reach {reaches.Count} garden plots");
        }

        return reaches.Count;
    }

    static void Print(string[] input, HashSet<Position> reaches, Position start, int steps)
    {
        var r1 = start.R - steps;
        var r2 = start.R + steps;
        var c1 = start.C - steps;
        var c2 = start.C + steps;

        for (int r = r1; r < r2; r++)
        {
            for (int c = c1; c < c2; c++)
            {
                Position p = new(r, c);
                Console.Write(reaches.Contains(p) ? 'O' : Map(input, p));
            }
            Console.WriteLine();
        }
    }

    public static void Solve()
    {
        //string[] input = TestInput;
        string[] input = File.ReadAllLines(@"2023_21_input.txt");
        Part1(input, 64);
        Part2(input);
    }
    static void Part1(string[] input, int steps = 64)
    {
        var reaches = Walk(input, steps);
        Console.WriteLine($"In exactly {steps} steps, he can reach {reaches} garden plots");
    }

    static void Part2(string[] input, int n = 26_501_365)
    {
        Dictionary<int, int> steps = [];
        HashSet<Position> reaches = [FindStart(input)];

        for (var i = 0; i < 328; i++)
        {
            steps[i] = reaches.Count;
            var states = reaches.ToList();
            reaches.Clear();

            foreach (var state in states)
            {
                foreach (var adjacent in Adjacents(input, state))
                {
                    reaches.Add(adjacent);
                }
            }
        }

        (decimal x0, decimal y0) = (65, steps[65]);
        (decimal x1, decimal y1) = (196, steps[196]);
        (decimal x2, decimal y2) = (327, steps[327]);

        decimal y01 = (y1 - y0) / (x1 - x0);
        decimal y12 = (y2 - y1) / (x2 - x1);
        decimal y012 = (y12 - y01) / (x2 - x0);

        var extrapolated = decimal.Round(y0 + y01 * (n - x0) + y012 * (n - x0) * (n - x1));

        Console.WriteLine($"In exactly {n} steps, he can reach {extrapolated} garden plots");
    }
}