using System.Diagnostics;

namespace AdventOfCode;

internal class Day15
{
    record struct Lens(string Label, int FocalLength);

    record Box
    {
        public LinkedList<Lens> Lenses { get; } = [];
    }

    static int Hash(ReadOnlySpan<char> s)
    {
        /*
         * The HASH algorithm is a way to turn any string of characters into a single number in the range 0 to 255.
         * To run the HASH algorithm on a string, start with a current value of 0.
         * Then, for each character in the string starting from the beginning:
         *
         *  Determine the ASCII code for the current character of the string.
         *  Increase the current value by the ASCII code you just determined.
         *  Set the current value to itself multiplied by 17.
         *  Set the current value to the remainder of dividing itself by 256.
        */
        var current = 0;
        foreach (var c in s)
        {
            if (c == '\n') continue;
            current += c;
            current *= 17;
            current %= 256;
        }

        Debug.Assert(current < 256, $"Hash must be in range 0-255 but was {current}");
        return current;
    }

    static long ComputeFocusPower(List<Box> boxes)
    {
        /*
         * To confirm that all of the lenses are installed correctly,
         * add up the focusing power of all of the lenses.
         * 
         * The focusing power of a single lens is the result of multiplying together:
         *  - One plus the box number of the lens in question.
         *  - The slot number of the lens within the box: 1 for the first lens, 2 for the second lens, and so on.
         *  - The focal length of the lens.
         */

        var focusPower = 0;
        for (var box = 0; box < boxes.Count; box++)
        {
            for (var slot = 0; slot < boxes[box].Lenses.Count; slot++)
            {
                focusPower += (box + 1) * (slot + 1) * boxes[box].Lenses.ElementAt(slot).FocalLength;
            }
        }

        return focusPower;
    }

    public static void SolvePart1()
    {
        //var input = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";
        var input = File.ReadAllText(@"2023_15_input.txt");
        var sum = input.Split(',').Sum(s => Hash(s));
        Console.WriteLine($"Part 1: {sum}");
    }

    public static void SolvePart2()
    {
        //var input = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";
        var input = File.ReadAllText(@"2023_15_input.txt");
        
        char[] splitters = [',', '\n'];
        var operations = input.Split(splitters, StringSplitOptions.RemoveEmptyEntries);

        var boxes = new List<Box>(255);
        for (var i  = 0; i < 256; i++)
        {
            boxes.Add(new());
        }

        foreach (var operation in operations)
        {
            if (operation[operation.Length - 1] == '-')
            {
                var label = operation[..(operation.Length - 1)];
                var box = Hash(label);

                var lens = boxes[box].Lenses.FirstOrDefault(l => l.Label == label);
                if (lens.Label != null)
                {
                    var removed = boxes[box].Lenses.Remove(lens);
                    Debug.Assert(removed, "Lens should have been removed!");
                }
            }
            else
            {
                var delimiter = operation.IndexOf('=');
                var label = operation[..delimiter];
                var focalLength = int.Parse(operation[(delimiter+1)..]);
                var box = Hash(label);

                var lens = boxes[box].Lenses.FirstOrDefault(l => l.Label == label);
                if (lens.Label != null)
                {
                    var node = boxes[box].Lenses.Find(lens);
                    node!.ValueRef.FocalLength = focalLength;
                }
                else
                {
                    boxes[box].Lenses.AddLast(new Lens(label, focalLength));
                }
            }
        }

        for (var box = 0; box < boxes.Count; box++)
        {
            if (boxes[box].Lenses.Count > 0)
            {
                Console.WriteLine($"Box {box}:");
                foreach (var lens in boxes[box].Lenses)
                {
                    Console.WriteLine($"  {lens}");
                }
            }
        }

        var totalFocusPower = ComputeFocusPower(boxes);
        Console.WriteLine($"Total focus power: {totalFocusPower}");
    }
}
