namespace Aoc2017;
static class Day11 {

    public static void Run() {
        var input = File.ReadAllLines(@"input_day_11.txt").First().Split(',');
        Walk(input);
    }

    static int Distance(this Position vec) => (Math.Abs(vec.R) + Math.Abs(vec.Q) + Math.Abs(vec.S)) / 2;

    record struct Position(int R, int Q, int S);

    static void Walk(string[] steps) {

        var max = 0;

        var pos = new Position(0, 0, 0);
        foreach (var line in steps) {
            var diff = line switch {
                "nw" => new Position(0, -1, 1),
                "n" => new(-1, 0, 1),
                "ne" => new(-1, 1, 0),
                "sw" => new(1, -1, 0),
                "s" => new(1, 0, -1),
                "se" => new(0, 1, -1),
                _ => throw new NotImplementedException(line),
            }; ;

            pos = new(pos.R + diff.R, pos.Q + diff.Q, pos.S + diff.S);
            max = Math.Max(max, pos.Distance());
        }

        Console.WriteLine(pos.Distance());
        Console.WriteLine(max);
    }
}