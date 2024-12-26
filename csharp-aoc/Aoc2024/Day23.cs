using System.Linq;

namespace Aoc2024;

public static class Day23
{
    static readonly Dictionary<string, HashSet<string>> Connections = [];

    public static void Solve()
    {
        foreach (var connection in File.ReadAllLines("input/day23_input.txt").Select(x => x.Split('-')))
        {
            if (!Connections.TryAdd(connection[0], [connection[1]]))
            {
                Connections[connection[0]].Add(connection[1]);
            }
            if (!Connections.TryAdd(connection[1], [connection[0]]))
            {
                Connections[connection[1]].Add(connection[0]);
            }
        }

        Part1();
        Part2();
    }

    static void Part1()
    {
        var clusters = new HashSet<HashSet<string>>(HashSet<string>.CreateSetComparer());

        foreach (var connection in Connections.Keys)
        {
            foreach (var n in Connections[connection])
            {
                foreach (var nn in Connections[n].Where(nn => nn != connection && Connections[nn].Contains(connection)))
                {
                    clusters.Add([connection, n, nn]);
                }
            }
        }

        var filtered = clusters.Where(cluster => cluster.Any(n => n.StartsWith('t')));
        Console.WriteLine($"Part 1: {filtered.Count()}");
    }

    static void Part2()
    {
        var clusters = new HashSet<HashSet<string>>(HashSet<string>.CreateSetComparer());

        foreach (var node in Connections.Keys)
        {
            var cluster = new HashSet<string> { node };
            foreach (var n in Connections[node])
            {
                foreach (var nn in Connections[n].Where(nn => nn != node && Connections[nn].Contains(node)))
                {
                    cluster.Add(nn);
                }
            }

            clusters.Add(cluster);
        }

        var largest = clusters.OrderByDescending(c => c.Count).First();
        Console.WriteLine($"Part 2: {string.Join(',', largest.Order())}");
    }


}