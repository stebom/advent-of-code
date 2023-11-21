namespace Day16;

public static class Day16
{
    public readonly static IList<string> Labels = new List<string>();
    public readonly static IDictionary<int, int> Rate = new Dictionary<int, int>();
    public readonly static IDictionary<(int,int), int> Distance = new Dictionary<(int, int), int>();
    public readonly static IDictionary<int, List<int>> Neighbours = new Dictionary<int, List<int>>();

    public static Lazy<IList<int>> StartValves => new Lazy<IList<int>>(() => Rate.Where(r => r.Value > 0).Select(r => r.Key).ToList());

    public static IDictionary<int, int> BestScore = new Dictionary<int, int>();

    public static void Run()
    {
        const string input = "input.txt";

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

        var part1 = Search(30, Labels.IndexOf("AA"), StartValves.Value);
        Console.WriteLine($"Part 1: {part1}");

        var start = (26, Labels.IndexOf("AA"));
        var part2 = Search2(start, start.Item1, start.Item2, 2, StartValves.Value);
        Console.WriteLine($"Part 2: {part2}");
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

    public static int Search2((int t, int node) start, int t, int node, int players, IList<int> remaining)
    {
        var best = 0;

        foreach (var next in remaining)
        {
            var time = t - GetDistance(node, next) - 1;

            if (time <= 0) {
                if (remaining.Count > 0)
                {
                    Search2(start, start.t, start.node, players - 1, remaining);
                }
                continue;
            }

            var r = remaining.ToList();
            r.Remove(next);

            best = Math.Max(best, Search2(start, time, next, players, r) + Rate[next] * time);
        }

        BestScore[players] = best;
        return best;
    }

    public static int GetDistance(int start, int end)
    {
        if (Distance.TryGetValue((start, end), out var d))
            return d;

        var q = new Queue<int>();
        var visited = new HashSet<int> { start };
        var distance = new Dictionary<int, int>();
        distance[start] = 0;

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
