
namespace Aoc2024;

public static class Day9
{
    public static void Solve()
    {
        var input = File.ReadAllLines(@"input/day9_input.txt").Single().ToCharArray();
        Console.WriteLine($"Part 1: {Checksum(Fragment(Expand(input)))}");
        Console.WriteLine($"Part 2: {Checksum(MoveFiles(Expand(input)))}");
    }

    private static long Checksum(char[] input)
    {
        long result = 0;
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] == '.') continue;

            var id = input[i] - '0';
            result += i * id;
        }

        return result;
    }

    private static char[] MoveFiles(char[] input)
    {
        var rightPointer = input.Length - 1;
        var current = input[rightPointer];
        var size = 0;

        while (rightPointer > 0)
        {
            if (input[rightPointer] == current) size++;
            else
            {
                if (current != '.')
                {
                    var slot = FindSpace(input, size);
                    if (slot != -1 && slot < rightPointer)
                    {
                        Array.Fill(input, current, slot, size);
                        Array.Fill(input, '.', rightPointer + 1, size);
                    }
                }
                current = input[rightPointer];
                size = 1;
            }

            rightPointer--;
        }

        return input;
    }
    private static int FindSpace(char[] input, int space)
    {
        var pointer = 0;
        var size = 0;
        while (pointer < input.Length)
        {
            if (input[pointer] != '.')
            {
                if (size >= space) return pointer - size;
                size = 0;
            }
            else size++;
            pointer++;
        }

        if (size >= space) return pointer - size;
        return -1;
    }

    private static char[] Fragment(char[] input)
    {
        var leftPointer = 0;
        var rightPointer = input.Length - 1;

        while (leftPointer < rightPointer)
        {
            if (input[leftPointer] != '.')
            {
                leftPointer++;
            }
            if (input[rightPointer] == '.')
            {
                rightPointer--;
            }

            if (input[leftPointer] == '.' && input[rightPointer] != '.')
            {
                (input[leftPointer], input[rightPointer]) = (input[rightPointer], input[leftPointer]);
            }
        }

        return input;
    }

    private static char[] Expand(char[] input)
    {
        var pointer = 0;
        var buffer = new List<char>(1_000_000);

        while (pointer < input.Length)
        {
            var value = input[pointer] - '0';
            var even = (pointer & 1) == 0;

            for (var i = 0; i < value; i++)
            {
                buffer.Add(even ? (char)(pointer/2 + '0') : '.');
            }
            pointer++;
        }

        return [.. buffer];
    }
}
