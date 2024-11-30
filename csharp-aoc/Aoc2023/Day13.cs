using System.Diagnostics;

namespace AdventOfCode;

internal class Day13
{
    static char[][] Rotate(char[][] input)
    {
        Debug.Assert(input.All(x => x.Length == input[0].Length));

        var rotated = new char[input[0].Length][];
        for (var c = 0; c < rotated.Length; c++)
        {
            rotated[c] = input.Select(r => r[c]).ToArray();
        }

        return rotated;
    }

    static IEnumerable<int> FindPairs(char[][] input)
    {
        Debug.Assert(input.Length > 1);
        for (var i = 1; i < input.Length; i++)
        {
            if (input[i - 1].SequenceEqual(input[i]))
            {
                yield return i - 1;
            }
        }
    }

    static bool IsReflection(char[][] input, int midPoint)
    {
        var isReflection = true;
        for (var offset = 1; offset <= midPoint; offset++)
        {
            var left = midPoint - offset;
            var right = midPoint + 1 + offset;

            if (left < 0 || right >= input.Length)
            {
                break;
            }

            isReflection = input[left].SequenceEqual(input[right]);
            if (!isReflection) return false;
        }

        return isReflection;
    }

    static IEnumerable<int> FindValidPairs(char[][] input)
        => FindPairs(input).Where(p => IsReflection(input, p));

    static int Summarize(char[][] input)
    {
        var horizontalPairs = FindValidPairs(input);
        var verticalPairs = FindValidPairs(Rotate(input));
        return horizontalPairs.Select(p => (p + 1) * 100).Sum() +
               verticalPairs.Select(p => p + 1).Sum();
    }

    static int SmudgeAndSummarize(char[][] pattern)
    {
        var originalHorizontalPairs = FindValidPairs(pattern).ToList();
        var originalVerticalPairs = FindValidPairs(Rotate(pattern)).ToList();

        var horizontalPairs = new List<int>();
        var verticalPairs = new List<int>();

        static char Flip(char c) => c == '.' ? '#' : '.';

        for (var r = 0; r < pattern.Length; r++)
        {
            for (var c = 0; c < pattern[r].Length; c++)
            {
                pattern[r][c] = Flip(pattern[r][c]);

                horizontalPairs.AddRange(FindValidPairs(pattern));
                verticalPairs.AddRange(FindValidPairs(Rotate(pattern)));

                pattern[r][c] = Flip(pattern[r][c]);
            }
        }

        return horizontalPairs.Except(originalHorizontalPairs).Select(p => (p + 1) * 100).Sum() +
               verticalPairs.Except(originalVerticalPairs).Select(p => p + 1).Sum();
    }

    public static void Solve()
    {
        var patterns = File.ReadAllText(@"2023_13_input.txt")
                           .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                           .Select(x => x.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToCharArray()).ToArray())
                           .ToList();

        Console.WriteLine($"Part 1: {patterns.Sum(Summarize)}");
        Console.WriteLine($"Part 2: {patterns.Sum(SmudgeAndSummarize)}");
    }
}
