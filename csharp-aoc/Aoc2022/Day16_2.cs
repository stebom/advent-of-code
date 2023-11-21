
namespace Day16;

public static class Day16_2
{
    public readonly static IList<string> Labels = new List<string>();
    public readonly static IDictionary<int, int> Rate = new Dictionary<int, int>();
    public readonly static IDictionary<(int,int), int> Distance = new Dictionary<(int, int), int>();
    public readonly static IDictionary<int, List<int>> Neighbours = new Dictionary<int, List<int>>();

    public static Lazy<IList<int>> StartValves => new Lazy<IList<int>>(() => Rate.Where(r => r.Value > 0).Select(r => r.Key).ToList());
    public static IList<int> Valves => StartValves.Value;

    public const bool Test = false;

    // You guessed 2412

    public static void Run()
    {
        const string input = Test ? "testinput_day16.txt" : "input_day16.txt";

        foreach (var l in File.ReadAllText(input).Split("\n"))
        {
            var words = l.Split(new[] { ' ', ';', ',', '=' }, StringSplitOptions.RemoveEmptyEntries);

            var node = words[1];
            if (!Labels.Contains(node))
                Labels.Add(node);

            var index = Labels.IndexOf(words[1]);

            Neighbours.Add(index, words.Skip(10).Select(neighbour =>
            {
                if (!Labels.Contains(neighbour)) { Labels.Add(neighbour); }
                return Labels.IndexOf(neighbour);
            }).ToList());

            Rate.Add(index, int.Parse(words[5]));
        };

        var part1 = Search(30, Labels.IndexOf("AA"), Valves);
        Console.WriteLine($"Part 1: {part1}");

        var s2 = Search2(26, Labels.IndexOf("AA"));
        Console.WriteLine($"Part 2: {s2}");
    }

    public static int Search(int t, int node, IList<int> remaining)
    {
        var best = 0;

        foreach (var next in remaining)
        {
            var time = t - GetDistance(node, next) - 1;

            if (time <= 0)
                continue;

            var r = remaining.ToList();
            r.Remove(next);

            best = Math.Max(best, Search(time, next, r) + Rate[next] * time);
        }
        return best;
    }

    public static int Search2(int time, int start)
    {
        var initialState = new State { Time = time, Rate = 0, Node = start, Visited = new List<int>() };

        var queue = new Queue<State>();
        queue.Enqueue(initialState);

        var states = new List<State>();

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();

            foreach (var adjacent in Valves.Except(state.Visited))
            {
                var t = state.Time - GetDistance(state.Node, adjacent) - 1;
                if (t <= 0) {
                    continue;
                }

                var next = state;
                next.Time = t;
                next.Node = adjacent;
                next.Rate += Rate[adjacent] * t;
                next.Visited = new List<int>(next.Visited) { adjacent };

                queue.Enqueue(next);
                states.Add(next);
            }
        }

        var high = 0;
        foreach (var a in states)
        {
            high = Math.Max(high, a.Rate + Search(26, Labels.IndexOf("AA"), Valves.Except(a.Visited).ToList()));
        }
        return high;
    }

    public struct State
    {
        public int Time;
        public int Rate;
        public int Node;
        public List<int> Visited;
    }

    public static int GetDistance(int start, int end)
    {
        if (Distance.TryGetValue((start, end), out var d))
            return d;

        var q = new Queue<int>();
        var visited = new HashSet<int> { start };
        var distance = new Dictionary<int, int> { [start] = 0 };

        q.Enqueue(start);

        while (q.Any())
        {
            var s = q.Dequeue();

            foreach (var adjacent in Neighbours[s])
            {
                if (visited.Contains(adjacent)) continue;
                visited.Add(adjacent);

                distance[adjacent] = distance[s] + 1;
                q.Enqueue(adjacent);

                Distance[(s, adjacent)] = 1;
            }
        }

        Distance[(start, end)] = distance[end];

        foreach (var kvp in distance)
            Distance[(start, kvp.Key)] = kvp.Value;
        
        return distance[end];
    }
}