namespace aoc_2020;

static class Day13
{
    public static void Run()
    {
        var input = File.ReadAllLines(@"input_2020_day_13.txt")
            .Where(l => l != string.Empty)
            .ToArray();

        Console.WriteLine(Part1(input));
        Console.WriteLine(Part2(input.Skip(1).First().Split(',')));
    }

    static int Part1(string[] input)
    {
        var start = int.Parse(input.First());
        var busses = input.Skip(1).First().Split(',').Where(t => t != "x").Select(int.Parse).ToArray();

        int time = start;
        while (true)
        {
            foreach (int bus in busses) {
                if (time % bus == 0) { return (time - start) * bus; }
            }
            time++;
        }
    }

    static long Part2(string[] input)
    {
        var departures = input.Select(v => v == "x" ? 1 : long.Parse(v)).ToArray();
        long time = 0;
        long step = departures[0];

        int offset = 1;

        while (true)
        {
            if ((time + offset) % departures[offset] == 0)
            {
                if (offset == departures.Length - 1) { break; }
                step *= departures[offset];
                offset++;
            }

            time += step;
        }

        return time;
    }

}

