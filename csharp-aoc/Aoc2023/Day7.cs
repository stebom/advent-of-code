namespace Aoc2023;

internal static class Day7 {
    record struct Hand(string Cards, int Bid);

    static char[] CardValues = new[] { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };

    static HandType GetBestJokerHand(Hand hand) {
        var possible = new HashSet<HandType>();

        var current = hand.Cards.ToArray();

        for (int i = 0; i < CardValues.Length - 1; i++) {
            if (hand.Cards[0] == 'J') current[0] = CardValues[i];

            for (int j = 0; j < CardValues.Length - 1; j++) {
                if (hand.Cards[1] == 'J') current[1] = CardValues[j];

                for (int k = 0; k < CardValues.Length - 1; k++) {
                    if (hand.Cards[2] == 'J') current[2] = CardValues[k];

                    for (int l = 0; l < CardValues.Length - 1; l++) {
                        if (hand.Cards[3] == 'J') current[3] = CardValues[l];

                        for (int m = 0; m < CardValues.Length - 1; m++) {
                            if (hand.Cards[4] == 'J') current[4] = CardValues[m];

                            var permuted = new string(current);
                            possible.Add(permuted.ToHandType());

                            if (hand.Cards[4] != 'J') break;
                        }
                        if (hand.Cards[3] != 'J') break;
                    }
                    if (hand.Cards[2] != 'J') break;
                }
                if (hand.Cards[1] != 'J') break;
            }
            if (hand.Cards[0] != 'J') break;
        }

        return possible.OrderBy(e => (int)e).First();
    }

    public class HandComparer : IComparer<Hand> {
        readonly bool useJoker;

        public HandComparer(bool useJoker = false) {
            this.useJoker = useJoker;
        }

        int IComparer<Hand>.Compare(Hand x, Hand y) {

            var xHandType = x.ToHandType();
            var yHandType = y.ToHandType();

            if (useJoker) {
                // get best hands possible by replacing Joker with any other card
                xHandType = GetBestJokerHand(x);
                yHandType = GetBestJokerHand(y);
            }

            var ordinalDiff = xHandType - yHandType;
            if (ordinalDiff == 0) {
                return CompareHands(x.Cards, y.Cards);
            }
            return ordinalDiff;
        }
    }

    static int CompareHands(string a, string b) {
        for (int i = 0; i < 5; i++) {
            var diff = Array.IndexOf(CardValues, a[i]) - Array.IndexOf(CardValues, b[i]);
            if (diff != 0) { return diff; }
        }
        return 0;
    }

    static HandType ToHandType(this Hand hand) => hand.Cards.ToHandType();

    enum HandType {
        FiveOfAKind,    // Five of a kind, where all five cards have the same label: AAAAA
        FourOfAKind,    // Four of a kind, where four cards have the same label and one card has a different label: AA8AA
        FullHouse,      // Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
        ThreeOfAKind,   // Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
        TwoPair,        // Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
        OnePair,        // One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
        HighCard,       // High card, where all cards' labels are distinct: 23456
        None            // Can this ever happen?
    };

    static HandType ToHandType(this string hand) {
        var occurences = hand.GroupBy(c => c).ToDictionary(g => g.Key, v => v.Count());
        if (occurences.Count == 1) return HandType.FiveOfAKind;
        if (occurences.Any(kvp => kvp.Value == 4)) return HandType.FourOfAKind;
        if (occurences.Any(kvp => kvp.Value == 3) && occurences.Any(kvp => kvp.Value == 2)) return HandType.FullHouse;
        if (occurences.Any(kvp => kvp.Value == 3)) return HandType.ThreeOfAKind;
        if (occurences.Count(kvp => kvp.Value == 2) == 2) return HandType.TwoPair;
        if (occurences.Count(kvp => kvp.Value == 2) == 1) return HandType.OnePair;
        if (occurences.Count() == 5) return HandType.HighCard;
        return HandType.None;
    }

    internal static void Run() {
        Part1();
        Part2();
    }

    static void Part1() {
        var hands = File.ReadAllLines("input_day_7.txt")
                        .Where(l => !string.IsNullOrWhiteSpace(l))
                        .Select(l => l.Split())
                        .Select(t => new Hand(t[0], int.Parse(t[1])))
                        .ToList();

        var ordedCards = hands.Order(new HandComparer());
        var rankedCards = ordedCards.Reverse().ToList();

        var total = 0;
        for (int i = 0; i < rankedCards.Count; i++) {
            total += rankedCards[i].Bid * (i + 1);
        }

        Console.WriteLine($"Part 1: {total}");
    }

    static void Part2() {
        var hands = File.ReadAllLines("input_day_7.txt").Where(l => !string.IsNullOrWhiteSpace(l))
                        .Select(l => l.Split())
                        .Select(t => new Hand(t[0], int.Parse(t[1])))
                        .ToList();

        // J cards are now the weakest
        CardValues = CardValues.OrderBy(c => c == 'J').ToArray();

        var ordedCards = hands.Order(new HandComparer(true));
        var rankedCards = ordedCards.Reverse().ToList();

        long total = 0;
        for (int i = 0; i < rankedCards.Count; i++) {
            total += rankedCards[i].Bid * (i + 1);
        }

        Console.WriteLine($"Part 2: {total}");
    }
}