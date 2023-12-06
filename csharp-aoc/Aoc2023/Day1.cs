namespace Aoc2023;

internal class Day1 {
    internal static void Run() {
        Part1();
        Part2();
    }

    static void Part1() {
        var total = 0;
        foreach (var line in File.ReadAllLines("input_day_1.txt")) {
            var numbers = new List<int>();
            for (int i = 0; i < line.Length; i++) {
                if (char.IsDigit(line[i])) {
                    numbers.Add(line[i] - '0');
                }
            }
            total += numbers.First() * 10 + numbers.Last();
        }
        Console.WriteLine($"Part 1: {total}");
    }

    static void Part2() {
        var total = 0;
        foreach (var line in File.ReadAllLines("input_day_1.txt")) {
            var numbers = new List<int>();
            for (int i = 0; i < line.Length; i++) {
                if (char.IsDigit(line[i])) {
                    numbers.Add(line[i] - '0');
                } else {
                    var words = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
                    var word = words.FirstOrDefault(w => line[i..].StartsWith(w));
                    if (word != null) {
                        numbers.Add(Array.IndexOf(words, word) + 1);
                    }
                }
            }
            total += numbers.First() * 10 + numbers.Last();
        }
        Console.WriteLine($"Part 2: {total}");
    }
}