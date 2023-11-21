using System.Collections.Immutable;
using System.Net;

namespace aoc_2020;
static class Day9 {
    static bool IsValid(long val, long[] preamble)
    {
        for (int i = 0; i < preamble.Length - 1; i++)
        {
            for (int j = i + 1; j < preamble.Length; j++)
            {
                if ((preamble[i] + preamble[j]) == val)
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal static void Run()
    {
        var numbers = File.ReadAllLines(@"input_2020_day_9.txt")
                          .Select(long.Parse)
                          .ToArray();

        long step1 = RunPart1(numbers);
        long step2 = RunPart2(step1, numbers);

        Console.WriteLine(step1);
        Console.WriteLine(step2);
    }

    internal static long RunPart1(long[] numbers)
    {
        const int preambleSize = 25;
        for (int i = preambleSize; i < numbers.Length; i++)
        {
            var preamble = numbers.Skip(i - preambleSize).Take(preambleSize).ToArray();

            if (!IsValid(numbers[i], preamble))
            {
                return numbers[i];
            }
        }

        throw new InvalidOperationException("No solution found!");
    }

    internal static long RunPart2(long needle, long[] numbers)
    {
        for (int i = 0; i < numbers.Length-1; i++)
        {
            for (int j = 2; j < numbers.Length; j++)
            {
                var contiguousSet = numbers.Skip(i).Take(j).ToList();
                if (contiguousSet.Sum() == needle)
                {
                    contiguousSet.Sort();
                    return contiguousSet.First() + contiguousSet.Last();
                }

            }
        }

        throw new InvalidOperationException("No solution found!");
    }
}
