using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace AdventOfCode;

internal static class Day12
{
    static readonly string[] TestInput = [
        "???.### 1,1,3",
        ".??..??...?##. 1,1,3",
        "?#?#?#?#?#?#?#? 1,3,1,6",
        "????.#...#... 4,1,1",
        "????.######..#####. 1,6,5",
        "?###???????? 3,2,1",
    ];

    record struct Key(int I, int Bi, int Current);

    static readonly char[] Cars = ['.', '#'];

    static readonly Dictionary<Key, long> Cache = [];

    static long Count(ReadOnlySpan<char> dots, ReadOnlySpan<int> blocks, int i, int bi, int current)
    {
        Key key = new(i, bi, current);
        if (Cache.TryGetValue(key, out var result)) { return result; }

        if (i == dots.Length)
        {
            if (bi == blocks.Length && current == 0)
            {
                return 1;
            }
            else if (bi == blocks.Length - 1 && blocks[bi] == current)
            {
                return 1;
            }
            else return 0;
        }

        var ans = 0L;

        foreach (var c in Cars)
        {
            if (dots[i] == c || dots[i]== '?')
            { 
                if (c == '.' && current== 0) {
                    ans += Count(dots, blocks, i + 1, bi, 0);
                }
                else if (c== '.' && current> 0 && bi < blocks.Length && blocks[bi]== current)
                {
                    ans += Count(dots, blocks, i + 1, bi + 1, 0);
                }
                else if (c == '#')
                {
                    ans += Count(dots, blocks, i + 1, bi, current + 1);
                }
            }
        }

        Cache[key] = ans;
        return ans;
    }

    public static void Solve()
    {
        //var input = TestInput;
        var input = File.ReadAllLines(@"2023_12_input.txt");

        long total = 0;
        long unfoldedTotal = 0;

        foreach (var item in input)
        {
            var row = item.Split(' ');
            var condition = row[0];
            var nums = row[1].Split(',').Select(int.Parse).ToArray();

            var count = Count(condition, nums, 0, 0, 0);
            total += count;
            Console.WriteLine($"{item} - {count} arrangement");

            Cache.Clear();

            var deflatedCondition = string.Join('?', Enumerable.Repeat(condition, 5));
            int[] deflatedNums = nums.Concat(nums).Concat(nums).Concat(nums).Concat(nums).ToArray();

            var unfoldedCount = Count(deflatedCondition, deflatedNums, 0, 0, 0);
            unfoldedTotal += unfoldedCount;
            Console.WriteLine($"{deflatedCondition} - {unfoldedTotal} arrangement");

            Cache.Clear();
        }

        Console.WriteLine($"Total: {total}");
        Console.WriteLine($"Unfolded total: {unfoldedTotal}");
    }
}