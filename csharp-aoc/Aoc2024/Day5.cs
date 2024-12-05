using System.Diagnostics;

namespace Aoc2024;

public static class Day5
{
    public static readonly string[][] TestInput = [
        [
            "47|53",
            "97|13",
            "97|61",
            "97|47",
            "75|29",
            "61|13",
            "75|53",
            "29|13",
            "97|29",
            "53|29",
            "61|53",
            "97|53",
            "61|29",
            "47|13",
            "75|47",
            "97|75",
            "47|61",
            "75|61",
            "47|29",
            "75|13",
            "53|13",
        ],
        [
            "75,47,61,53,29",
            "97,61,53,29,13",
            "75,29,13",
            "75,97,47,61,53",
            "61,13,29",
            "97,13,75,29,47",
        ]
    ];

    record struct Rule(int A, int B);

    public static void Solve()
    {
        string[] input = File.ReadAllText(@"day5_input.txt").Split("\n\n");
        string[][] groups = [input[0].Split(), input[1].Split()];
        
        //string[][] groups = TestInput;

        var rules = groups[0].Select(input => new Rule(int.Parse(input[0..input.IndexOf('|')]), int.Parse(input[(input.IndexOf('|') + 1)..]))).ToList();
        var updates = groups[1].Where(line => line != "").Select(line => line.Split(",").Select(int.Parse).ToList()).ToList();

        long part1 = 0;
        long part2 = 0;

        foreach (var update in updates)
        {
            if (ProcessUpdate(update, rules))
            {
                part1 += update[update.Count / 2];
            }
            else
            {
                var orderedUpdate = OrderUpdate(update, rules).ToList();
                part2 += orderedUpdate[orderedUpdate.Count / 2];
            }
        }

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    private static bool ProcessUpdate(List<int> update, List<Rule> rules)
    {
        foreach (var rule in rules)
        {
            var a = update.IndexOf(rule.A);
            var b = update.IndexOf(rule.B);

            if (a != -1 && b != -1)
            {
                if (a > b) return false;
            }
        }

        return true;
    }

    private static IOrderedEnumerable<int> OrderUpdate(List<int> update, List<Rule> rules)
    {
        return update.OrderBy(u => u, new Comparer(rules));
    }

    private class Comparer(List<Rule> Rules) : IComparer<int>
    {
        public int Compare(int a, int b)
        {
            if (Rules.Any(r => r.A == a && r.B == b)) return -1;
            if (Rules.Any(r => r.A == b && r.B == a)) return 1;
            return 0;
        }
    }
}
