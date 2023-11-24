namespace Aoc2017;
static class Day10 {
    public static void Run() {
        var input = File.ReadAllLines(@"input_day_10.txt").First();

        {
            // Part One
            var lengths = input.Split(',').Select(int.Parse).ToArray();
            var list = Enumerable.Range(0, 256).ToArray();
            Process(list, lengths);
            Console.WriteLine(list.Take(2).Aggregate((a, b) => a * b));
        }

        {
            // Part Two
            var sparse = Enumerable.Range(0, 256).ToArray();
            var lengths = input.ToArray().Select(c => (int)c).Concat(new[] { 17, 31, 73, 47, 23 }).ToArray();

            var pos = 0;
            var skip = 0;
            for (int i = 0; i < 64; i++) {
                (pos, skip) = Process(sparse, lengths, pos, skip);
            }

            var dense = sparse.Chunk(16).Select(chunk => chunk.Aggregate((a, b) => a ^ b));
            var hash = dense.Select(v => v.ToString("X2").ToLower()).ToArray();
            Console.WriteLine(string.Join("", hash));
        }
    }

    static IEnumerable<int> TakeWrap(this int[] list, int pos, int num) {
        for (int i = 0; i < num; i++) {
            yield return list[pos];
            pos = (pos + 1) % list.Length;
        }
    }

    static (int pos, int skip) Process(int[] list, int[] lengths, int pos = 0, int skip = 0) {

        foreach (var length in lengths) {
            // - Reverse the order of that length of elements in the list,
            //   starting with the element at the current position.
            // - Move the current position forward by that length plus the skip size.
            // - Increase the skip size by one.

            var reversed = list.TakeWrap(pos, length).Reverse().ToArray();

            int p = pos;
            for (int i = 0; i < length; i++) {
                list[p] = reversed[i];
                p = (p + 1) % list.Length;
            }

            pos += length;
            pos += skip;
            pos %= list.Length;
            skip++;
        }

        return (pos, skip);
    }
}