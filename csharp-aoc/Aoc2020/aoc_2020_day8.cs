namespace aoc_2020;
static class Day8 {
    internal static void Run()
    {
        var instructions = File.ReadAllLines(@"input_2020_day_8.txt");
        Console.WriteLine(Run(instructions)); // part 1

        for (int i = 0; i < instructions.Length; i++)
        {
            if (instructions[i].StartsWith("nop") || instructions[i].StartsWith("jmp"))
            {
                var copy = instructions.ToArray();
                if (instructions[i].StartsWith("nop"))
                {
                    copy[i] = copy[i].Replace("nop", "jmp");
                }
                else
                {
                    copy[i] = copy[i].Replace("jmp", "nop");
                }

                (int ip, int acc) = Run(copy);
                if (ip == instructions.Length)
                {
                    Console.WriteLine((ip,acc)); // part 2
                    break;
                }

            }
        }
    }

    static (int,int) Run(string[] instructions)
    {
        var visited = new HashSet<int>();

        var acc = 0;
        var ip = 0;
        while (ip < instructions.Length)
        {
            if (visited.Contains(ip)) { break; }
            visited.Add(ip);

            var ins = instructions[ip];
            (ip,acc) = ins.Split(' ') switch
            {
                ["acc", var val]    => (ip + 1, acc + int.Parse(val)),
                ["jmp", var val]    => (ip += int.Parse(val), acc),
                ["nop", _]    => (ip + 1, acc),
                _ => throw new InvalidDataException(ins)
            };
        }

        return (ip,acc);
    }
}
