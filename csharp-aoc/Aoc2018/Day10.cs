using System.Text.RegularExpressions;

namespace Aoc2018;
static class Day10 {
    record Pair {
        public required int X { get; set; }
        public required int Y { get; set; }
    }

    readonly static Regex IntPattern = new(@"\-{0,1}\d+");

    internal static void Run() {

        var positions = new List<Pair>();
        var velocities = new List<Pair>();

        var lines = File.ReadAllLines(@"input_day_10.txt");
        foreach (var line in lines) {
            var match = IntPattern.Matches(line).Where(m => m.Success).Select(m => int.Parse(m.Value)).ToArray();
            positions.Add(new Pair { X = match[0], Y = match[1] });
            velocities.Add(new Pair { X = match[2], Y = match[3] });
        }

        var ticks = 0;
        while (true) {
            ticks++;
            for (var i = 0; i < positions.Count; i++) {
                positions[i].X += velocities[i].X;
                positions[i].Y += velocities[i].Y;
            }

            var dx = positions.Max(p => p.X) - positions.Min(p => p.X);
            var dy = positions.Max(p => p.Y) - positions.Min(p => p.Y);

            if (dx == 61 && dy == 9) { break; }
        }

        for (var y = positions.Min(p => p.Y); y <= positions.Max(p => p.Y); y++) {
            var row = positions.Where(p => p.Y == y);
            for (var x = positions.Min(p => p.X); x <= positions.Max(p => p.X); x++) {
                ;
                Console.Write(row.Any(p => p.X == x) ? '#' : '.');
            }
            Console.WriteLine();
        }

        Console.WriteLine($"ticks: {ticks}");
    }
}
