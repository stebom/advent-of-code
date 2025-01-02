namespace Aoc2018;

static class Day11
{
    const int SerialNumber = 7403;
    const int SideLength = 300;

    static readonly long[][] Grid = Enumerable.Range(1, SideLength).Select(x => Enumerable.Range(1, SideLength).Select(y =>
                                    CalculatePowerLevel(y, x, SerialNumber)).ToArray()).ToArray();

    static long CalculatePowerLevel(int x, int y, int serialNumber)
    {
        // Find the fuel cell's rack ID, which is its X coordinate plus 10.
        // Begin with a power level of the rack ID times the Y coordinate.
        // Increase the power level by the value of the grid serial number (your puzzle input).
        // Set the power level to itself multiplied by the rack ID.
        // Keep only the hundreds digit of the power level (so 12345 becomes 3; numbers with no hundreds digit become 0).
        // Subtract 5 from the power level.
        var rackId = x + 10;

        var powerLevel = rackId * y;
        powerLevel += serialNumber;
        powerLevel *= rackId;
        powerLevel = (powerLevel / 100) % 10;
        powerLevel -= 5;
        return powerLevel;
    }

    public static void Run()
    {
        Part1();
        Part2();
    }

    static void Part1()
    {
        long best = 0;
        (int X, int Y) coordinate = (-1, -1);

        for (var i = 1; i < SideLength - 1; i++)
        {
            for (var y = 1; y < SideLength - 1; y++)
            {
                var sum = Grid[i - 1][y - 1] + Grid[i - 1][y] + Grid[i - 1][y + 1] +
                          Grid[i][y - 1] + Grid[i][y] + Grid[i][y + 1] +
                          Grid[i + 1][y - 1] + Grid[i + 1][y] + Grid[i + 1][y + 1];

                if (sum > best)
                {
                    best = sum;
                    coordinate = (y, i);
                }
            }
        }
        Console.WriteLine($"Part 1: {coordinate.X},{coordinate.Y} has power {best}");
    }

    static void Part2()
    {
        Dictionary<(int X, int Y), long> sat = [];

        for (var x = 0; x < SideLength; x++)
        {
            for (var y = 0; y < SideLength; y++)
            {
                if (!sat.TryGetValue((x, y - 1), out long n)) { n = 0; }
                if (!sat.TryGetValue((x - 1, y), out long w)) { w = 0; }
                if (!sat.TryGetValue((x - 1, y - 1), out long nw)) { nw = 0; }

                sat[(x, y)] = Grid[x][y] + n + w - nw;
            }
        }

        Dictionary<(int X, int Y,int Size), long> quadrants = [];

        for (var i = 0; i < SideLength; i++)
        {
            for (var y = 0; y < SideLength; y++)
            {
                quadrants[(y, i, 1)] = Grid[i][y];

                var size = 2;
                while (true)
                {
                    if ((i + size) >= Grid.Length || (y + size) >= SideLength) break;

                    long quadrantSize = sat[(i + size, y + size)] + sat[(i, y)] - sat[(i, y + size)] - sat[(i + size, y)];
                    quadrants[(y+2, i+2, size)] = quadrantSize;
                    size++;
                }
            }
        }

        var best = quadrants.MaxBy(kvp => kvp.Value);
        Console.WriteLine($"Part 2: {best.Key.X},{best.Key.Y},{best.Key.Size} has power {best.Value}");
    }
}
