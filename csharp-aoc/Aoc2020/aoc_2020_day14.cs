namespace aoc_2020;

static class Day14 {
    public static void Run() {
        var input = File.ReadAllLines(@"input_2020_day_14.txt")
            .Where(l => l != string.Empty)
            .ToArray();

        Part1(input);
        Part2(input);
    }

    static void Part1(string[] input) {
        var memory = new Dictionary<string, long>();
        var mask = new string('X', 36);

        foreach (var line in input) {
            if (line.StartsWith("mask")) {
                mask = line[7..];
            } else {
                var tokens = line.Split(" = ");
                var address = tokens[0];

                var value = Convert.ToString(int.Parse(tokens[1]), 2).PadLeft(36, '0').ToArray();
                for (var i = 0; i < mask.Length; i++) {
                    if (mask[i] == '0' || mask[i] == '1') {
                        value[i] = mask[i];
                    }
                }

                memory[address] = Convert.ToInt64(new string(value), 2);
            }

        }

        Console.WriteLine(memory.Sum(a => a.Value));
    }

    static void Part2(string[] input) {
        var memory = new Dictionary<long, long>();
        var mask = new string('X', 36);

        foreach (var line in input) {
            if (line.StartsWith("mask")) {
                mask = line[7..];
            } else {
                var tokens = line.Split(" = ");
                var address = long.Parse(tokens[0][4..].Replace("]", ""));
                var value = int.Parse(tokens[1]);

                var maskedAddress = Convert.ToString(address, 2).PadLeft(36, '0').ToArray();

                for (var i = 0; i < mask.Length; i++) {
                    if (mask[i] == '1' || mask[i] == 'X') {
                        maskedAddress[i] = mask[i];
                    }
                }

                foreach (var subAddress in ExpandMask(maskedAddress)) {
                    memory[subAddress] = value;
                }
            }

        }

        Console.WriteLine(memory.Sum(a => a.Value));
    }

    static IEnumerable<long> ExpandMask(char[] maskedAddress) {

        var queue = new Queue<char[]>();
        queue.Enqueue(maskedAddress);

        while (queue.Count > 0) {
            var top = queue.Dequeue();

            int next = Array.IndexOf(top, 'X');
            if (next == -1) {
                yield return Convert.ToInt64(new string(top), 2);
            } else if (top.Length > 0) {
                {
                    var zero = top[..];
                    zero[next] = '0';
                    queue.Enqueue(zero);
                }
                {
                    var one = top[..];
                    one[next] = '1';
                    queue.Enqueue(one);
                }
            }
        }
    }
}

