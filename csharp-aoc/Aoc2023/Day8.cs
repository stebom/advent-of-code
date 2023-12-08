namespace Aoc2023;

internal static class Day8 {

    record Node(string Name, string Left, string Right);

    static long GCF(long a, long b) {
        while (b != 0) {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static long LCM(IEnumerable<long> arr) => arr.Aggregate((a, b) => (a / GCF(a, b)) * b);

    internal static void Run() {
        var lines = File.ReadAllLines("input_day_8.txt");
        var steps = lines[0].ToArray();
        var nodes = lines[2..].Where(l => !string.IsNullOrWhiteSpace(l))
                  .Select(line => new Node(line[..3], line[7..10], line[12..15])
        ).ToDictionary(n => n.Name, n => n);

        Part1(steps, nodes);
        Part2(steps, nodes);
    }

    static long WalkToZ(Dictionary<string, Node> nodes, char[] steps, Node start) {
        var current = new Node(start.Name, start.Left, start.Right);
        var walk = 0;
        while (current.Name[2] != 'Z') {
            var step = steps[walk % steps.Length];
            current = step switch {
                'R' => nodes[current.Right],
                'L' => nodes[current.Left],
                _ => throw new Exception($"{current} {step}")
            };
            walk++;
        };
        return walk;
    }

    static void Part1(char[] steps, Dictionary<string, Node> nodes) {
        Console.WriteLine($"Part 1: {WalkToZ(nodes, steps, nodes["AAA"])}");
    }

    static void Part2(char[] steps, Dictionary<string, Node> nodes) {
        var allSteps = nodes.Keys.Where(n => n[2] == 'A').Select(n => WalkToZ(nodes, steps, nodes[n]));
        Console.WriteLine($"Part 2: {LCM(allSteps)}");
    }
}