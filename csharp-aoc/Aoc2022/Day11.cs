using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Day11;

public static class Day11
{
    public static void Run()
    {
        const string monkeyString_ = "Monkey ";
        const string startingItemsString_ = @"  Starting items: ";
        const string operationString_ = @"  Operation: new = old ";
        const string testString_ = @"  Test: divisible by ";
        const string ifString_ = @"    If true: throw to monkey ";

        var items = new List<List<long>>();
        var operations = new List<Func<long, long>>();
        var tests = new List<Func<long, long>>();
        var inspections = new List<long>();
        var divisors = new List<long>();

        var lines = File.ReadAllLines("input.txt").ToArray();
        
        foreach (var monkey in lines.Where(l => l.StartsWith(monkeyString_)))
        {
            var monkeyId = long.Parse(monkey.Substring(monkeyString_.Length).Split(':').First());
            var offset = Array.IndexOf(lines, monkey);

            Debug.Assert(offset > -1);

            var startingItems = lines[offset + 1].Substring(startingItemsString_.Length).Split(", ").Select(long.Parse).ToList();
            items.Add(startingItems);

            var operationGroup = lines[offset + 2].Substring(operationString_.Length).Split(' ');
            Debug.Assert(operationGroup.Length == 2);
            operations.Add(CreateOperation(operationGroup));

            var diviser = long.Parse(lines[offset + 3].Substring(testString_.Length));
            var monkey1 = long.Parse(lines[offset + 4].Substring(ifString_.Length));
            var monkey2 = long.Parse(lines[offset + 5].Substring(ifString_.Length + 1));
            tests.Add(CreateTest(diviser, monkey1, monkey2));
            divisors.Add(diviser);

            inspections.Add(0);
        }

        Debug.Assert(items.Count == operations.Count);
        Debug.Assert(operations.Count == tests.Count);
        Debug.Assert(inspections.Count == inspections.Count);

        var ldc = divisors.Aggregate(1, (a, b) => (int)(a * b));


        Console.WriteLine($"Before playing, the monkeys are holding items with these worry levels:");
        for (var monkey = 0; monkey < items.Count; ++monkey)
        {
            Console.WriteLine($"Monkey {monkey}: {string.Join(',', items[monkey])}");
        }

        var numRounds = 10000;
        for (var round = 0; round < numRounds; ++round)
        {
            for (var monkey = 0; monkey < items.Count; ++monkey)
            {
                var currentitems = items[monkey].ToList();
                foreach (var item in currentitems)
                {
                    var result = operations[monkey](item);
                    var toMonkey = tests[monkey](result);

                    result %= ldc;

                    items[monkey].Remove(item);

                    //Console.WriteLine($"Monkey {monkey} produced {item} -> {result}");

                    items[(int)toMonkey].Add(result);

                    inspections[monkey]++;
                }
            }

            if (round + 1 == 20 || (round + 1) % 1000 == 0) { 
                Console.WriteLine($"== After round {round+1} ==");
                for (var monkey = 0; monkey < items.Count; ++monkey)
                {
                    Console.WriteLine($"Monkey {monkey} inspected items {inspections[monkey]} times.");
                }

                Console.WriteLine();
            }
        }

        var highest = inspections.OrderDescending().Take(2);
        var product = highest.Aggregate(1L, (acc, val) => acc * val);

        Console.WriteLine($"Highest: {string.Join(", ", highest)}: {product}.");

        Func<long, long> CreateTest(long diviser, long monkey1, long monkey2)
            => val => val % diviser == 0 ? monkey1 : monkey2;

        Func<long, long> CreateOperation(string[] operationGroup) =>
            operationGroup[0] switch
            {
                "*" => v => Product(v, operationGroup[1] == "old" ? v : long.Parse(operationGroup[1])),
                "+" => v => Sum(v, operationGroup[1] == "old" ? v : long.Parse(operationGroup[1])),
                "-" => v => Subtract(v, operationGroup[1] == "old" ? v : long.Parse(operationGroup[1])),
                "/" => v => Divide(v, operationGroup[1] == "old" ? v : long.Parse(operationGroup[1])),
                _ => throw new ArgumentException($"{operationGroup[0]} not handled")
            };

        long Product(long o, long n) => o * n;
        long Sum(long o, long n) => o + n;
        long Subtract(long o, long n) => o - n;
        long Divide(long o, long n) => o / n;
    }
}