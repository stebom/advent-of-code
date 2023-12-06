namespace Aoc2023;

internal class Day2 {
    internal static void Run() {
        Part1();
        Part2();
    }

    static void Part1() {
        var total = 0;
        foreach (var line in File.ReadAllLines("input_day_2.txt")) {
            var gameId = int.Parse(line.Substring(5, line.IndexOf(":") - 5));
            var sets = line[(line.IndexOf(":") + 2)..].Split(";");

            var possible = true;

            foreach (var set in sets) {
                var cubes = set.Split(",", StringSplitOptions.RemoveEmptyEntries);

                foreach (var cube in cubes) {
                    var tokens = cube.Trim().Split();
                    var num = int.Parse(tokens[0]);
                    var color = tokens[1];

                    // only 12 red cubes, 13 green cubes, and 14 blue cubes
                    possible &= color switch {
                        "red" => num <= 12,
                        "green" => num <= 13,
                        "blue" => num <= 14,
                        _ => throw new Exception(color)
                    };
                }
            }

            if (possible) {
                total += gameId;
            }
        }
        Console.WriteLine($"Part 1: {total}");
    }

    static void Part2() {
        var power = 0;

        foreach (var line in File.ReadAllLines("input_day_2.txt")) {
            var sets = line[(line.IndexOf(":") + 2)..].Split(";");

            // red, green, blue
            var maxCubes = new[] { 0, 0, 0 };

            foreach (var set in sets) {
                var cubes = set.Split(",", StringSplitOptions.RemoveEmptyEntries);

                foreach (var cube in cubes) {
                    var tokens = cube.Trim().Split();
                    var num = int.Parse(tokens[0]);
                    var color = tokens[1];
                    var index = color switch {
                        "red" => 0,
                        "green" => 1,
                        "blue" => 2,
                        _ => throw new Exception(color)
                    };
                    maxCubes[index] = Math.Max(maxCubes[index], num);
                }
            }
            power += maxCubes.Aggregate((a, b) => a * b);
        }

        Console.WriteLine($"Part 2: {power}");
    }
}