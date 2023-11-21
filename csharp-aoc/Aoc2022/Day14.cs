
using System.Diagnostics;
using System.Linq;

namespace Day14;

public static class Day14
{    
    public static void Run()
    {
        const string input = "input.txt";

        var rocks = File.ReadAllText(input)
            .Split("\n")
            .SelectMany(l =>
            {
                var positions = l.Split(new[] { ",", " -> " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Chunk(2);
                var r = new List<(int r, int d)> { (positions.First()[0], positions.First()[1]) };
                foreach (var position in positions.Skip(1).SelectMany(pos => Walk(r.Last(), (pos[0], pos[1]))))
                    r.Add(position);

                return r;
            }).ToHashSet();

        var ticks = 0;
        var floor = rocks.Max(r => r.d) + 2;
        var hitFloor = false;

        while (!rocks.Contains((500, 0)))
        {
            (int r, int d) sand = (500, 0);
            //Console.WriteLine($"Spawning sand at {sand.d},{sand.r}");

            while (true)
            {
                if (!hitFloor && sand.d + 1 == floor)
                {
                    Console.WriteLine("part1: " + ticks);
                    hitFloor = true;
                }

                if (!isOccupied((sand.r, sand.d + 1)))
                {
                    sand.d++;
                    //Console.WriteLine($"Moving sand down to {sand.d},{sand.r}");
                }
                else if (!isOccupied((sand.r - 1, sand.d + 1)))
                {
                    sand.d++; sand.r--;
                    //Console.WriteLine($"Moving sand down left to {sand.d},{sand.r}");
                }
                else if (!isOccupied((sand.r + 1, sand.d + 1)))
                {
                    sand.d++; sand.r++;
                    //Console.WriteLine($"Moving sand down right to {sand.d},{sand.r}");
                }
                else 
                {
                    //Console.WriteLine($"Sand comes to rest at {sand.d},{sand.r}");
                    break; // comes to rest
                }
            }

            rocks.Add(sand);
            ticks++;
        }

        Console.WriteLine("part2: " + ticks);

        bool isOccupied((int r, int d) pos) => pos.d == floor || rocks.Contains(pos);
    }

    
    static IEnumerable<(int r, int d)> Walk((int r, int d) from, (int r, int d) to)
    {
        var dr = to.r - from.r;
        var dd = to.d - from.d;

        Debug.Assert(dr != 0 ^ dd != 0, "moving diagonally");

        if (dr > 0)
            for (var r = from.r + 1; r <= to.r; r++)
                yield return (r, from.d);
        if (dr < 0)
            for (var r = from.r - 1; r >= to.r; r--)
                yield return (r, from.d);
        else if (dd > 0)
            for (var d = from.d + 1; d <= to.d; d++)
                yield return (from.r, d);
        else if (dd < 0)
            for (var d = from.d - 1; d >= to.d; d--)
                yield return (from.r, d);
    }
}