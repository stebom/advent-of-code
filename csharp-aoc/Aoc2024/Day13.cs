using System.Text.RegularExpressions;

namespace Aoc2024;

public static class Day13
{
    public static void Solve()
    {
        var chunks = File.ReadAllText(@"input/day13_input.txt").Split("\r\n\r\n");

        long part1 = 0;
        long part2 = 0;

        foreach (var chunk in chunks)
        {
            var lines = chunk.Split("\r\n");
            var a = Regex.Match(lines[0], @"Button A: X\+(\d+), Y\+(\d+)");
            var b = Regex.Match(lines[1], @"Button B: X\+(\d+), Y\+(\d+)");
            var prize = Regex.Match(lines[2], @"Prize: X\=(\d+), Y\=(\d+)");

            part1 += Simulate(int.Parse(a.Groups[1].Value), int.Parse(a.Groups[2].Value),
                               int.Parse(b.Groups[1].Value), int.Parse(b.Groups[2].Value),
                               int.Parse(prize.Groups[1].Value), int.Parse(prize.Groups[2].Value));

            part2 += Calculate(long.Parse(a.Groups[1].Value), long.Parse(a.Groups[2].Value),
                               long.Parse(b.Groups[1].Value), long.Parse(b.Groups[2].Value),
                               long.Parse(prize.Groups[1].Value), long.Parse(prize.Groups[2].Value));
        }

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    static int Simulate(int a1, int a2, int b1, int b2, int X, int Y)
    {
        for (int a = 1; a <= 100; a++)
        {
            for (int b = 1; b <= 100; b++)
            {
                var x = (a1 * a + b1 * b);
                var y = (a2 * a + b2 * b);

                if (X == x && Y == y)
                {
                    return a * 3 + b;
                }
            }
        }
        return 0;
    }

    static long Calculate(double a1, double a2, double b1, double b2, double X, double Y)
    {
        X += 10_000_000_000_000;
        Y += 10_000_000_000_000;

        double a = (X * b2 - Y * b1) / (a1 * b2 - a2 * b1);
        double b = (Y * a1 - X * a2) / (a1 * b2 - a2 * b1);

        if ((a % 1 == 0) && (b % 1 == 0))
        {
            return (long)a * 3 + (long)b;
        }
        return 0;
    }
}