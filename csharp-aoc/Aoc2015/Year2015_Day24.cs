namespace AdventOfCode;

internal class Year2015_Day24
{
    private const int m = 3;

    internal static void Solve()
    {
        //Console.WriteLine(FindMin([1, 2, 3, 4, 5, 7, 8, 9, 10, 11], 3));
        //Console.WriteLine(FindMin([1, 2, 3, 4, 5, 7, 8, 9, 10, 11], 4));

        int[] packages = [1, 2, 3, 5, 7, 13, 17, 19, 23, 29, 31, 37, 41, 43, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113];
        Console.WriteLine(FindMin(packages, 3));
        Console.WriteLine(FindMin(packages, 4));
    }

    public static long FindMin(int[] packages, int compartmentCount)
    {
        var compartmentWeight = packages.Sum() / compartmentCount;

        var size = 1;

        while (size < 10)
        {
            // Find the smallest size that has valid combinations
            var validCombinations = GetCombinations([.. packages], size)
                                   .Where(combo => combo.Sum() == compartmentWeight);

            if (validCombinations.Any())
            {
                // Calculate minimum quantum entanglement (product)
                return validCombinations.Select(combo => combo.Aggregate(1L, (product, num) => product * num)).Min();
            }
            size++;
        }

        return -1;
    }

    // Helper method to generate combinations of given size
    private static IEnumerable<int[]> GetCombinations(int[] numbers, int size)
    {
        if (size == 0)
        {
            yield return [];
            yield break;
        }

        if (numbers.Length < size)
        {
            yield break;
        }

        var remaining = numbers[1..numbers.Length];

        // Include combinations with the first number
        foreach (var combination in GetCombinations(remaining, size - 1))
        {
            yield return [..combination, numbers[0]];
        }

        // Include combinations without the first number
        foreach (var combination in GetCombinations(remaining, size))
        {
            yield return combination;
        }
    }
}