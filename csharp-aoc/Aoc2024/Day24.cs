using System.Diagnostics;
using System.Text;

namespace Aoc2024;

public static class Day24
{
    enum Operation { AND, OR, XOR };

    record struct Connection(string InputA, string InputB, string Output, Operation Op);

    public static void Solve()
    {
        var register = new Dictionary<string, int>();
        var connections = new List<Connection>();

        var groups = File.ReadAllText("input/day24_input.txt").Split("\r\n\r\n");
        foreach (var r in groups[0].Split("\r\n"))
        {
            var s = r.Split(": ");
            register.Add(s[0], int.Parse(s[1]));
        }

        foreach (var c in groups[1].Split("\r\n"))
        {
            var s = c.Split();
            var op = s[1] switch
            {
                "AND" => Operation.AND,
                "XOR" => Operation.XOR,
                "OR" => Operation.OR,
                _ => throw new Exception(s[1])
            };
            connections.Add(new(s[0], s[2], s[4], op));
        }

        Part1(register.ToDictionary(), connections);
        Part2(register.ToDictionary(), connections);
    }

    private static long Run(Dictionary<string, int> register, List<Connection> connections)
    {
        long GetValue(char c)
        {
            long val = 0;
            foreach (var reg in register.Where(r => r.Key.StartsWith(c) && r.Value == 1))
            {
                var bit = int.Parse(reg.Key[1..]);
                val += (1L << bit);
            }
            return val;
        }

        var queue = new Queue<Connection>(connections);

        while (queue.TryDequeue(out var connection))
        {
            var inputA = connection.InputA;
            var inputB = connection.InputB;
            var output = connection.Output;

            if (!register.ContainsKey(inputA) || !register.ContainsKey(inputB))
            {
                queue.Enqueue(connection);
                continue;
            }

            register[output] = connection.Op switch
            {
                Operation.AND => register[inputA] & register[inputB],
                Operation.XOR => register[inputA] ^ register[inputB],
                Operation.OR => ((register[inputA] == 1) || (register[inputB] == 1)) ? 1 : 0,
                _ => throw new Exception()
            };
        }

        return GetValue('z');
    }

    private static void Part1(Dictionary<string, int> register, List<Connection> connections)
    {
        Console.WriteLine($"Part 1: {Run(register, connections)}");
    }

    private static void Part2(Dictionary<string, int> register, List<Connection> connections)
    {
        PrintDiagram(connections);

        foreach (var gate in register.Where(x => x.Key.StartsWith('x') || x.Key.StartsWith('y')))
        {
            register[gate.Key] = 0;
        }

        for (var x = 0; x < 44; x++)
        {
            for (var y = 0; y < 44; y++)
            {
                var cpy = register.ToDictionary();
                for (var i = 0; i <= x; i++) cpy[$"x{i:D2}"] = 1;
                for (var h = 0; h <= y; h++) cpy[$"y{h:D2}"] = 1;

                var result = Run(cpy, connections);

                var expected = Convert.ToInt64(new string('1', x + 1), 2) + Convert.ToInt64(new string('1', y + 1), 2);

                Console.WriteLine($"[x={x}, y={y}] {result} {expected}");

                Console.Write("x =  ");
                for (var i = 44; i >= 0; i--) Console.Write(cpy[$"x{i:D2}"]);
                Console.WriteLine();
                Console.Write("y =  ");
                for (var i = 44; i >= 0; i--) Console.Write(cpy[$"y{i:D2}"]);
                Console.WriteLine();
                Console.Write("z = ");
                for (var i = 45; i >= 0; i--) Console.Write(cpy[$"z{i:D2}"]);
                Console.WriteLine();
                var o = Convert.ToString(expected, 2);
                Console.WriteLine($"    {new string(' ', 46 - o.Length)}{o}");
                Debug.Assert(result == expected);
            }
        }

        // Swap 1: z11 => rpv
        // Swap 2: ctg => rpb
        // Swap 3: z31 => dmh
        // Swap 4: z38 => dvq
        string[] swaps = ["z11", "rpv", "ctg", "rpb", "z31", "dmh", "z38", "dvq"];
        Console.WriteLine(string.Join(',', swaps.Order()));
    }

    private static void PrintDiagram(List<Connection> connections)
    {
        var sb = new StringBuilder();

        void Follow(string gate, int depth = 2)
        {
            if (gate.StartsWith('x') || gate.StartsWith('y')) { /* Don't follow */ }
            else
            {
                var indent = new string('.', depth);
                var connection = connections.Single(c => c.Output == gate);
                sb.AppendLine($"{indent}{connection.Output} => {connection.InputA} {connection.Op} {connection.InputB}");
                Follow(connection.InputA, depth + 2);
                Follow(connection.InputB, depth + 2);
            }
        }

        foreach (var connection in connections.Where(c => c.Output.StartsWith('z')).OrderBy(c => c.Output))
        {
            sb.AppendLine($"---------------- {connection.Output} ----------------");
            Follow(connection.Output);
            sb.AppendLine($"-------------------------------------");
        }

        File.WriteAllText("day24_gates.txt", sb.ToString());
    }
}