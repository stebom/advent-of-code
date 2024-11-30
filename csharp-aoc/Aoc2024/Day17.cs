

namespace AdventOfCode;

internal static class Day17
{
    enum Direction { None, Up, Right, Down, Left };
    record struct Crucible(int Row, int Column, Direction Direction, int Steps);
    record State(Crucible Crucible, int Heat);

    static readonly string[] TestInput = [
        "2413432311323",
        "3215453535623",
        "3255245654254",
        "3446585845452",
        "4546657867536",
        "1438598798454",
        "4457876987766",
        "3637877979653",
        "4654967986887",
        "4564679986453",
        "1224686865563",
        "2546548887735",
        "4322674655533",
    ];

    static readonly string[] TestInput2 = [
        "111111111111",
        "999999999991",
        "999999999991",
        "999999999991",
        "999999999991"
    ];

    static Crucible Walk(Crucible current, int steps = 1)
    {
        var delta = current.Direction switch
        {
            Direction.Up => (-steps, 0),
            Direction.Right => (0, steps),
            Direction.Down => (steps, 0),
            Direction.Left => (0, -steps),
            _ => throw new ArgumentException(nameof(current.Direction))
        };

        var row = current.Row + delta.Item1;
        var column = current.Column + delta.Item2;
        return new(row, column, current.Direction, current.Steps + steps);
    }

    static (int Row, int Column) Walk(Crucible current, Direction direction, int steps)
    {
        return direction switch
        {
            Direction.Up => (current.Row - steps, current.Column),
            Direction.Right => (current.Row, current.Column + steps),
            Direction.Down => (current.Row + steps, current.Column),
            Direction.Left => (current.Row, current.Column - steps),
            _ => throw new ArgumentException(nameof(current.Direction))
        };
    }

    private static bool IsValidPosition(string[] grid, int row, int column)
    {
        return row >= 0 && row < grid.Length && column >= 0 && column < grid[0].Length;
    }

    static List<Crucible> Adjacent(Crucible current)
    {
        var adjacent = new List<Crucible>();

        if (current.Steps < 2)
        {
            adjacent.Add(Walk(current));
        }

        if (current.Direction == Direction.Up || current.Direction == Direction.Down)
        {
            adjacent.Add(new(current.Row, current.Column - 1, Direction.Left, 0));
            adjacent.Add(new(current.Row, current.Column + 1, Direction.Right, 0));
        }
        else if (current.Direction == Direction.Right || current.Direction == Direction.Left)
        {
            adjacent.Add(new(current.Row - 1, current.Column, Direction.Up, 0));
            adjacent.Add(new(current.Row + 1, current.Column, Direction.Down, 0));
        }

        return adjacent;
    }

    private static Direction[] Turn(Direction direction)
    {
        return direction switch
        {
            Direction.Up => [Direction.Right, Direction.Left],
            Direction.Right => [Direction.Up, Direction.Down],
            Direction.Down => [Direction.Right, Direction.Left],
            Direction.Left => [Direction.Up, Direction.Down],
            Direction.None => [Direction.Up, Direction.Right, Direction.Down, Direction.Left],
            _ => throw new NotImplementedException()
        };
    }

    public static void Solve()
    {
        //var grid = TestInput;
        var grid = File.ReadAllLines(@"2023_17_input.txt");

        Console.WriteLine($"Part 1: {SolvePart1(grid).Min()}");
        Console.WriteLine($"Part 2: {SolvePart2(grid).Min()}");
    }

    private static HashSet<int> SolvePart1(string[] grid)
    {
        var visited = new Dictionary<Crucible, int>();
        var solutions = new HashSet<int>();
        var queue = new Queue<State>();

        queue.Enqueue(new(new(0, 0, Direction.Right, 0), 0));
        queue.Enqueue(new(new(0, 0, Direction.Down, 0), 0));

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();

            foreach (var adjacent in Adjacent(state.Crucible))
            {
                if (!IsValidPosition(grid, adjacent.Row, adjacent.Column)) continue;

                var heat = grid[adjacent.Row][adjacent.Column] - '0';
                var next = new State(adjacent, state.Heat + heat);

                if (adjacent.Row == grid.Length - 1 && adjacent.Column == grid[0].Length - 1)
                {
                    solutions.Add(next.Heat);
                    continue;
                }

                if (!visited.TryGetValue(adjacent, out int value))
                {
                    queue.Enqueue(next);
                    visited.Add(adjacent, next.Heat);
                }
                else if (value > next.Heat)
                {
                    queue.Enqueue(next);
                    visited[adjacent] = next.Heat;
                }
            }
        }

        return solutions;
    }

    private static HashSet<int> SolvePart2(string[] grid)
    {
        var visited = new Dictionary<Crucible, int>();
        var solutions = new HashSet<int>();
        var queue = new Queue<State>();

        var goal = (grid.Length - 1, grid[0].Length - 1);

        queue.Enqueue(new(new(0, 0, Direction.None, 0), 0));

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();

            if (state.Crucible.Row == goal.Item1 && state.Crucible.Column == goal.Item2)
            {
                solutions.Add(state.Heat);
                continue;
            }

            foreach (var direction in Turn(state.Crucible.Direction))
            {
                var cumulativeHeat = 0;

                for (var steps = 1; steps <= 10; steps++)
                {
                    var nextPos = Walk(state.Crucible, direction, steps);
                    if (!IsValidPosition(grid, nextPos.Row, nextPos.Column))
                    {
                        break;
                    }

                    cumulativeHeat += grid[nextPos.Row][nextPos.Column] - '0';

                    if (steps > 3)
                    {
                        var nextCrucible = new Crucible(nextPos.Row, nextPos.Column, direction, steps);
                        var nextState = new State(nextCrucible, state.Heat + cumulativeHeat);

                        if (!visited.TryGetValue(nextCrucible, out int value))
                        {
                            queue.Enqueue(nextState);
                            visited.Add(nextCrucible, nextState.Heat);
                        }
                        else if (value > nextState.Heat)
                        {
                            queue.Enqueue(nextState);
                            visited[nextCrucible] = nextState.Heat;
                        }
                    }
                }
            }
        }

        return solutions;
    }
}