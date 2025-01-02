namespace Aoc2018;

static class Day12
{
    static readonly string InitialState = "##.#############........##.##.####..#.#..#.##...###.##......#.#..#####....##..#####..#.#.##.#.##";
    static readonly Dictionary<string, char> Notes = new()
    {
        {"###.#",'#'},
        {".####",'#'},
        {"#.###",'.'},
        {".##..",'.'},
        {"##...",'#'},
        {"##.##",'#'},
        {".#.##",'#'},
        {"#.#..",'#'},
        {"#...#",'.'},
        {"...##",'#'},
        {"####.",'#'},
        {"#..##",'.'},
        {"#....",'.'},
        {".###.",'.'},
        {"..#.#",'.'},
        {"..###",'.'},
        {"#.#.#",'#'},
        {".....",'.'},
        {"..##.",'.'},
        {"##.#.",'#'},
        {".#...",'#'},
        {"#####",'.'},
        {"###..",'#'},
        {"..#..",'.'},
        {"##..#",'#'},
        {"#..#.",'#'},
        {"#.##.",'.'},
        {"....#",'.'},
        {".#..#",'#'},
        {".#.#.",'#'},
        {".##.#",'.'},
        {"...#.",'.'},
    };

    public static void Run()
    {
        Part1();
        Part2();
    }

    static void Part1()
    {
        HashSet<int> plants = [];

        for (var i = 0; i < InitialState.Length; i++)
        {
            if (InitialState[i] == '#') plants.Add(i);
        }

        Console.WriteLine($"{0:D2}: {new string(Enumerable.Range(plants.Min() - 2, plants.Max() - plants.Min() + 4).Select(i => plants.Contains(i) ? '#' : '.').ToArray())}");

        char P(int i) => plants.Contains(i) ? '#' : '.';
        for (var gen = 0; gen < 20; gen++)
        {
            HashSet<int> next = [];
            for (var i = plants.Min() - 2; i < plants.Max() + 2; i++)
            {
                if (Notes.TryGetValue($"{P(i - 2)}{P(i - 1)}{P(i)}{P(i + 1)}{P(i + 2)}", out char result) && result == '#')
                {
                    next.Add(i);
                }
            }

            plants = next;

            Console.WriteLine($"{gen + 1:D2}: {new string(Enumerable.Range(plants.Min() - 2, plants.Max() - plants.Min() + 2).Select(i => plants.Contains(i) ? '#' : '.').ToArray())}");
        }

        Console.WriteLine($"Part 1: {plants.Sum()}");
    }

    static void Part2()
    {
        HashSet<int> plants = [];
        long diff = 0;

        for (var i = 0; i < InitialState.Length; i++)
        {
            if (InitialState[i] == '#') plants.Add(i);
        }

        char P(int i) => plants.Contains(i) ? '#' : '.';
        for (int gen = 0; gen < 200; gen++)
        {
            HashSet<int> next = [];
            for (int i = plants.Min() - 2; i < plants.Max() + 2; i++)
            {
                if (Notes.TryGetValue($"{P(i - 2)}{P(i - 1)}{P(i)}{P(i + 1)}{P(i + 2)}", out char result) && result == '#')
                {
                    next.Add(i);
                }
            }

            diff = next.Sum() - plants.Sum();
            plants = next;
        }

        long sum = plants.Sum() + ((50_000_000_000 - 200) * diff);
        Console.WriteLine($"Part 2: {sum}");
    }
}
