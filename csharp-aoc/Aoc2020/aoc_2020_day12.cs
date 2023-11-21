namespace aoc_2020;

static class Day12
{
    public static void Run()
    {
        var actions = File.ReadAllLines(@"input_2020_day_12.txt")
            .Where(l => l != string.Empty)
            .ToArray();

        Part1(actions);
        Part2(actions);
    }

    static void Part1(string[] actions)
    {
        var dir = 1; // The ship starts by facing east.
        var x = 0;
        var y = 0;

        foreach (var action in actions)
        {
            var value = int.Parse(action.Skip(1).ToArray());

            if (action[0] == 'N')
            {
                y += value;
            }
            else if (action[0] == 'S')
            {
                y -= value;
            }
            else if (action[0] == 'E')
            {
                x += value;
            }
            else if (action[0] == 'W')
            {
                x -= value;
            }
            else if (action[0] == 'L')
            {
                var turns = value / 90;
                dir = (dir + 3 * turns) % 4;
            }
            else if (action[0] == 'R')
            {
                var turns = value / 90;
                dir = (dir + turns) % 4;

            }
            else if (action[0] == 'F')
            {
                if (dir == 0) { y += value; }
                if (dir == 1) { x += value; }
                if (dir == 2) { y -= value; }
                if (dir == 3) { x -= value; }
            }
        }

        Console.WriteLine(Math.Abs(x) + Math.Abs(y));
    }


    static void Part2(string[] actions)
    {
        var x = 0;
        var y = 0;
        
        // The waypoint starts 10 units east and 1 unit north relative to the ship.
        var wx = 10;
        var wy = 1;

        foreach (var action in actions)
        {
            var value = int.Parse(action.Skip(1).ToArray());

            if (action[0] == 'N') {
                wy += value;
            }
            else if (action[0] == 'S') {
                wy -= value;
            }
            else if (action[0] == 'E') {
                wx += value;
            }
            else if (action[0] == 'W') {
                wx -= value;
            }
            else if (action[0] == 'L') {
                // Action L means to rotate the waypoint around the ship left
                // (counter - clockwise) the given number of degrees.

                for (var i = 0; i < (value / 90); i++)
                {
                    var tmpwx = wx;
                    wx = -wy;
                    wy = tmpwx;
                }

            }
            else if (action[0] == 'R') {
                // Action R means to rotate the waypoint around the ship right
                // (clockwise) the given number of degrees.

                for (var i = 0; i < (value/90); i++) {
                    var tmpwx = wx;
                    wx = wy;
                    wy = -tmpwx;
                }
            }
            else if (action[0] == 'F') {
                // Action F means to move forward to the waypoint a number of times
                // equal to the given value.
                y += wy * value;
                x += wx * value;
            }
        }

        Console.WriteLine(Math.Abs(x) + Math.Abs(y));
    }
}

