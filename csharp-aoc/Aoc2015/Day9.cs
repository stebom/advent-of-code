
using System.Collections.Generic;

namespace Aoc2015
{
    internal class Day9
    {
        public record struct Vertex(string Name);
        public record struct Edge(Vertex From, Vertex To);

        public record Route(List<Vertex> Visited, int Distance);

        public static void Run()
        {
            var weights = new Dictionary<Edge, int>();

            // Norrath to Straylight = 115
            var delimiters = new []{ " to ", " = " };
            foreach (var tokens in File.ReadAllLines("2015_day_9_input.txt")
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)))
            {
                weights.Add(new Edge(new Vertex(tokens[0]), new Vertex(tokens[1])), int.Parse(tokens[2]));
                weights.Add(new Edge(new Vertex(tokens[1]), new Vertex(tokens[0])), int.Parse(tokens[2]));
            }

            var vertices = new HashSet<Vertex>();
            var adjacencyList = new Dictionary<Vertex, HashSet<Vertex>>();

            foreach (var edges in weights.Keys)
            {
                vertices.Add(edges.From);
                vertices.Add(edges.To);

                if (!adjacencyList.ContainsKey(edges.From))
                {
                    adjacencyList.Add(edges.From, new HashSet<Vertex> { edges.To });
                }
                else
                {
                    adjacencyList[edges.From].Add(edges.To);
                }
            }

            var shortest = int.MaxValue;
            var longest = 0;

            // Try every vertice
            var q = new Queue<Route>(vertices.Select(v => new Route(new List<Vertex> { v }, 0)));

            while (q.Count > 0)
            {
                var route = q.Dequeue();

                //Console.WriteLine($"{string.Join(" -> ", route.Visited.Select(v => v.Name))} = {route.Distance}");

                // We have visited all
                if (route.Visited.Distinct().Count() == vertices.Count)
                {
                    if (route.Distance < shortest)
                    {
                        // Found best route
                        shortest = route.Distance;
                    }
                    if (route.Distance > longest)
                    {
                        // Found worst route
                        longest = route.Distance;
                    }
                    continue;
                }

                var current = route.Visited.Last();
                if (adjacencyList.TryGetValue(current, out var adjacent))
                {
                    foreach (var vertex in adjacent)
                    {
                        // Rule states we must only visit each vertex once
                        if (route.Visited.Contains(vertex)) continue;

                        var visited = new List<Vertex>(route.Visited) { vertex };
                        var distance = weights[new(current, vertex)];

                        q.Enqueue(new(visited, route.Distance + distance));
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Shortest: {shortest}");
            Console.WriteLine($"Longest: {longest}");
        }

    }
}
