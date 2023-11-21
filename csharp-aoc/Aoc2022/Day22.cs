using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Day22;

public class Day22
{
    private static readonly bool Test = false;
    
    private static readonly bool Part2 = true;

    private static readonly Dictionary<int, bool[][]> Maps = new();

    private static readonly List<string> Movements = new();

    private static readonly Dictionary<(int Map, Direction Direction), (int Map, Direction Direction)> MapConnections = new();

    public static void Run()
    {
        var parts = File.ReadAllText(Test ? "testinput_day22.txt" : "input_day22.txt").Split("\n\n");

        var lines = parts[0].Split('\n');

        if (Test) {
            // Grid 4x3:
            //
            //  - - 1 -
            //  2 3 4 -
            //  - - 5 6
            //

            var mapWidth = lines.Max(l => l.Length) / 4;
            var mapHeight = lines.Length / 3;

            Maps.Add(1, Enumerable.Range(0, mapHeight).Select(row =>
                        Enumerable.Range(mapWidth * 2, mapWidth).Select(col => Cell(lines[row][col])).ToArray()).ToArray());

            Maps.Add(2, Enumerable.Range(mapHeight, mapHeight).Select(row =>
                        Enumerable.Range(0, mapWidth).Select(col => Cell(lines[row][col])).ToArray()).ToArray());

            Maps.Add(3, Enumerable.Range(mapHeight, mapHeight).Select(row =>
                        Enumerable.Range(mapWidth, mapWidth).Select(col => Cell(lines[row][col])).ToArray()).ToArray());

            Maps.Add(4, Enumerable.Range(mapHeight, mapHeight).Select(row =>
                        Enumerable.Range(mapWidth * 2, mapWidth).Select(col => Cell(lines[row][col])).ToArray()).ToArray());

            Maps.Add(5, Enumerable.Range(mapHeight * 2, mapHeight).Select(row =>
                        Enumerable.Range(mapWidth * 2, mapWidth).Select(col => Cell(lines[row][col])).ToArray()).ToArray());

            Maps.Add(6, Enumerable.Range(mapHeight * 2, mapHeight).Select(row =>
                        Enumerable.Range(mapWidth * 3, mapWidth).Select(col => Cell(lines[row][col])).ToArray()).ToArray());

            if (Part2) {
                MapConnections.Add((1, Direction.Up), (2, Direction.Down));
                MapConnections.Add((1, Direction.Right), (6, Direction.Left));
                MapConnections.Add((1, Direction.Down), (4, Direction.Down));
                MapConnections.Add((1, Direction.Left), (3, Direction.Down));

                MapConnections.Add((2, Direction.Up), (1, Direction.Down));
                MapConnections.Add((2, Direction.Right), (3, Direction.Right));
                MapConnections.Add((2, Direction.Down), (5, Direction.Up));
                MapConnections.Add((2, Direction.Left), (6, Direction.Up));

                MapConnections.Add((3, Direction.Up), (1, Direction.Right));
                MapConnections.Add((3, Direction.Right), (4, Direction.Right));
                MapConnections.Add((3, Direction.Down), (5, Direction.Right));
                MapConnections.Add((3, Direction.Left), (2, Direction.Left));

                MapConnections.Add((4, Direction.Up), (1, Direction.Up));
                MapConnections.Add((4, Direction.Right), (6, Direction.Down));
                MapConnections.Add((4, Direction.Down), (5, Direction.Down));
                MapConnections.Add((4, Direction.Left), (3, Direction.Left));

                MapConnections.Add((5, Direction.Up), (4, Direction.Up));
                MapConnections.Add((5, Direction.Right), (6, Direction.Right));
                MapConnections.Add((5, Direction.Down), (2, Direction.Up));
                MapConnections.Add((5, Direction.Left), (3, Direction.Up));

                MapConnections.Add((6, Direction.Up), (4, Direction.Left));
                MapConnections.Add((6, Direction.Right), (1, Direction.Left));
                MapConnections.Add((6, Direction.Down), (2, Direction.Right));
                MapConnections.Add((6, Direction.Left), (5, Direction.Left));
            }
            else { 
                MapConnections.Add((1, Direction.Up), (5, Direction.Up));
                MapConnections.Add((1, Direction.Left), (1, Direction.Left));
                MapConnections.Add((1, Direction.Right), (1, Direction.Right));
                MapConnections.Add((1, Direction.Down), (4, Direction.Down));

                MapConnections.Add((2, Direction.Up), (2, Direction.Up));
                MapConnections.Add((2, Direction.Left), (4, Direction.Left));
                MapConnections.Add((2, Direction.Right), (3, Direction.Right));
                MapConnections.Add((2, Direction.Down), (2, Direction.Down));

                MapConnections.Add((3, Direction.Up), (1, Direction.Up));
                MapConnections.Add((3, Direction.Left), (2, Direction.Left));
                MapConnections.Add((3, Direction.Right), (4, Direction.Right));
                MapConnections.Add((3, Direction.Down), (3, Direction.Down));

                MapConnections.Add((4, Direction.Up), (1, Direction.Up));
                MapConnections.Add((4, Direction.Left), (3, Direction.Left));
                MapConnections.Add((4, Direction.Right), (2, Direction.Right));
                MapConnections.Add((4, Direction.Down), (5, Direction.Down));

                MapConnections.Add((5, Direction.Up), (4, Direction.Up));
                MapConnections.Add((5, Direction.Left), (6, Direction.Left));
                MapConnections.Add((5, Direction.Right), (6, Direction.Right));
                MapConnections.Add((5, Direction.Down), (1, Direction.Down));

                MapConnections.Add((6, Direction.Up), (6, Direction.Up));
                MapConnections.Add((6, Direction.Left), (5, Direction.Left));
                MapConnections.Add((6, Direction.Right), (5, Direction.Right));
                MapConnections.Add((6, Direction.Down), (6, Direction.Down));
            }
        }
        else {
            // Grid 3x4:
            //
            //  - 1 2
            //  - 3 -
            //  4 5 -
            //  6 - -
            //

            var mapWidth = lines.Max(l => l.Length) / 3;
            var mapHeight = lines.Length / 4;

            Maps.Add(1, Enumerable.Range(0, mapHeight).Select(row =>
                        Enumerable.Range(mapWidth, mapWidth).Select(col => Cell(lines[row][col])).ToArray()).ToArray());

            Maps.Add(2, Enumerable.Range(0, mapHeight).Select(row =>
                        Enumerable.Range(mapWidth * 2, mapWidth).Select(col => Cell(lines[row][col])).ToArray()).ToArray());

            Maps.Add(3, Enumerable.Range(mapHeight, mapHeight).Select(row =>
                        Enumerable.Range(mapWidth, mapWidth).Select(col => Cell(lines[row][col])).ToArray()).ToArray());

            Maps.Add(4, Enumerable.Range(mapHeight * 2, mapHeight).Select(row =>
                        Enumerable.Range(0, mapWidth).Select(col => Cell(lines[row][col])).ToArray()).ToArray());

            Maps.Add(5, Enumerable.Range(mapHeight * 2, mapHeight).Select(row =>
                        Enumerable.Range(mapWidth, mapWidth).Select(col => Cell(lines[row][col])).ToArray()).ToArray());

            Maps.Add(6, Enumerable.Range(mapHeight * 3, mapHeight).Select(row =>
                        Enumerable.Range(0, mapWidth).Select(col => Cell(lines[row][col])).ToArray()).ToArray());

            if (Part2) {
                MapConnections.Add((1, Direction.Up), (6, Direction.Right));
                MapConnections.Add((1, Direction.Right), (2, Direction.Right));
                MapConnections.Add((1, Direction.Down), (3, Direction.Down));
                MapConnections.Add((1, Direction.Left), (4, Direction.Right));

                MapConnections.Add((2, Direction.Up), (6, Direction.Up));
                MapConnections.Add((2, Direction.Right), (5, Direction.Left));
                MapConnections.Add((2, Direction.Down), (3, Direction.Left));
                MapConnections.Add((2, Direction.Left), (1, Direction.Left));

                MapConnections.Add((3, Direction.Up), (1, Direction.Up));
                MapConnections.Add((3, Direction.Right), (2, Direction.Up));
                MapConnections.Add((3, Direction.Down), (5, Direction.Down));
                MapConnections.Add((3, Direction.Left), (4, Direction.Down));

                MapConnections.Add((4, Direction.Up), (3, Direction.Right));
                MapConnections.Add((4, Direction.Right), (5, Direction.Right));
                MapConnections.Add((4, Direction.Down), (6, Direction.Down));
                MapConnections.Add((4, Direction.Left), (1, Direction.Right));

                MapConnections.Add((5, Direction.Up), (3, Direction.Up));
                MapConnections.Add((5, Direction.Right), (2, Direction.Left));
                MapConnections.Add((5, Direction.Down), (6, Direction.Left));
                MapConnections.Add((5, Direction.Left), (4, Direction.Left));

                MapConnections.Add((6, Direction.Up), (4, Direction.Up));
                MapConnections.Add((6, Direction.Right), (5, Direction.Up));
                MapConnections.Add((6, Direction.Down), (2, Direction.Down));
                MapConnections.Add((6, Direction.Left), (1, Direction.Down));

            } else { 
                MapConnections.Add((1, Direction.Up), (5, Direction.Up));
                MapConnections.Add((1, Direction.Left), (2, Direction.Left));
                MapConnections.Add((1, Direction.Right), (2, Direction.Right));
                MapConnections.Add((1, Direction.Down), (3, Direction.Down));

                MapConnections.Add((2, Direction.Up), (2, Direction.Up));
                MapConnections.Add((2, Direction.Left), (1, Direction.Left));
                MapConnections.Add((2, Direction.Right), (1, Direction.Right));
                MapConnections.Add((2, Direction.Down), (2, Direction.Down));

                MapConnections.Add((3, Direction.Up), (1, Direction.Up));
                MapConnections.Add((3, Direction.Left), (3, Direction.Left));
                MapConnections.Add((3, Direction.Right), (3, Direction.Right));
                MapConnections.Add((3, Direction.Down), (5, Direction.Down));

                MapConnections.Add((4, Direction.Up), (6, Direction.Up));
                MapConnections.Add((4, Direction.Left), (5, Direction.Left));
                MapConnections.Add((4, Direction.Right), (5, Direction.Right));
                MapConnections.Add((4, Direction.Down), (6, Direction.Down));

                MapConnections.Add((5, Direction.Up), (3, Direction.Up));
                MapConnections.Add((5, Direction.Left), (4, Direction.Left));
                MapConnections.Add((5, Direction.Right), (4, Direction.Right));
                MapConnections.Add((5, Direction.Down), (1, Direction.Down));

                MapConnections.Add((6, Direction.Up), (4, Direction.Up));
                MapConnections.Add((6, Direction.Left), (6, Direction.Left));
                MapConnections.Add((6, Direction.Right), (6, Direction.Right));
                MapConnections.Add((6, Direction.Down), (4, Direction.Down));
            }
        }

        // Assume height and width are the same
        var sideWidth = Maps[1].Length;

        // Parse steps and directions
        Movements.AddRange(Regex.Split(parts[1], @"(\d+[RL])").Where(l => !string.IsNullOrEmpty(l)));

        // Current position is the start facing right
        (int R, int C, int Map, Direction Dir) position = (0, 0, 1, Direction.Right);

        foreach (var movement in Movements)
        {                        
            var steps = int.Parse(movement.Substring(0, movement.Length - (movement.EndsWith('R') || movement.EndsWith('L') ? 1 : 0)));

            //Console.WriteLine($"Walking {steps} steps from ({position.R}, {position.C}) on map {position.Map} facing {position.Dir}");

            foreach (var _ in Enumerable.Range(0, steps))
            {
                var next = (position.R, position.C, position.Map, position.Dir);

                if (position.Dir == Direction.Up) next.R -= 1;
                if (position.Dir == Direction.Right) next.C += 1;
                if (position.Dir == Direction.Down) next.R += 1;
                if (position.Dir == Direction.Left) next.C -= 1;

                if (next.R < 0 || next.R >= sideWidth || next.C < 0 || next.C >= sideWidth) {

                    var connection = MapConnections[(position.Map, position.Dir)];
                    Console.WriteLine($"Walk off map {position.Map} onto {connection.Map} in direction {position.Dir}");

                    if (!Part2) Debug.Assert(connection.Direction == position.Dir, "Rotation not implemented");

                    if (next.R < 0) next.R = sideWidth - 1;
                    else next.R %= sideWidth;
                    if (next.C < 0) next.C = sideWidth - 1;
                    else next.C %= sideWidth;

                    if (position.Dir != connection.Direction) // handle rotation between maps
                    {
                        Console.WriteLine($"Direction is being rotated: {position.Dir} -> {connection.Direction}");

                        if (position.Dir == Direction.Down && connection.Direction == Direction.Up)
                        {
                            next.R = position.R;
                            next.C = sideWidth - 1 - position.C;
                        }
                        else if (position.Dir == Direction.Up && connection.Direction == Direction.Right)
                        {
                            next.R = position.C;
                            next.C = 0;
                        }
                        else if (position.Dir == Direction.Right && connection.Direction == Direction.Up)
                        {
                            next.R = sideWidth - 1;
                            next.C = position.R;
                        }
                        else if (position.Dir == Direction.Right && connection.Direction == Direction.Down)
                        {
                            next.R = 0;
                            next.C = sideWidth - 1 - position.R;
                        }
                        else if (position.Dir == Direction.Down && connection.Direction == Direction.Left)
                        {
                            next.R = position.C;
                            next.C = sideWidth - 1;
                        }
                        else if (position.Dir == Direction.Left && connection.Direction == Direction.Right)
                        {
                            next.R = sideWidth - 1 - position.R;
                            next.C = 0;
                        }
                        else if (position.Dir == Direction.Left && connection.Direction == Direction.Down)
                        {
                            next.R = 0;
                            next.C = position.R;
                        }
                        else if (position.Dir == Direction.Right && connection.Direction == Direction.Left)
                        {
                            next.R = sideWidth - 1 - position.R;
                            next.C = sideWidth - 1;
                        }
                        else
                        { 
                            Debug.Fail($"{position.Dir} -> {connection.Direction} not implemented");
                        }
                    }

                    next.Map = connection.Map;
                    next.Dir = connection.Direction;
                }

                if (!Maps[next.Map][next.R][next.C]) {
                    //Console.WriteLine($"Reached a tree at ({next.R}, {next.C}) on map {next.Map}");
                    break;
                }

                position.R = next.R;
                position.C = next.C;
                position.Map = next.Map;
                position.Dir = next.Dir;
                //Console.WriteLine($"Walk {position.Dir} to ({position.R}, {position.C}) on map {position.Map}");
            }

            if (movement.EndsWith('R'))
            {
                //Console.WriteLine("Turn right");
                position.Dir = (Direction)(((int)position.Dir + 1) % 4);
            }
            else if (movement.EndsWith('L'))
            {
                //Console.WriteLine("Turn left");
                position.Dir = (Direction)(((int)position.Dir + 3) % 4);
            }
        }

        Console.WriteLine();
        Console.WriteLine($"Final position: ({position.R}, {position.C}) on map {position.Map} facing {position.Dir}");
        Console.WriteLine("Part 1: " + Score(position.R, position.C, position.Map, position.Dir));
    }

    private enum Direction { Up, Right, Down, Left }

    private static bool Cell(char c) => c switch
    {
        '#' => false,
        '.' => true,
        _ => throw new ArgumentException(c.ToString())
    };

    private static int Score(int row, int col, int map, Direction direction)
    {
        var side = Maps[1].Length;

        var r = row;
        var c = col;

        if (Test)
        {
            //  - - 1 -
            //  2 3 4 -
            //  - - 5 6

            if (map == 1)
            {
                c += side * 2;
            }
            else if (map > 1 && map < 5)
            {
                r += side;
                c += side * (map - 2);
            }
            else if (map > 4)
            {
                r += side * 2;
                c += side * (4 - map);
            }

        } else {

            //  - 1 2
            //  - 3 -
            //  4 5 -
            //  6 - -

            if (map == 1) c += side;
            if (map == 6) r += side * 3;
            if (map == 5) {
                r += side * 2;
                c += side;
            }

            Debug.Assert(map == 1 || map == 6 || map == 5, $"No conversion exist for map {map}");
        }

        r++;
        c++;

        var face = ((int)direction + 3) % 4;
        return (1000 * r) + (4 * c) + face;
    }
}


