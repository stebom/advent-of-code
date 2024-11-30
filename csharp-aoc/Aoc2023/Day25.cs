namespace AdventOfCode;

internal class Day25
{
    static readonly string[] TestInput = [
        "jqt: rhn xhk nvd",
        "rsh: frs pzl lsr",
        "xhk: hfx",
        "cmg: qnr nvd lhk bvb",
        "rhn: xhk bvb hfx",
        "bvb: xhk hfx",
        "pzl: lsr hfx nvd",
        "qnr: nvd",
        "ntq: jqt hfx bvb xhk",
        "nvd: lhk",
        "lsr: lhk",
        "rzs: qnr cmg lsr rsh",
        "frs: qnr lhk lsr",
    ];

    record struct Edge(int S, int D);

    static int Solve(List<int> vertices, List<Edge> edges)
    {
        static int CountCuts(List<Edge> edges, List<List<int>> subsets)
        {
            int cuts = 0;
            for (int i = 0; i < edges.Count; ++i)
            {
                var subset1 = subsets.First(s => s.Contains(edges[i].S));
                var subset2 = subsets.First(s => s.Contains(edges[i].D));
                if (subset1 != subset2) ++cuts;
            }

            return cuts;
        }

        Random random = new();
        List<List<int>> subsets = [];

        do
        {
            subsets = [];

            foreach (var vertex in vertices)
            {
                subsets.Add([vertex]);
            }

            while (subsets.Count > 2)
            {
                var i = random.Next() % edges.Count;

                var subset1 = subsets.First(s => s.Contains(edges[i].S));
                var subset2 = subsets.First(s => s.Contains(edges[i].D));

                if (subset1 == subset2) continue;

                subsets.Remove(subset2);
                subset1.AddRange(subset2);
            }
        } while (CountCuts(edges, subsets) != 3);

        return subsets.Aggregate(1, (p, s) => p * s.Count);
    }

    public static void Solve()
    {
        //var input = TestInput;
        var input = File.ReadAllLines(@"2023_25_input.txt");

        var edges = new List<Edge>();
        var namedVertices = new List<string>();

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            
            var groups = line.Split(": ");
            var component = groups[0];

            if (!namedVertices.Contains(component)) namedVertices.Add(component);
            var source = namedVertices.IndexOf(component);

            foreach (var subcomponent in groups[1].Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (!namedVertices.Contains(subcomponent)) namedVertices.Add(subcomponent);

                var destination = namedVertices.IndexOf(subcomponent);

                if (!edges.Contains(new(source, destination)) && !edges.Contains(new(destination, source)))
                {
                    edges.Add(new(source, destination));
                }
            }
        }

        var vertices = Enumerable.Range(0, namedVertices.Count).ToList();
        Console.WriteLine($"Part 1: {Solve(vertices, edges)}");
    }
}