namespace Aoc2023;

internal class Day3 {

    record struct Symbol(char Char, int r, int c);

    internal static void Run() {
        var lines = File.ReadAllLines("input_day_3.txt");

        static IEnumerable<Symbol> FindSymbols(string[] lines, int r, int c, char[] digits) {
            for (int ir = r - 1; ir <= r + 1; ir++) {
                for (int ic = c - 1; ic < c + digits.Length + 1; ic++) {
                    char sym = '?';
                    try {
                        var cur = lines[ir][ic];
                        if (!char.IsDigit(cur) && cur != '.') {
                            sym = cur;
                        }
                    }
                    catch { /* discard */ }

                    if (sym != '?') {
                        yield return new Symbol(sym, ir, ic);
                    }
                }
            }
        }

        var numbers = new List<int>();
        var gears = new Dictionary<Symbol, List<int>>();

        for (int r = 0; r < lines.Length; r++) {
            for (int c = 0; c < lines[r].Length; c++) {
                if (lines[r][c] == '.') {
                    continue;
                }
                if (char.IsDigit(lines[r][c])) {
                    var digits = lines[r][c..].TakeWhile(char.IsDigit).ToArray();
                    var number = int.Parse(digits);

                    var symbols = FindSymbols(lines, r, c, digits);
                    if (symbols.Any()) {
                        numbers.Add(number);

                        foreach (var symbol in symbols.Where(sym => sym.Char == '*')) {
                            if (gears.ContainsKey(symbol)) {
                                gears[symbol].Add(number);
                            } else {
                                gears[symbol] = new List<int> { number };
                            }
                        }
                    }

                    c += digits.Length;
                }
            }
        }

        Console.WriteLine($"Part 1: {numbers.Sum()}");
        Console.WriteLine($"Part 2: {gears.Where(g => g.Value.Count == 2).Sum(k => k.Value.Aggregate((a, b) => a * b))}");
    }
}