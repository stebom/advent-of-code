using System.Collections.Generic;

namespace AdventOfCode;

internal class Day22
{
    static readonly string[] TestInput = [
        "1,0,1~1,2,1",
        "0,0,2~2,0,2",
        "0,2,3~2,2,3",
        "0,0,4~0,2,4",
        "2,0,5~2,2,5",
        "0,1,6~2,1,6",
        "1,1,8~1,1,9",
    ];

    record struct Cube(int X, int Y, int Z);

    record Brick(char Name)
    {
        public List<Cube> Cubes { get; set; } = [];
    }

    static Cube ToCube(int[] val) => new(val[0], val[1], val[2]);

    static List<Brick> CreateBricks(string[] input)
    {
        List<Brick> bricks = [];
        var brickIndex = 0;

        foreach (var line in input)
        {
            var delimiter = line.IndexOf('~');
            var from = ToCube(line[0..delimiter].Split(',').Select(int.Parse).ToArray());
            var to = ToCube(line[(delimiter + 1)..].Split(',').Select(int.Parse).ToArray());

            var startX = Math.Min(from.X, to.X);
            var endX = Math.Max(from.X, to.X);

            var startY = Math.Min(from.Y, to.Y);
            var endY = Math.Max(from.Y, to.Y);

            var startZ = Math.Min(from.Z, to.Z);
            var endZ = Math.Max(from.Z, to.Z);

            var brick = new Brick((char)('A' + brickIndex));

            for (var x = startX; x <= endX; x++)
            {
                for (var y = startY; y <= endY; y++)
                {
                    for (var z = startZ; z <= endZ; z++)
                    {
                        brick.Cubes.Add(new Cube(x, y, z));
                    }
                }
            }

            bricks.Add(brick);

            brickIndex++;
        }

        return bricks;
    }

    static void Compress(List<Brick> bricks)
    {
        var moved = true;
        while (moved)
        {
            moved = false;
            foreach (var brick in bricks)
            {
                var movedCubes = brick.Cubes.Select(c => new Cube(c.X, c.Y, c.Z - 1)).ToList();

                // Check collision with floor
                if (movedCubes.Any(c => c.Z < 1)) continue;

                var cubes = bricks.Where(b => b != brick).SelectMany(b => b.Cubes); //.ToHashSet(); // possible optimization?
                var collides = movedCubes.Any(cubes.Contains);

                // Check collision with other bricks
                if (!collides)
                {
                    brick.Cubes = movedCubes;
                    moved = true;
                }
            }
        }
    }

    static List<Brick> Supports(List<Brick> bricks, Brick brick)
    {
        var supports = new List<Brick>();

        foreach (var cube in brick.Cubes)
        {
            var upper = new Cube(cube.X, cube.Y, cube.Z + 1);

            foreach (var supportedBrick in bricks)
            {
                if (supportedBrick != brick && supportedBrick.Cubes.Contains(upper))
                {
                    supports.Add(supportedBrick);
                }
            }
        }

        return supports;
    }

    static List<Brick> SupportedBy(List<Brick> bricks, Brick brick)
    {
        var supports = new List<Brick>();

        foreach (var cube in brick.Cubes)
        {
            var upper = new Cube(cube.X, cube.Y, cube.Z - 1);

            foreach (var supportedBrick in bricks)
            {
                if (supportedBrick != brick && supportedBrick.Cubes.Contains(upper))
                {
                    supports.Add(supportedBrick);
                }
            }
        }

        return supports;
    }

    static bool CanDisintegrated(List<Brick> bricks, Brick brick)
    {
        var supports = Supports(bricks, brick);

        if (supports.Count == 0)
        {
            //Console.WriteLine($"Brick {brick.Name} can be disintegrated; it does not support any other bricks.");
            return true;
        }

        var canDisintegrated = true;
        foreach (var support in supports)
        {
            var supportedBy = SupportedBy(bricks, support).Where(b => b != brick).ToList();

            if (supportedBy.Count > 0)
            {
                //Console.WriteLine($"Brick {brick.Name} can be disintegrated; the brick above it ({support.Name}) would still be supported by brick {string.Join(',', supportedBy.Select(b => b.Name))}.");
            }
            else
            {
                //Console.WriteLine($"Brick {brick.Name} cannot be disintegrated; the brick above it ({support.Name}) would fall.");
                canDisintegrated = false;
                return canDisintegrated;
            }
        }

        return canDisintegrated;
    }

    public static void Solve()
    {
        //var input = TestInput;
        var input = File.ReadAllLines(@"2023_22_input.txt");

        //Part1(input);
        Part2(input);
    }

    static void Part1(string[] input)
    {
        var bricks = CreateBricks(input);
        Compress(bricks);

        var count = 0;
        foreach (var brick in bricks)
        {
            count += CanDisintegrated(bricks, brick) ? 1 : 0;
        }
        Console.WriteLine($"{count} can be disintegrated");
    }

    static void Disintegrate(List<Brick> bricks, Brick brick)
    {
        List<Brick> chained = [];
        foreach (var supported in Supports(bricks, brick))
        {
            var supportedBy = SupportedBy(bricks, supported).Where(b => b != brick).ToList();
            if (supportedBy.Count == 0)
            {
                bricks.Remove(supported);
                chained.Add(supported);
            }
        }

        foreach (var chain in chained)
        {
            Disintegrate(bricks, chain);
        }
    }

    static void Part2(string[] input)
    {
        var bricks = CreateBricks(input);
        Compress(bricks);

        var total = 0;
        foreach (var brick in bricks)
        {
            List<Brick> b = [.. bricks];
            Disintegrate(b, brick);
            total += bricks.Count - b.Count;
            Console.WriteLine($"Disintegrating {brick.Name} cause {bricks.Count - b.Count} bricks to fall");
        }

        Console.WriteLine($"Sum: {total}");
    }
}