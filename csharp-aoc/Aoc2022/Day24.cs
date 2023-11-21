namespace Day24;

public class Day24
{
    private static readonly bool Test = false;

    public static void Run()
    {
        var lines = File.ReadAllLines(Test ? "testinput_day24.txt" : "input_day24.txt");
        var grid = lines.Skip(1).Take(lines.Length - 2).Select(l => l.Substring(1, l.Length - 2)).ToArray();

        var blizzards = SimulateBlizzards(grid);

        var startPosition = (-1, lines[0].IndexOf('.') - 1);
        var endPosition = (grid.Length, lines[lines.Length-1].IndexOf('.') - 1);
        var rowBounds = (0, grid.Length - 1);
        var colBounds = (0, grid[0].Length - 1);

        var memo = new HashSet<(int T, int R, int C, int Z)>();

        var queue = new Queue<(int T, int R, int C, int Z)>();
        queue.Enqueue((1, -1, 0, 0));

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();
            if (memo.Contains(state)) continue;
            memo.Add(state);

            if (state.Z == 0 && (state.R, state.C) == endPosition)
            {
                state.Z += 1;
                Console.WriteLine($"Reached end target {state.Z} after {state.T-1} steps. Turning back!");
                queue.Clear();
            }
            else if (state.Z == 1 && (state.R, state.C) == startPosition)
            {
                state.Z += 1;
                Console.WriteLine($"Reached start target after {state.T - 1} steps. Turning back, again!");
                queue.Clear();
            }
            if (state.Z == 2 && (state.R, state.C) == endPosition)
            {
                Console.WriteLine($"Reached end target again after {state.T - 1} steps. Finally made it!");
                break;
            }

            //Console.WriteLine($"T{state.T} at ({state.R},{state.C})");

            var blizzardPositions = blizzards[state.T % blizzards.Count];

            var up = U(state.R, state.C);
            if (up == startPosition || up.R >= 0 && !blizzardPositions.Contains(up))
                queue.Enqueue((state.T + 1, up.R, up.C, state.Z));

            var right = R(state.R, state.C);
            if (right.C <= colBounds.Item2 && right.R >= 0 && !blizzardPositions.Contains(right))
                queue.Enqueue((state.T + 1, right.R, right.C, state.Z));

            var down = D(state.R, state.C);
            if (down == endPosition || (down.R <= rowBounds.Item2 && !blizzardPositions.Contains(down)))
                queue.Enqueue((state.T + 1, down.R, down.C, state.Z));

            var left = L(state.R, state.C);
            if (left.C >= 0 && left.R <= rowBounds.Item2 && left.R >= 0 && !blizzardPositions.Contains(left))
                queue.Enqueue((state.T + 1, left.R, left.C, state.Z));

            if (!blizzardPositions.Contains((state.R, state.C)))
                queue.Enqueue((state.T + 1, state.R, state.C, state.Z));
        }
    }

    private static Dictionary<int, HashSet<(int R, int C)>> SimulateBlizzards(string[] grid)
    {
        var blizzards = new Dictionary<int, HashSet<(char Dir, int R, int C)>>();
        var initialBlizzardState = new HashSet<(char Dir, int R, int C)>();

        for (var r = 0; r < grid.Length; r++)
            for (var c = 0; c < grid[0].Length; c++)
                if (grid[r][c] != '.') initialBlizzardState.Add((grid[r][c], r, c));

        blizzards.Add(0, initialBlizzardState);

        var time = 1;

        var simulation = new HashSet<(char Dir, int R, int C)>(initialBlizzardState);
        while (true)
        {
            var newSimulation = new HashSet<(char Dir, int R, int C)>();
            foreach (var b in simulation)
            {
                var state = b;
                if (b.Dir == '^')
                {
                    state.R -= 1;
                    if (state.R < 0) state.R = grid.Length + state.R;
                }
                if (b.Dir == '<')
                {
                    state.C -= 1;
                    if (state.C < 0) state.C = grid[0].Length + state.C;
                }
                if (b.Dir == '>') state.C = (state.C + 1) % grid[0].Length;
                if (b.Dir == 'v') state.R = (state.R + 1) % grid.Length;

                newSimulation.Add(state);
            }

            if (blizzards.Values.Any(b => b.SetEquals(newSimulation)))
            {
                Console.WriteLine($"Blizzard simulation wrapped after {time} steps");
                break;
            }

            blizzards.Add(time, newSimulation);
            simulation = newSimulation;
            time++;
        }

        return blizzards.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(v => (v.R, v.C)).ToHashSet());
    }

    private static (int R, int C) U(int R, int C) => (R - 1, C);
    private static (int R, int C) D(int R, int C) => (R + 1, C);
    private static (int R, int C) L(int R, int C) => (R, C - 1);
    private static (int R, int C) R(int R, int C) => (R, C + 1);
}