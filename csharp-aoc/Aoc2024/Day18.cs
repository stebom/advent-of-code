using System.Diagnostics;

namespace AdventOfCode;

internal static class Day18
{
    record Position(int R, int C);

    static readonly string[] TestInput = [
        "R 6 (#70c710)",
        "D 5 (#0dc571)",
        "L 2 (#5713f0)",
        "D 2 (#d2c081)",
        "R 2 (#59c680)",
        "D 2 (#411b91)",
        "L 5 (#8ceee2)",
        "U 2 (#caa173)",
        "L 1 (#1b58a2)",
        "U 2 (#caa171)",
        "R 2 (#7807d2)",
        "U 3 (#a77fa3)",
        "L 2 (#015232)",
        "U 2 (#7a21e3)",
    ];

    static int Steps(this string input)
    {
        if (input[4] == '(')
        {
            Debug.Assert(char.IsAsciiDigit(input[2]));
            return input[2] - '0';
        }

        Debug.Assert(input[5] == '(');
        Debug.Assert(char.IsAsciiDigit(input[2]) && char.IsAsciiDigit(input[3]));
        return int.Parse(input.Substring(2, 2));
    }

    static Position Delta(this string operation, int steps = 1) => operation[0].Delta(steps);

    static Position Delta(this char operation, int steps = 1) => operation switch
    {
        'U' => new(-steps, 0),
        'R' => new(0, steps),
        'D' => new(steps, 0),
        'L' => new(0, -steps),
        _ => throw new NotImplementedException(),
    };

    static readonly Position Up = 'U'.Delta();
    static readonly Position Right = 'R'.Delta();
    static readonly Position Down = 'D'.Delta();
    static readonly Position Left = 'L'.Delta();

    static Position Walk(this Position position, Position delta) => new(position.R + delta.R, position.C + delta.C);

    static (bool, HashSet<Position>) Flood(this HashSet<Position> positions, Position start)
    {
        Debug.Assert(!positions.Contains(start), "Can't flood fill from trench");

        var minR = positions.Min(p => p.R);
        var minC = positions.Min(p => p.C);
        var maxR = positions.Max(p => p.R);
        var maxC = positions.Max(p => p.C);

        bool Outside(Position position) => position.R < minR || position.C < minC ||
                                           position.R > maxR || position.C > maxC;

        var queue = new Queue<Position>();
        queue.Enqueue(start);

        var visited = new HashSet<Position>();

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (visited.Contains(current)) continue;

            visited.Add(current);

            if (Outside(current))
            {
                return (false, visited);
            }
            else if (positions.Contains(current))
            {
                // We found the edge of the trench
                continue;
            }

            var up = current.Walk(Up);
            if (!visited.Contains(up)) queue.Enqueue(up);

            var right = current.Walk(Right);
            if (!visited.Contains(right)) queue.Enqueue(right);

            var down = current.Walk(Down);
            if (!visited.Contains(down)) queue.Enqueue(down);

            var left = current.Walk(Left);
            if (!visited.Contains(left)) queue.Enqueue(left);
        }

        return (true, visited);
    }

    static HashSet<Position> FloodFill(HashSet<Position> positions)
    {
        var visited = new HashSet<Position>();
        var interiors = new HashSet<Position>();
        var externals = new HashSet<Position>();

        var minR = positions.Min(p => p.R);
        var minC = positions.Min(p => p.C);
        var maxR = positions.Max(p => p.R);
        var maxC = positions.Max(p => p.C);

        for (var r = minR; r <= maxR; r++)
        {
            for (var c = minC; c <= maxC; c++)
            {
                Position current = new(r, c);

                if (visited.Contains(current)) { continue; }

                if (!positions.Contains(current))
                {
                    var (inside, cells) = positions.Flood(current);

                    foreach (var cell in cells)
                    {
                        if (inside) interiors.Add(cell);
                        else externals.Add(cell);
                        visited.Add(cell);
                    }
                }

                visited.Add(current);
            }
        }

        return interiors;
    }

    static int FloodFill(HashSet<Position> positions, Position midPoint)
    {
        var stack = new Stack<Position>();
        stack.Push(midPoint);

        var visited = new HashSet<Position>();

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (visited.Contains(current)) continue;
            visited.Add(current);

            var adjacents = new [] { current.Walk(Up), current.Walk(Right), current.Walk(Down), current.Walk(Left) };
            foreach (var next in adjacents)
            {
                if (visited.Contains(next) || positions.Contains(next)) continue;

                visited.Add(next);
                stack.Push(next);
            }
        }
        
        return visited.Count;
    }

    static void Print(HashSet<Position> positions)
    {
        var minR = positions.Min(p => p.R);
        var minC = positions.Min(p => p.C);
        var maxR = positions.Max(p => p.R);
        var maxC = positions.Max(p => p.C);

        var midR = (minR + maxR) / 2;
        var midC = (minC + maxC) / 2;

        for (var r = minR; r <= maxR; r++)
        {
            for (var c = minC; c <= maxC; c++)
            {
                if (c == 0 && r == 0)
                {
                    Console.Write('S');
                }
                else if (c == midC && r == midR)
                {
                    Console.Write('M');
                }
                else
                {
                    Console.Write(positions.Contains(new(r, c)) ? '#' : '.');
                }
            }
            Console.WriteLine();
        }
    }

    static void Part1(string[] input)
    {
        Position current = new(0, 0);
        var trench = new HashSet<Position> { current };

        foreach (var operation in input)
        {
            var delta = operation.Delta();
            var steps = operation.Steps();

            for (var i = 0; i < steps; i++)
            {
                current = current.Walk(delta);
                if (!trench.Add(current))
                {
                    Console.WriteLine($"{current} already visited");
                }
            }
        }

        //Print(trench);

        var interiors = FloodFill(trench);
        var lagoon = trench.Union(interiors).ToHashSet();

        Console.WriteLine($"Trench is {trench.Count} m3");
        Console.WriteLine($"Flood filled {interiors.Count} positions");
        Console.WriteLine($"Part 1: Lagoon is {lagoon.Count} m3");
    }

    record struct BigPos(long R, long C);

    static void Part2(string[] input)
    {
        BigPos current = new(0, 0);
        var positions = new List<BigPos> { current };

        long perimeter = 0;

        foreach (var operation in input)
        {
            var delimiter = operation.IndexOf('#') + 1;
            var steps = Convert.ToInt32(operation.Substring(delimiter, 5), 16);

            current = operation[delimiter + 5] switch {
                '0' => new(current.R, current.C + steps),
                '1' => new(current.R + steps, current.C),
                '2' => new(current.R, current.C - steps),
                '3' => new(current.R - steps, current.C),
                _ => throw new NotImplementedException()
            };

            positions.Add(current);
            perimeter += steps;
        }

        long area = 0;

        for (var i = 0; i < positions.Count; i++)
        {
            var nextI = (i + 1) % positions.Count;
            var prevI = i - 1 < 0 ? positions.Count - 1 : i - 1;
            area += positions[i].C * (positions[nextI].R - positions[prevI].R);
        }

        area = Math.Abs(area) / 2;
        area += perimeter / 2 + 1;

        Console.WriteLine($"Part 2: Lagoon is {area} m3");
    }

    public static void Solve()
    {
        var input = File.ReadAllLines(@"2023_18_input.txt");
        
        Part1(TestInput);
        Part1(input);

        Part2(TestInput);
        Part2(input);
    }
}
