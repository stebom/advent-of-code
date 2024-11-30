namespace AdventOfCode;

internal static class Year2015_Day16
{
    record Record(int SueIndex, int Score);

    public static void Solve()
    {
        var needle = new Dictionary<string, int> {
            { "children", 3 },
            { "cats", 7 },
            { "samoyeds", 2 },
            { "pomeranians", 3 },
            { "akitas", 0 },
            { "vizslas", 0 },
            { "goldfish", 5 },
            { "trees", 3 },
            { "cars", 2 },
            { "perfumes", 1 }
        };

        Record? best = null;
        foreach (var line in File.ReadAllLines(@"2015_16_input.txt"))
        {
            var delim = line.IndexOf(": ");

            var sueIndex = int.Parse(line[4..delim]);
            var compounds = line[(delim+2)..].Split(", ").Select(t => t.Split(": "));

            var score = 0;
            foreach (var compound in compounds)
            {
                var key = compound[0];
                var value = int.Parse(compound[1]);

                // In particular,
                // the cats and trees readings indicates that there are greater than that many
                if (key == "cats" || key == "trees")
                {
                    if (needle[key] < value) score++;
                }
                // while the pomeranians and goldfish readings indicate that there are fewer than that many
                if (key == "pomeranians" || key == "goldfish")
                {
                    if (needle[key] > value) score++;
                }
                else if (needle[key] == value) score++;
            }
            if (best == null || score > best.Score) {
                best = new(sueIndex, score);
            }
        }

        Console.WriteLine($"Sue {best!.SueIndex} has score of {best!.Score}");
    }
}
