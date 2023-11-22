namespace Aoc2017;
static class Day10 {
    public static void Run() {
        var numbers = File.ReadAllLines(@"input_day_10.txt").First().Split(',').Select(int.Parse).ToArray();
        Process(Enumerable.Range(0, 256).ToArray(), numbers);
    }

    static IEnumerable<int> TakeWrap(this int[] list, int pos, int num) {
        for (int i = 0; i < num; i++) {
            yield return list[pos];
            pos = (pos + 1) % list.Length;
        }
    }

    static void Process(int[] list, int[] lengths) {

        int pos = 0;
        int skip = 0;

        foreach (var length in lengths) {

            //Console.WriteLine($"pass {skip} pos: {pos} length: {length} list: {string.Join(',', list)}");

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

        Console.WriteLine($"pass {skip} pos: {pos} list: {string.Join(',', list)}");

        Console.WriteLine(list.Take(2).Aggregate((a, b) => a * b));
    }
}