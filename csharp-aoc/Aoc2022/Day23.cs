using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Day23;

public class Day23
{
    private static readonly HashSet<(int R, int C)> Cells = new();

    private static readonly bool Test = false;
    
    public static void Run()
    {
        var lines = File.ReadAllLines(Test ? "testinput_day23.txt" : "input_day23.txt");
        for (var r = 0; r < lines.Length; r++)
            for (var c = 0; c < lines[0].Length; c++)
                if (lines[r][c] == '#') Cells.Add((r, c));

        var cells = new HashSet<(int R, int C)>(Cells);

        var round = 0;
        while (true)
        {
            var proposedMove = new Dictionary<(int R, int C), (int R, int C)>();
            var stay = new HashSet<(int R, int C)>();

            foreach (var cell in cells)
            {
                // If no other Elves are in one of those eight positions,
                // the Elf does not do anything during this round.
                if (Adjacent(cell).Count(cells.Contains) == 0) {
                    stay.Add(cell);
                }
                else
                {
                    var proposed = false;

                    // Otherwise, the Elf looks in each of four directions in the
                    // following order and proposes moving one step in the first valid direction:
                    for (var i = 0; i < 4; i++)
                    {
                        var direction = (i + round) % 4;

                        Func<int, (int R, int C), bool> propose = (dir, cell) => {

                            // If there is no Elf in the N, NE, or NW adjacent positions,
                            // the Elf proposes moving north one step.
                            if (dir == 0 && !Any(cells, N(cell.R, cell.C), NE(cell.R, cell.C), NW(cell.R, cell.C))) {
                                proposedMove.Add(cell, N(cell.R, cell.C));
                                return true;
                            }

                            // If there is no Elf in the S, SE, or SW adjacent positions,
                            // the Elf proposes moving south one step.
                            if (dir == 1 && !Any(cells, S(cell.R, cell.C), SE(cell.R, cell.C), SW(cell.R, cell.C))) {
                                proposedMove.Add(cell, S(cell.R, cell.C));
                                return true;
                            }

                            // If there is no Elf in the W, NW, or SW adjacent positions,
                            // the Elf proposes moving west one step.
                            if (dir == 2 && !Any(cells, W(cell.R, cell.C), NW(cell.R, cell.C), SW(cell.R, cell.C))) {
                                proposedMove.Add(cell, W(cell.R, cell.C));
                                return true;
                            }

                            // If there is no Elf in the E, NE, or SE adjacent positions,
                            // the Elf proposes moving east one step.
                            if (dir == 3 && !Any(cells, E(cell.R, cell.C), NE(cell.R, cell.C), SE(cell.R, cell.C))) {
                                proposedMove.Add(cell, E(cell.R, cell.C));
                                return true;
                            }

                            return false;
                        };

                        if (propose(direction, cell)) {
                            proposed = true;
                            break;
                        }
                    }

                    if (!proposed)
                        stay.Add(cell);
                }
            }

            if (proposedMove.Count == 0) {
                break;
            }

            // Make any conflicting proposals stay
            foreach (var p in proposedMove) {
                var conflict = proposedMove.Any(m => m.Key != p.Key && m.Value == p.Value);
                if (conflict) stay.Add(p.Key);
            }

            var numCells = cells.Count;
            cells.Clear();
            foreach (var cell in stay) {
                cells.Add(cell);
                proposedMove.Remove(cell);
            }
            foreach (var cell in proposedMove.Values) cells.Add(cell);

            Debug.Assert(numCells == cells.Count);

            if (round == 10) { 
                var tiles = 0;
                for (var r = cells.Min(c => c.R); r <= cells.Max(c => c.R); r++)
                    for (var c = cells.Min(c => c.C); c <= cells.Max(c => c.C); c++)
                        tiles++;

                Console.WriteLine($"Part 1: {tiles - cells.Count}");
            }

            round++;
        }

        Console.WriteLine($"Part 2: {round + 1}");
    }

    private static bool Any(HashSet<(int R, int C)> c, params (int R, int C)[] cells)
        => cells.Any(c.Contains);

    private static IEnumerable<(int R, int C)> Adjacent((int R, int C) cell)
        => Adjacent(cell.R, cell.C);

    private static IEnumerable<(int R, int C)> Adjacent(int R, int C)
    {
        yield return (R - 1, C - 1); yield return (R - 1, C); yield return (R - 1, C + 1);
        yield return (R, C - 1); yield return (R, C + 1);
        yield return (R + 1, C - 1); yield return (R + 1, C); yield return (R + 1, C + 1);
    }

    private static (int R, int C) NW(int R, int C) => (R - 1, C - 1);
    private static (int R, int C) N(int R, int C) => (R - 1, C);
    private static (int R, int C) NE(int R, int C) => (R - 1, C + 1);
    private static (int R, int C) S(int R, int C) => (R + 1, C);
    private static (int R, int C) SE(int R, int C) => (R + 1, C + 1);
    private static (int R, int C) SW(int R, int C) => (R + 1, C - 1);
    private static (int R, int C) W(int R, int C) => (R, C - 1);
    private static (int R, int C) E(int R, int C) => (R, C + 1);
}