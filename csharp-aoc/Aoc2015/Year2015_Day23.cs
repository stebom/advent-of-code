namespace AdventOfCode;

internal class Year2015_Day23
{
    internal static void Solve()
    {
        uint a = 1;
        uint b = 0;

        string[] instructions = [
            "jio a, +16",
            "inc a",
            "inc a",
            "tpl a",
            "tpl a",
            "tpl a",
            "inc a",
            "inc a",
            "tpl a",
            "inc a",
            "inc a",
            "tpl a",
            "tpl a",
            "tpl a",
            "inc a",
            "jmp +23",
            "tpl a",
            "inc a",
            "inc a",
            "tpl a",
            "inc a",
            "inc a",
            "tpl a",
            "tpl a",
            "inc a",
            "inc a",
            "tpl a",
            "inc a",
            "tpl a",
            "inc a",
            "tpl a",
            "inc a",
            "inc a",
            "tpl a",
            "inc a",
            "tpl a",
            "tpl a",
            "inc a",
            "jio a, +8",
            "inc b",
            "jie a, +4",
            "tpl a",
            "inc a",
            "jmp +2",
            "hlf a",
            "jmp -7",
        ];

        for (var i  = 0; i < instructions.Length;)
        {
            var instruction = instructions[i];
            var offset = 1;

            switch (instruction[0..3]) 
            {
                case "hlf":
                    if (instruction[4] == 'a') a /= 2;
                    else if (instruction[4] == 'b') b /= 2;
                    else throw new InvalidOperationException(instruction);
                    break;
                case "tpl":
                    if (instruction[4] == 'a') a *= 3;
                    else if (instruction[4] == 'b') b *= 3;
                    else throw new InvalidOperationException(instruction);
                    break;
                case "inc":
                    if (instruction[4] == 'a') a++;
                    else if (instruction[4] == 'b') b++;
                    else throw new InvalidOperationException(instruction);
                    break;
                case "jmp":
                    offset = int.Parse(instruction[4..]);
                    break;
                case "jie":
                    { 
                        var register = instruction[4] switch
                        {
                            'a' => a,
                            'b' => b,
                            _ => throw new InvalidOperationException(instruction)
                        };

                        if (register % 2 == 0) offset = int.Parse(instruction[7..]);
                    }
                    break;
                case "jio":
                    { 
                        var register = instruction[4] switch
                        {
                            'a' => a,
                            'b' => b,
                            _ => throw new InvalidOperationException(instruction)
                        };
                        if (register == 1) offset = int.Parse(instruction[7..]);
                    }
                    break;
                default:
                    throw new InvalidOperationException(instruction);
            }

            i += offset;
        }

        Console.WriteLine($"Part 1: {b}");
    }
}