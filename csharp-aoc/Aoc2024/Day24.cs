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
    }

    private static void Part1(Dictionary<string, int> register, List<Connection> connections)
    {
        var queue = new Queue<Connection>(connections);

        while (queue.TryDequeue(out var connection))
        {
            if (!register.ContainsKey(connection.InputA) || !register.ContainsKey(connection.InputB))
            {
                queue.Enqueue(connection);
                continue;
            }

            register[connection.Output] = connection.Op switch
            {
                Operation.AND => register[connection.InputA] & register[connection.InputB],
                Operation.XOR => register[connection.InputA] ^ register[connection.InputB],
                Operation.OR => ((register[connection.InputA] == 1) || (register[connection.InputB] == 1)) ? 1 : 0,
                _ => throw new Exception()
            };
        }

        Console.WriteLine($"Part 1: z={register.GetValue('z')}");
    }

    static long GetValue(this Dictionary<string, int> register, char c)
    {
        long val = 0;
        foreach (var reg in register.Where(r => r.Key.StartsWith(c) && r.Value == 1))
        {
            var bit = int.Parse(reg.Key[1..]);
            val += (1L << bit);
        }
        return val;
    }
}