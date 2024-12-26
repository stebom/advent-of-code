namespace Aoc2024;

public static class Day22
{
    public static void Solve()
    {
        var numbers = File.ReadAllLines("input/day22_input.txt").Select(long.Parse).ToArray();
        Part1(numbers);
        Part2(numbers);
    }

    static void Part1(long[] numbers)
    {
        const int n = 2000;
        long sum = 0;
        foreach (var number in numbers)
        {
            var secret = number;
            for (var i = 0; i < n; i++) secret = Process(secret);
            sum += secret;
        }

        Console.WriteLine($"Part 2: {sum}");
    }

    record struct Change(int Seller, int Price, int Change1, int Change2, int Change3, int Change4);

    static void Part2(long[] numbers)
    {
        const int n = 2000;
        var priceArrays = numbers.Select(secret => Generate(secret).Select(secret => (int)secret % 10).Take(n).ToArray()).ToArray();

        var changes = new List<Change>();

        for (var seller = 0; seller < priceArrays.Length; seller++)
        {
            var prices = priceArrays[seller];

            for (var i = 4; i < prices.Length - 1; i++)
            {
                changes.Add(new Change(seller,
                                       prices[i - 0],
                                       prices[i - 3] - prices[i - 4],
                                       prices[i - 2] - prices[i - 3],
                                       prices[i - 1] - prices[i - 2],
                                       prices[i - 0] - prices[i - 1]));
            }
        }

        var matching = changes.GroupBy(change => new { change.Change1, change.Change2, change.Change3, change.Change4 })
                              .Select(match => match.GroupBy(match => match.Seller).Sum(g => g.First().Price));

        Console.WriteLine($"Part 2: {matching.Max()}");
    }

    static IEnumerable<long> Generate(long secret)
    {
        while (true)
        {
            yield return secret;
            secret = Process(secret);
        }
    }

    static long Process(long secret)
    {
        // Calculate the result of multiplying the secret number by 64.
        // Then, mix this result into the secret number.
        // Finally, prune the secret number.
        secret = Prune(Mix(secret, secret * 64));

        // Calculate the result of dividing the secret number by 32.
        // Round the result down to the nearest integer.
        // Then, mix this result into the secret number.
        // Finally, prune the secret number.
        secret = Prune(Mix(secret, secret / 32));

        // Calculate the result of multiplying the secret number by 2048.
        // Then, mix this result into the secret number.
        // Finally, prune the secret number.
        secret = Prune(Mix(secret, secret * 2048));

        return secret;
    }

    /// <summary>
    /// To mix a value into the secret number, calculate the bitwise XOR of the given value and the secret number.
    /// </summary>
    static long Mix(long secret, long value) => secret ^ value;

    /// <summary>
    /// To prune the secret number, calculate the value of the secret number modulo 16777216.
    /// </summary>
    static long Prune(long secret) => secret % 16777216;
}