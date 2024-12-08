namespace Aoc2017;

public static class Day12
{
    public static void Solve()
    {
        var graph = new Dictionary<int, HashSet<int>>();

        foreach (var input in File.ReadAllLines(@"input_day_12.txt"))
        {
            var split = input.Split(" <-> ");
            var id = int.Parse(split[0]);

            graph[id] = [];

            foreach (var connection in split[1].Split(", ").Select(int.Parse))
            {
                graph[id].Add(connection);

                if (graph.TryGetValue(connection, out var o))
                {
                    o.Add(id);
                }
                else
                {
                    graph[connection] = [id];
                }
            }
        }

        var groups = 0;
        var visited = new HashSet<int>();

        foreach (var node in graph.Keys)
        {
            if (visited.Contains(node)) continue;

            CountNodes(graph, visited, node);
            if (node == 0) Console.WriteLine($"Part 1: {visited.Count}");

            groups++;
        }

        Console.WriteLine($"Part 2: {groups}");
    }

    private static void CountNodes(Dictionary<int, HashSet<int>> graph, HashSet<int> visited, int node)
    {
        if (visited.Contains(node)) return;
        visited.Add(node);

        foreach (var connection in graph[node])
        {
            CountNodes(graph, visited, connection);
        }
    }

}