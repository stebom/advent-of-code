using System.Text.RegularExpressions;

namespace Aoc2024;

public static class Day3
{
    public static void Solve()
    {
        var lines = File.ReadAllLines(@"day3_input.txt");
        var regex = new Regex(@"(do\(|mul\(|don't\()(?:(\d+),(\d+))?\)");

        // \((?:(\\d+),(\\d+))?\)
        var mul = true;
        long sum = 0;
        foreach (var line in lines) {
            var matches = regex.Matches(line);
            foreach (Match item in matches)
            {
                var op = item.Groups[1].Value;

                if (op == "do(") { mul = true; }
                if (op == "don't(") { mul = false; }
                if (op == "mul(" && mul) {
                    sum += int.Parse(item.Groups[2].Value) * int.Parse(item.Groups[3].Value);
                }
            }

        }
        
        Console.WriteLine(sum);
    }

}
