namespace aoc_2020;

static class Day10
{
    static void Run()
    {
        var numbers = File.ReadAllLines(@"input_2020_day_10.txt")
            .Where(l => l != string.Empty)
            .Select(int.Parse)
            .ToList();

        numbers.Insert(0, 0);
        numbers.Sort();
        numbers.Add(numbers.Last() + 3);

        // Part 1
        {
            var diffs = new Dictionary<int, int>();
            for (int i = 1; i < numbers.Count; i++)
            {
                var diff = numbers[i] - numbers[i - 1];
                if (diffs.ContainsKey(diff)) { diffs[diff]++; }
                else { diffs[diff] = 1; }
            }

            Console.WriteLine(diffs[1] * diffs[3]);
        }

        // Part 2
        long solve(int i, Dictionary<int, long> memo)
        {
            if (i == numbers!.Count - 1) { return 1; }
            if (memo.ContainsKey(i)) { return memo[i]; }

            long ans = 0;

            for (var y = i + 1; y < numbers.Count; y++)
            {
                if (numbers[y] - numbers[i] <= 3)
                {
                    ans += solve(y, memo);
                }

                if (y > i + 3) { break; }
            }

            memo[i] = ans;
            return ans;
        }

        Console.WriteLine(solve(0, new()));
    }
}

