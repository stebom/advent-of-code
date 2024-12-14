using System.Drawing;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Aoc2024;

public static class Day14
{
    // Predefined constants
    private const int Rows = 103;
    private const int Columns = 101;
    private const int MiddleRow = (Rows / 2);
    private const int MiddleColumn = (Columns / 2);

    record struct Values(int R, int C);

    record class Robot(Values Velocity, Values InitialPosition);
    
    const string InputFormat = @"p=(\-?\d+),(\-?\d+) v=(\-?\d+),(\-?\d+)";

    public static void Solve()
    {
        // Parse input
        var robots = File.ReadAllLines(@"input/day14_input.txt").Select(line =>
        {
            var match = Regex.Match(line, InputFormat);
            return new Robot(new Values(int.Parse(match.Groups[4].Value), int.Parse(match.Groups[3].Value)),
                             new Values(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[1].Value)));
        }).ToList();

        Console.WriteLine($"Part 1: {Part1(robots)}");
        Console.WriteLine($"Part 2: {Part2(robots)}");

        //PrintGrid(Rows, Columns, robots, 26);
    }

    private static long Part1(List<Robot> robots)
    {
        const int time = 100;

        // Gather robot positions at 100 seconds
        var postitions = robots.Select(robot =>
        {
            var row = (robot.InitialPosition.R + robot.Velocity.R * time) % Rows;
            var column = (robot.InitialPosition.C + robot.Velocity.C * time) % Columns;
            row = row < 0 ? Rows + row : row;
            column = column < 0 ? Columns + column : column;
            return (row, column);
        }).Where(position => position.row != MiddleRow && position.column != MiddleColumn);

        // Get number of robots in each quadrant
        var quadrants = postitions.Select(position => (position.row > (Rows / 2) ? 1 : 0, position.column > (Columns / 2) ? 1 : 0))
                                  .GroupBy(quadrant => quadrant)
                                  .Select(quadrant => quadrant.Count());

        // Calculate safety factory by multiplying all quadrants
        return quadrants.Aggregate((a, b) => a * b);
    }

    private static long Part2(List<Robot> robots)
    {
        // Found this by rending bitmaps and looking for the tree!
        const int part2 = 6285;
        PrintGrid(Render(robots, part2));
        Draw(robots, part2);
        return part2;
    }

    private static void PrintGrid(IEnumerable<char> grid)
    {
        foreach (var character in grid) Console.Write(character);
    }

    private static IEnumerable<char> Render(List<Robot> robots, int time)
    {
        var positions = robots.Select(robot =>
        {
            var row = (robot.InitialPosition.R + robot.Velocity.R * time) % Rows;
            var column = (robot.InitialPosition.C + robot.Velocity.C * time) % Columns;
            row = row < 0 ? Rows + row : row;
            column = column < 0 ? Columns + column : column;
            return (row, column);
        }).ToHashSet();

        Console.WriteLine($"Middle column robots: {positions.Where(p => p.column == MiddleColumn).Count()}");

        for (var r = 0; r < Rows; r++)
        {
            for (var c = 0; c < Columns; c++)
            {
                yield return positions.Contains((r, c)) ? '#' : ' ';
            }
            yield return '\n';
        }
    }

    private static void Draw(List<Robot> robots, int time)
    {
        var positions = robots.Select(robot =>
        {
            var row = (robot.InitialPosition.R + robot.Velocity.R * time) % Rows;
            var column = (robot.InitialPosition.C + robot.Velocity.C * time) % Columns;
            row = row < 0 ? Rows + row : row;
            column = column < 0 ? Columns + column : column;
            return (row, column);
        }).ToHashSet();

        using var bitmap = new Bitmap(Columns, Rows);
        
        foreach (var position in positions)
        {
            bitmap.SetPixel(position.column, position.row, Color.Black);
        }
        bitmap.Save($"day-14-{time}.bmp");
    }
}