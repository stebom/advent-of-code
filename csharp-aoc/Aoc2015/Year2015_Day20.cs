using System.Linq;

namespace AdventOfCode;

internal class Year2015_Day20
{
    static List<int> Divisors(int p)
    {
        List<int> v = [];
        for (var i = 1; i <= (int)Math.Sqrt(p); i++)
        {
            if (p % i == 0)
            {
                v.Add(i);
                if (i != p)
                {
                    v.Add(p / i);
                }
            }
        }

        return v;
    }

    static readonly int[] counter = new int[83160000];

    static int Calculate(int house, int product, bool exclude = false)
    {
        var divisors = Divisors(house);
        if (!exclude) return divisors.Sum(p => p * product);

        var total = 0;
        foreach (var divisor in divisors)
        {
            counter[divisor]++;
            if (counter[divisor] > 50) continue;
            total += divisor * product;
        }
        return total;
    }

    public static void Solve()
    {
        var house = 1;
        while (true)
        {
            var presents = Calculate(house, 10);
            if (presents > 36000000) break;
            house++;
        }
        Console.WriteLine($"Part 1: Reached {36000000} at house {house}");

        house = 1;
        while (true)
        {
            var presents = Calculate(house, 11, true);
            if (presents > 36000000) break;
            house++;
        }
        Console.WriteLine($"Part 2: Reached {36000000} at house {house}");
    }
}