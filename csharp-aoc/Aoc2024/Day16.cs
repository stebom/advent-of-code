namespace Aoc2024;

public static class Day16
{
    record struct Cell(int R, int C);

    record class Node
    {
        public Node? North { get; set; }
        public Node? East { get; set; }
        public Node? South { get; set; }
        public Node? West { get; set; }
    }

    record struct State(Cell Position, Cell Direction);

    static readonly Cell North = new(-1, 0);
    static readonly Cell East = new(0, 1);
    static readonly Cell South = new(1, 0);
    static readonly Cell West = new(0, -1);
    static readonly Cell[] Directions = [North, East, South, West];

    static bool CanWalk(this Node node, Cell direction)
    {
        if (direction == North) return node.North != null;
        if (direction == East) return node.East != null;
        if (direction == South) return node.South != null;
        if (direction == West) return node.West != null;
        return false;
    }

    public static void Solve()
    {
        var grid = File.ReadAllLines(@"input/day16_input.txt").Select(l => l.ToCharArray()).ToArray();

        Dictionary<Cell, Node> nodes = [];
        var (start, end) = WalkNodes(grid, nodes);

        var best = FindBestScore(nodes, new State(start, East), end);
        Console.WriteLine($"Part 1: {best}");

        var distances = FindDistances(nodes, end, new State(start, East));
        var reversedDistances = FindDistances(nodes, end, new State(end, South), new State(start, West));

        var count = 0;
        foreach (var node in nodes)
        {
            foreach (var direction in Directions) {

                var state = new State(node.Key, direction);
                if (distances.ContainsKey(state) &&
                    reversedDistances.ContainsKey(state) &&
                    distances[state] + reversedDistances[state] == best)
                {
                    count++;
                }
            }

        }
        Console.WriteLine($"Part 2: {count}");
    }

    static long FindBestScore(Dictionary<Cell, Node> nodes, State start, Cell target)
    {
        var queue = new PriorityQueue<State, long>();
        queue.Enqueue(start, 0);

        var visited = new HashSet<State>();

        while (queue.TryDequeue(out var state, out var score))
        {
            if (state.Position == target) return score;
            
            if (visited.Contains(state)) continue;
            visited.Add(state);

            var (cell, direction) = state;
            var node = nodes[state.Position];

            if (node.CanWalk(direction))
            {
                queue.Enqueue(new State(cell.Walk(direction), direction), score + 1);
            }

            foreach (var d in Directions.Where(d => d != direction))
            {
                queue.Enqueue(new State(cell, d), score + 1000);
            }
        }

        return -1;
    }

    static Dictionary<State, long> FindDistances(Dictionary<Cell, Node> nodes, Cell target, params State[] starts)
    {
        var queue = new PriorityQueue<State, long>();
        foreach (var start in starts) queue.Enqueue(start, 0);

        var distance = new Dictionary<State, long>();
        var visited = new HashSet<State>();

        while (queue.TryDequeue(out var state, out var score))
        {
            if (visited.Contains(state)) continue;
            visited.Add(state);

            if (!distance.ContainsKey(state)) distance[state] = score;

            var (cell, direction) = state;
            var node = nodes[state.Position];

            if (node.CanWalk(direction))
            {
                queue.Enqueue(new State(cell.Walk(direction), direction), score + 1);
            }

            foreach (var d in Directions.Where(d => d != direction))
            {
                queue.Enqueue(new State(cell, d), score + 1000);
            }
        }

        return distance;
    }

    private static HashSet<Cell> GetVisitedPaths(Cell start, Cell end, Dictionary<Cell, List<Cell>> prev)
    {
        var queue = new Queue<Cell>();
        queue.Enqueue(end);

        var visited = new HashSet<Cell>();
        
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            visited.Add(current);
            if (current == start) continue;
            foreach (var parent in prev[current]) queue.Enqueue(parent);
        }

        return visited;
    }

    static Cell Walk(this Cell cell, Cell direction) => new(cell.R + direction.R, cell.C + direction.C);

    static (Cell Start, Cell End) WalkNodes(char[][] grid, Dictionary<Cell, Node> nodes)
    {
        Cell start = default;
        Cell end = default;

        for (var r = 1; r < grid.Length - 1; r++)
        {
            for (var c = 1; c < grid[r].Length - 1; c++)
            {
                if (grid[r][c] == '#') continue;

                var cell = new Cell(r, c);

                if (grid[r][c] == 'S') start = cell;
                if (grid[r][c] == 'E') end = cell;

                var node = new Node();

                if (nodes.TryGetValue(new(r + North.R, c + North.C), out var northNode))
                {
                    node.North = northNode;
                    northNode.South = node;
                }

                if (nodes.TryGetValue(new(r + East.R, c + East.C), out var east))
                {
                    node.East = east;
                    east.West = node;
                }

                if (nodes.TryGetValue(new(r + South.R, c + South.C), out var south))
                {
                    node.South = south;
                    south.North = node;
                }

                if (nodes.TryGetValue(new(r + West.R, c + West.C), out var west))
                {
                    node.West = west;
                    west.East = node;
                }

                nodes.Add(cell, node);
            }
        }

        return (start, end);
    }
}