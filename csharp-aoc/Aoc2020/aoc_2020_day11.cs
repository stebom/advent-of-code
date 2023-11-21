namespace aoc_2020;

static class Day11
{
    public static void Run()
    {
        var originalSeats = File.ReadAllLines(@"input_2020_day_11.txt")
            .Where(l => l != string.Empty)
            .Select(l => l.Select(c => c).ToArray())
            .ToArray();

        {
            var seats = originalSeats.Select(r => r.ToArray()).ToArray();
            while (true)
            {
                var newSeats = Process(seats, false);
                if (!IsDifferent(seats, newSeats))
                {
                    break;
                }
                seats = newSeats;
            }

            var numOccupied = seats.Sum(row => row.Sum(c => c == '#' ? 1 : 0));
            Console.WriteLine(numOccupied);
        }

        {
            var seats = originalSeats.Select(r => r.ToArray()).ToArray();
            while (true)
            {
                var newSeats = Process(seats, true);
                if (!IsDifferent(seats, newSeats))
                {
                    break;
                }
                seats = newSeats;
            }

            var numOccupied = seats.Sum(row => row.Sum(c => c == '#' ? 1 : 0));
            Console.WriteLine(numOccupied);
        }
    }

    static bool IsDifferent(char[][] a, char[][] b)
    {
        for (var r = 0; r < a.Length; r++)
        {
            for (var c = 0; c < a[r].Length; c++)
            {
                if (a[r][c] != b[r][c])
                {
                    return true;
                }
            }
        }
        return false;
    }

    static int CountOccupied(char[][] seats, int r, int c, bool recurse)
    {
        var numOccupied = 0;

        // TOP LEFT
        if (FindOccupied(seats, r, c, -1, -1, recurse)) { numOccupied++; }
        // UP
        if (FindOccupied(seats, r, c, -1, 0, recurse)) { numOccupied++; }
        // TOP RIGHT
        if (FindOccupied(seats, r, c, -1, 1, recurse)) { numOccupied++; }
        // RIGHT
        if (FindOccupied(seats, r, c, 0, 1, recurse)) { numOccupied++; }
        // BOTTOM RIGHT
        if (FindOccupied(seats, r, c, 1, 1, recurse)) { numOccupied++; }
        // DOWN
        if (FindOccupied(seats, r, c, 1, 0, recurse)) { numOccupied++; }
        // BOTTOM LEFT
        if (FindOccupied(seats, r, c, 1, -1, recurse)) { numOccupied++; }
        // LEFT
        if (FindOccupied(seats, r, c, 0, -1, recurse)) { numOccupied++; }

        return numOccupied;
    }

    static bool FindOccupied(char[][] seats, int r, int c, int dr, int dc, bool recurse)
    {
        r += dr;
        c += dc;

        if (r < 0 || c < 0 || r == seats.Length || c == seats[r].Length)
        {
            return false;
        }

        if (seats[r][c] == '#') { return true; }
        if (seats[r][c] == 'L') { return false; }
        if (recurse) { return FindOccupied(seats, r, c, dr, dc, recurse); }
        return false;
    }

    static char[][] Process(char[][] seats, bool step2)
    {
        var newSeats = seats.Select(r => r.ToArray()).ToArray();

        for (var r = 0; r < newSeats.Length; r++)
        {
            for (var c = 0; c < newSeats[r].Length; c++)
            {
                var adjacentOccupied = CountOccupied(seats, r, c, step2);

                if (seats[r][c] == 'L')
                {
                    // If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
                    if (adjacentOccupied == 0)
                    {
                        newSeats[r][c] = '#';
                    }
                }
                else if (seats[r][c] == '#')
                {
                    // If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
                    if (adjacentOccupied >= (step2 ? 5 : 4))
                    {
                        newSeats[r][c] = 'L';
                    }
                }
                else if (seats[r][c] == '.')
                {
                    // Floor (.) never changes
                }
                else throw new InvalidOperationException(seats[r][c].ToString());

                // Otherwise, the seat's state does not change.
            }
        }

        return newSeats;
    }
}

