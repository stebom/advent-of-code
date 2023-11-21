/*
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;

var test = false;

var movements = test ? ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>".Select(c => c == '>' ? 1 : -1).ToArray()
                     : File.ReadAllLines("input_day17.txt").First().Select(c => c == '>' ? 1 : -1).ToArray();

// You guessed 1536994219667.
// You did not guess 1536994219669

var stack = new HashSet<(long y, long x)>(); // all bricks
var top = 0L; // height of tower
var count = 0L; // number of rocks
var rock = Create(count++, top + 4); // initial rock

var states = new Dictionary<(ulong Key, long Shape, long MovementIndex), (long, long)>();

var added = 0L;
const long part2Count = 1_000_000_000_000L;

var movement = 0L;
while (count <= part2Count)
{
    var movementIndex = movement % movements.Length;
    var push = movements[movementIndex];

    var canMoveX = rock.All(r => r.x + push >= 0 && r.x + push < 7) &&
                   !rock.Any(r => stack.Contains((r.y, r.x + push)));

    if (canMoveX) rock = rock.Select(r => (r.y, r.x + push)).ToList();

    var canMoveY = rock.All(r => r.y - 1 > 0) &&
                   !rock.Any(r => stack.Contains((r.y - 1, r.x)));

    if (canMoveY) rock = rock.Select(r => (r.y - 1, r.x)).ToList();

    if (!canMoveY)
    {
        rock.ForEach(block => stack.Add(block));
        top = stack.Max(r => r.y);

        var shape = count % 5;

        if (count == 2022)
        {
            Console.WriteLine($"Part 1: {top}");
        }
        else if (count > 2022)
        {
            // Look for cycles
            var key = CreateKey(); // create bitmap for top 9 rows

            if (states.ContainsKey((key, shape, movementIndex))) // Cycle found before
            {
                var (oldCount, oldTop) = states[(key, shape, movementIndex)];

                //Console.WriteLine($"Cycle found: ({key},{shape},{movementIndex}): ({oldCount},{oldTop})");

                var dy = top - oldTop;
                var dt = count - oldCount;
                var amt = (part2Count - count) / dt;
                added += amt * dy;
                count += amt * dt;
            }
            else
            {
                states[(key, shape, movementIndex)] = (count, top); // // Store current signature
            }
        }

        rock = Create(shape, top + 4); // create new rock
        count++; // increase rock count
    }

    movement++;
}

Console.WriteLine($"Part 2: {top + added}");

ulong CreateKey()
{
    var key = 0UL;
    for (var i = 0; i < 63; i++)
        key |= (ulong)(stack.Contains((top - (i / 7), i % 7)) ? 1 : 0) << i;
    return key;
}

List<(long y, long x)> Create(long shape, long y) => shape switch
{
    // ####
    0 => new List<(long y, long x)> { (y, 2), (y, 3), (y, 4), (y, 5) },

    // .#.
    // ###
    // .#.
    1 => new List<(long y, long x)> { (y + 2, 3), (y + 1, 2), (y + 1, 3), (y + 1, 4), (y, 3) },

    // ..#
    // ..#
    // ###
    2 => new List<(long y, long x)> { (y + 2, 4), (y + 1, 4), (y, 2), (y, 3), (y, 4) },

    // #
    // #
    // #
    // #
    3 => new List<(long y, long x)> { (y + 3, 2), (y + 2, 2), (y + 1, 2), (y, 2) },

    // ##
    // ##
    4 => new List<(long y, long x)> { (y + 1, 2), (y + 1, 3), (y, 2), (y, 3) },

    _ => throw new ArgumentException($"Invalid shape {shape}")
};
*/