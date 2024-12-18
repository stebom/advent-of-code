namespace Aoc2024;

public static class Day17
{
    record Registers
    {
        public required int A { get; set; }
        public required int B { get; set; }
        public required int C { get; set; }
    }

    record Program(int[] Instructions, Registers Registers, List<int> Output)
    {
        public int InstructionPointer { get; set; }

        public int Instruction => Instructions[InstructionPointer];

        public int Operand => Instructions[InstructionPointer + 1];

        public int ComboOperand => Operand switch
        {
            // Combo operands 0 through 3 represent literal values 0 through 3.
            0 or 1 or 2 or 3 => Operand,
            // Combo operand 4 represents the value of register A.
            4 => Registers.A,
            // Combo operand 5 represents the value of register B.
            5 => Registers.B,
            // Combo operand 6 represents the value of register C.
            6 => Registers.C,
            // Combo operand 7 is reserved and will not appear in valid programs.
            _ => throw new Exception($"{Operand}")
        };

        public bool Running => InstructionPointer < Instructions.Length;

        public void Run() {
            while (Running) ProcessInstruction();
        }

        private void ProcessInstruction()
        {
            // The adv instruction (opcode 0) performs division.
            // The numerator is the value in the A register.
            // The denominator is found by raising 2 to the power of the instruction's combo operand.
            // 
            // (So, an operand of 2 would divide A by 4 (2^2); an operand of 5 would divide A by 2^B.)
            // The result of the division operation is truncated to an integer and then written to the A register.
            if (Instruction is 0)
            {
                var numerator = Registers.A;
                var denominator = Math.Pow(2, ComboOperand);
                Registers.A = (int)(numerator / denominator);
            }
            // The bxl instruction (opcode 1) calculates the bitwise XOR of register B and the instruction's literal operand,
            // then stores the result in register B.
            else if (Instruction is 1)
            {
                Registers.B = Registers.B ^ Operand;
            }
            // The bst instruction (opcode 2) calculates the value of its combo operand modulo 8 (thereby keeping only its lowest 3 bits),
            // then writes that value to the B register.
            else if (Instruction is 2)
            {
                Registers.B = ComboOperand % 8;
            }
            // The jnz instruction (opcode 3) does nothing if the A register is 0.
            // However, if the A register is not zero, it jumps by setting the instruction pointer to the value of its literal operand;
            // if this instruction jumps, the instruction pointer is not increased by 2 after this instruction.
            else if (Instruction is 3)
            {
                if (Registers.A != 0)
                {
                    InstructionPointer = Operand;
                    return;
                }
            }
            // The bxc instruction (opcode 4) calculates the bitwise XOR of register B and register C,
            // then stores the result in register B. (For legacy reasons, this instruction reads an operand but ignores it.)
            else if (Instruction is 4)
            {
                Registers.B = Registers.B ^ Registers.C;
            }
            // The out instruction (opcode 5) calculates the value of its combo operand modulo 8, then outputs that value.
            else if (Instruction is 5)
            {
                Output.Add(ComboOperand % 8);
            }
            // The bdv instruction (opcode 6) works exactly like the adv instruction except that the result is stored in the B register.
            else if (Instruction is 6)
            {
                var numerator = Registers.A;
                var denominator = Math.Pow(2, ComboOperand);
                Registers.B = (int)(numerator / denominator);
            }
            // The cdv instruction (opcode 7) works exactly like the adv instruction except that the result is stored in the C register.
            else if (Instruction is 7)
            {
                var numerator = Registers.A;
                var denominator = Math.Pow(2, ComboOperand);
                Registers.C = (int)(numerator / denominator);
            }

            InstructionPointer += 2;
        }
    }

    public static void Solve()
    {
        int[] instructions = [2, 4, 1, 1, 7, 5, 0, 3, 1, 4, 4, 5, 5, 5, 3, 0];
        Part1(new Program(instructions, new Registers { A = 51571418, B = 0, C = 0 }, []));
        Part2(instructions);
    }

    private static void Part1(Program program)
    {
        program.Run();
        Console.WriteLine($"{program.Registers}");
        Console.WriteLine($"Output: {string.Join(",", program.Output)}");
    }
    private static void Part2(int[] instructions)
    {
        var registerA = 0;
        while (true)
        {
            var program = new Program(instructions, new Registers { A = registerA, B = 0, C = 0 }, []);
            program.Run();
            if (instructions.SequenceEqual(program.Output)) break;
            registerA++;
        }
        
        Console.WriteLine($"Part 2: {registerA}");
    }
}