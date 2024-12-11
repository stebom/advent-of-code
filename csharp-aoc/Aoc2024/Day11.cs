using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace Aoc2024;

public static class Day11
{
    public record Link {
        public long Value;
        public Link? Next;
    }

    public static void Solve()
    {
        var counters = File.ReadAllLines(@"input/day11_input.txt")
                           .Single().Split()
                           .Select(long.Parse)
                           .ToDictionary(key => key, _ => (long)1);

        var blinks = new Dictionary<int, long>();

        for (var i = 1; i <= 75; i++) {
            counters = Process(counters);
            blinks.Add(i, counters.Values.Sum());
        }

        Console.WriteLine($"Part 1: {blinks[25]}");
        Console.WriteLine($"Part 2: {blinks[75]}");
    }

    private static Dictionary<long, long> Process(Dictionary<long, long> input)
    {
        var counter = new Dictionary<long, long>();
        foreach (var kvp in input)
        {
            foreach (var value in Produce(kvp.Key))
            {
                if (!counter.TryAdd(value, kvp.Value))
                {
                    counter[value] += kvp.Value;
                }
            }
        }

        return counter;
    }

    private static IEnumerable<long> Produce(long value)
    {
        if (value == 0)
        {
            yield return 1;
        }
        else if (Math.Floor(Math.Log10(value) + 1) % 2 == 0)
        {
            var number = value.ToString();
            var halfLength = number.Length / 2;

            yield return long.Parse(number[..halfLength]);
            yield return long.Parse(number[halfLength..]);
        }
        else
        {
            yield return value * 2024;
        }
    }

    private static Link BuildLinks(List<long> numbers)
    {
        var root = new Link { Value = numbers[0] };

        var link = root;

        for (var i = 1; i < numbers.Count; i++)
        {
            var next = new Link { Value = numbers[i] };
            link.Next = next;
            link = next;
        }

        return root;
    }

    private static long Blink(Link root, int blinks)
        => Enumerable.Range(0, blinks).Sum(_ => Process(root));

    private static long Process(Link root)
    {
        var link = root;
        var created = 0;

        while (link != null)
        {
            // If the stone is engraved with the number 0,
            // it is replaced by a stone engraved with the number 1.
            if (link.Value == 0)
            {
                link.Value = 1;
            }
            // If the stone is engraved with a number that has an even number of digits,
            // it is replaced by two stones. The left half of the digits are engraved on the new left stone,
            // and the right half of the digits are engraved on the new right stone.
            // (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)
            else if (Math.Floor(Math.Log10(link.Value) + 1) % 2 == 0)
            {
                var number = link.Value.ToString();
                var halfLength = number.Length / 2;

                link.Value = long.Parse(number[..halfLength]);

                var split = new Link
                {
                    Value = long.Parse(number[halfLength..]),
                    Next = link.Next
                };

                link.Next = split;
                link = split;

                created++;
            }
            // If none of the other rules apply,
            // the stone is replaced by a new stone; the old stone's number multiplied by 2024 is engraved on the new stone.
            else
            {
                link.Value *= 2024;
            }
            
            link = link.Next;
        }

        return created;
    }
}