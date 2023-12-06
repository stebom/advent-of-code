namespace Aoc2023;

internal class Day4 {
    record struct Card(int Index, int Tickets);

    internal static void Run() {
        var lines = File.ReadAllLines("input_day_4.txt").Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

        var cards = new List<Card>();
        var score = 0;

        for (int i = 0; i < lines.Length; i++) {
            var line = lines[i];
            var groups = line[(line.IndexOf(':') + 1)..].Split("|");
            var a = groups[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            var b = groups[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            var winningTickets = a.Intersect(b).Count();

            if (winningTickets > 0) {
                score += (int)Math.Pow(2, winningTickets - 1);
            }

            cards.Add(new(i, winningTickets));
        }

        Console.WriteLine($"Part 1: {score}");

        int Recurse(Card card) {
            var sum = 1;
            for (var offset = 1; offset <= card.Tickets; offset++) {
                sum += Recurse(cards[card.Index + offset]);
            }
            return sum;
        }

        var total = cards.Sum(Recurse);
        Console.WriteLine($"Part 2: {total}");
    }
}