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
        var startState = new State(start, East);

        var bestScore = FindBestScore(nodes, startState, end);
        Console.WriteLine($"Part 1: {bestScore}");
        Console.WriteLine($"Part 2: {FindBestPath(nodes, startState, end, bestScore)}");
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

            foreach (var d in Directions)
            {
                if (!node.CanWalk(d)) continue;
                queue.Enqueue(new State(cell.Walk(d), d), score + 1 + (d == direction ? 0 : 1000));
            }
        }

        return -1;
    }

    record StateWithPath(State State, List<Cell> Path);

    static long FindBestPath(Dictionary<Cell, Node> nodes, State start, Cell end, long maxScore)
    {
        var queue = new PriorityQueue<StateWithPath, long>();
        queue.Enqueue(new(start, [start.Position]), 0);

        var dist = new Dictionary<State, long> { { start, 0 } };

        var paths = new HashSet<Cell>();

        while (queue.TryDequeue(out var state, out var score))
        {
            var ((cell, direction), path) = state;

            if (cell == end && score <= maxScore )
            {
                foreach (var p in path) paths.Add(p);
                continue;
            }

            var node = nodes[cell];
            foreach (var d in Directions)
            {
                if (!node.CanWalk(d)) continue;

                var nextScore = score + 1 + (d == direction ? 0 : 1000);
                var nextState = new State(cell.Walk(d), d);

                if (dist.TryGetValue(nextState, out var alt))
                {
                    if (nextScore < alt)
                    {
                        queue.Enqueue(new(nextState, [.. path, nextState.Position]), nextScore);
                        dist[nextState] = nextScore;
                    }
                    if (nextScore == alt)
                    {
                        queue.Enqueue(new(nextState, [.. path, nextState.Position]), nextScore);
                    }
                }
                else
                {
                    queue.Enqueue(new(nextState, [.. path, nextState.Position]), nextScore);
                    dist[nextState] = nextScore;
                }
            }
        }

        return paths.Count;
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