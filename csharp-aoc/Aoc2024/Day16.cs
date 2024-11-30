namespace AdventOfCode;

internal static class Day16
{
    record struct Direction(int Row, int Column);
    
    record struct Cell(int Row, int Column);

    record Beam(Cell Cell, Direction Direction);

    static readonly Direction Up    = new (-1, +0);
    static readonly Direction Right = new (+0, +1);
    static readonly Direction Down  = new (+1, +0);
    static readonly Direction Left  = new (+0, -1);

    static Cell Apply(this Cell cell, Direction direction)
        => new(cell.Row + direction.Row, cell.Column + direction.Column);

    static readonly string[] TestInput = [
        @".|...\....",
        @"|.-.\.....",
        @".....|-...",
        @"........|.",
        @"..........",
        @".........\",
        @"..../.\\..",
        @".-.-/..|..",
        @".|....-|.\",
        @"..//.|....",
    ];

    public static void Solve()
    {
        //var grid = TestInput;
        var grid = File.ReadAllLines(@"2023_16_input.txt");

        Console.WriteLine($"Part 1: {Solve(grid, new(new(0, -1), Right))}");

        var best = 0;

        foreach (var row in Enumerable.Range(0, grid.Length))
        {
            best = Math.Max(best, Solve(grid, new Beam(new(row, -1), Right)));
            best = Math.Max(best, Solve(grid, new Beam(new(row, grid[row].Length), Left)));
        }

        foreach (var column in Enumerable.Range(0, grid[0].Length))
        {
            best = Math.Max(best, Solve(grid, new Beam(new(-1, column), Down)));
            best = Math.Max(best, Solve(grid, new Beam(new(grid.Length, column), Up)));
        }

        Console.WriteLine($"Part 2: {best}");
    }

    static int Solve(string[] grid, Beam start)
    { 
        // Create dictionary of energized cells
        var energized = new Dictionary<Cell, int>();

        // Create list of beams and initial beam
        var beams = new Queue<Beam>();
        beams.Enqueue(start);

        // Keep track of starting nodes
        var visited = new HashSet<Beam>();

        while (beams.Count > 0)
        {
            var beam = beams.Dequeue();
            var nextCell = beam.Cell.Apply(beam.Direction);

            var validPosition = nextCell.Row >= 0 && nextCell.Row < grid.Length &&
                                nextCell.Column >= 0 && nextCell.Column < grid[nextCell.Row].Length;

            if (validPosition)
            {
                var tile = grid[nextCell.Row][nextCell.Column];

                var spawns = new List<Beam>();

                if (tile == '.' ||
                    (tile == '-' && beam.Direction.Row == 0) ||     // moving horizontally
                    (tile == '|' && beam.Direction.Column == 0))    // moving vertically
                {
                    spawns.Add(new(nextCell, beam.Direction));
                }
                else if (tile == '|' && beam.Direction.Row == 0)
                {
                    // Split beam
                    spawns.Add(new(nextCell, Up));
                    spawns.Add(new(nextCell, Down));
                }
                else if (tile == '-' && beam.Direction.Column == 0)
                {
                    // Split beam
                    spawns.Add(new(nextCell, Right));
                    spawns.Add(new(nextCell, Left));
                }
                else if (tile == '/')
                {
                    // Flip and invert Row/Column
                    spawns.Add(new(nextCell, new(-beam.Direction.Column, -beam.Direction.Row)));
                }
                else if (tile == '\\')
                {
                    // Flip Row/Column
                    spawns.Add(new(nextCell, new(beam.Direction.Column, beam.Direction.Row)));
                }

                foreach (var spawn in spawns)
                {
                    if (!visited.Contains(spawn))
                    {
                        beams.Enqueue(spawn);
                        visited.Add(spawn);
                    }
                }

                // Energize cell before move
                energized[nextCell] = energized.TryGetValue(nextCell, out int value) ? value + 1 : 1;
            }
        }

        return energized.Count;
    }
}
