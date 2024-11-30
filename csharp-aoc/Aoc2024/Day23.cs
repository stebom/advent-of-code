using System.Diagnostics;

namespace AdventOfCode;

internal class Day23
{
    static readonly string[] TestInput = [
        "#.#####################",
        "#.......#########...###",
        "#######.#########.#.###",
        "###.....#.>.>.###.#.###",
        "###v#####.#v#.###.#.###",
        "###.>...#.#.#.....#...#",
        "###v###.#.#.#########.#",
        "###...#.#.#.......#...#",
        "#####.#.#.#######.#.###",
        "#.....#.#.#.......#...#",
        "#.#####.#.#.#########v#",
        "#.#...#...#...###...>.#",
        "#.#.#v#######v###.###v#",
        "#...#.>.#...>.>.#.###.#",
        "#####v#.#.###v#.#.###.#",
        "#.....#...#...#.#.#...#",
        "#.#########.###.#.#.###",
        "#...###...#...#...#.###",
        "###.###.#.###v#####v###",
        "#...#...#.#.>.>.#.>.###",
        "#.###.###.#.###.#.#v###",
        "#.....###...###...#...#",
        "#####################.#",
    ];

    record struct Position(int R, int C);

    static IEnumerable<Position> Adjacents(char[][] grid, Position position)
    {
        // Handle icy slopes
        //if (grid[position.R][position.C] == '^') { yield return new(position.R - 1, position.C); yield break; }
        //if (grid[position.R][position.C] == '<') { yield return new(position.R, position.C - 1); yield break; }
        //if (grid[position.R][position.C] == '>') { yield return new(position.R, position.C + 1); yield break; }
        //if (grid[position.R][position.C] == 'v') { yield return new(position.R + 1, position.C); yield break; }

        if (position.R > 0 && grid[position.R - 1][position.C] != '#' /*&& grid[position.R - 1][position.C] != 'v'*/)
        {
            yield return new(position.R - 1,position.C);
        }
        if (position.R < grid.Length - 1 && grid[position.R + 1][position.C] != '#' /*&& grid[position.R + 1][position.C] != '^'*/)
        {
            yield return new(position.R + 1, position.C);
        }
        if (position.C > 0 && grid[position.R][position.C - 1] != '#' /*&& grid[position.R][position.C - 1] != '>'*/)
        {
            yield return new(position.R, position.C - 1);
        }
        if (position.C < grid[0].Length - 1 && grid[position.R][position.C + 1] != '#' /*&& grid[position.R][position.C + 1] != '<'*/)
        {
            yield return new(position.R, position.C + 1);
        }
    }

    static void Print(char[][] grid, List<Position> positions)
    {
        for (int r = 0; r < grid.Length; r++)
        {
            for (int c = 0; c < grid[0].Length; c++)
            {
                Console.Write(positions.Contains(new(r, c)) ? 'O' : grid[r][c]);
                Debug.Assert(positions.Count(p => p == new Position(r, c)) <= 1);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    record Node(List<Position> Positions);

    record State(Node Current, List<Node> Visited);

    public static void Solve()
    {
        //var input = TestInput;
        var input = File.ReadAllLines(@"2023_23_input.txt");

        char[][] grid = input.Select(line => line.ToCharArray()).ToArray();

        Dictionary<Node, HashSet<Node>> adjacencies = BuildNodes(grid);
        var nodes = adjacencies.Keys.ToList();

        Node start = adjacencies.Keys.Single(n => n.Positions.Contains(new(0, 1)));
        Node end = adjacencies.Keys.Single(n => n.Positions.Contains(new(grid.Length - 1, grid[0].Length - 2))); 

        List<State> solutions = [];

        Stack<State> stack = [];
        stack.Push(new State(start, [start]));

        while (stack.TryPop(out var state))
        {
            if (state.Current == end)
            {
                solutions.Add(state);
                continue;
            }

            foreach (var adjacent in adjacencies[state.Current].Except(state.Visited))
            {
                stack.Push(new State(adjacent, [.. state.Visited, adjacent]));
            }
        }

        foreach (var solution in solutions)
        {
            var path = solution.Visited.Select(node => (char)('A' + nodes.IndexOf(node)));
            var steps = solution.Visited.Sum(n => n.Positions.Count);
            Console.WriteLine($"Found hike consisting of {steps}: {string.Join(" -> ", path)}");
        }

        var best = solutions.Max(solution => solution.Visited.Sum(n => n.Positions.Count)) - 1;
        Console.WriteLine($"Best hike: {best}");
    }

    static Dictionary<Node, HashSet<Node>> BuildNodes(char[][] grid)
    {
        var start = new Node([new(0, 1)]);

        Dictionary<Node, HashSet<Node>> adjacencies = [];
        adjacencies.Add(start, []);

        HashSet<Position> visited = [start.Positions.Last()];

        Stack<Node> stack = [];
        stack.Push(start);

        while (stack.TryPop(out var current))
        {
            visited.Add(current.Positions.Last());

            var adjacents = Adjacents(grid, current.Positions.Last()).Except(visited).ToList();

            if (adjacents.Count == 1)
            {
                //Console.WriteLine($"Walking forward at {current.Positions.Last()} after {current.Positions.Count} steps.");

                current.Positions.Add(adjacents.Single());
                stack.Push(current);
            }
            else if (adjacents.Count > 1)
            {
                //Console.WriteLine($"Found a junction at {current.Positions.Last()} after {current.Positions.Count} steps.");

                var junction = current.Positions.Last();
                current.Positions.Remove(junction);

                var junctionNode = new Node([junction]);

                // Connect junction and current node
                adjacencies[current].Add(junctionNode);
                adjacencies.Add(junctionNode, [current]);

                foreach (var adjacent in adjacents)
                {
                    visited.Add(adjacent);
                    var next = new Node([adjacent]);

                    adjacencies[junctionNode].Add(next);
                    adjacencies.Add(next, [junctionNode]);

                    stack.Push(next);
                }
            }
            else
            {
                //Console.WriteLine($"Found a dead end at {current.Positions.Last()} after {current.Positions.Count} steps.");

                var connections = Adjacents(grid, current.Positions.Last()).Except(current.Positions);
                foreach (var connection in connections)
                {
                    foreach (var bridge in adjacencies.Keys.Where(n => n.Positions.Contains(connection)))
                    {
                        adjacencies[current].Add(bridge);
                        adjacencies[bridge].Add(current);
                    }
                }
            }
        }

        return adjacencies;
    }
}
