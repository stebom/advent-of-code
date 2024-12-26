namespace Aoc2024;

public static class Day21
{
    record struct Pair(char From, char To);

    static readonly Dictionary<Pair, string[]> NumericalKeypad = new()
    {
        { new Pair('A','0'), ["<" ]},
        { new Pair('A','1'), ["^<<" ]},
        { new Pair('A','2'), ["<^" ]},
        { new Pair('A','3'), ["^" ]},
        { new Pair('A','4'), ["^^<<" ]},
        { new Pair('A','5'), ["<^^" ]},
        { new Pair('A','6'), ["^^" ]},
        { new Pair('A','7'), ["^^^<<" ]},
        { new Pair('A','8'), ["<^^^" ]},
        { new Pair('A','9'), ["^^^" ]},

        { new Pair('0','A'), [">"] },
        { new Pair('0','1'), ["^<"] },
        { new Pair('0','2'), ["^"]},
        { new Pair('0','3'), [">^"] },
        { new Pair('0','4'), ["^^<"] },
        { new Pair('0','5'), ["^^"]},
        { new Pair('0','6'), [">^^"] },
        { new Pair('0','7'), ["^^^<"] },
        { new Pair('0','8'), ["^^^"] },
        { new Pair('0','9'), [">^^^"] },
                             
        { new Pair('1','A'), [">>v" ]},
        { new Pair('1','0'), [">v"]},
        { new Pair('1','2'), [">"] },
        { new Pair('1','3'), [">>"] },
        { new Pair('1','4'), ["^" ]},
        { new Pair('1','5'), ["^>" ]},
        { new Pair('1','6'), ["^>>"] },
        { new Pair('1','7'), ["^^"] },
        { new Pair('1','8'), ["^^>"]},
        { new Pair('1','9'), ["^^>>"] },
        { new Pair('2','A'), ["v>"] },
        { new Pair('2','0'), ["v" ]},
        { new Pair('2','1'), ["<"] },
        { new Pair('2','3'), [">" ]},
        { new Pair('2','4'), ["<^" ]},
        { new Pair('2','5'), ["^" ]},
        { new Pair('2','6'), ["^>" ]},
        { new Pair('2','7'), ["<^^]"] },
        { new Pair('2','8'), ["^^"]},
        { new Pair('2','9'), ["^^>"]},
                             
        { new Pair('3','A'), ["v" ]},
        { new Pair('3','0'), ["<v" ]},
        { new Pair('3','1'), ["<<"] },
        { new Pair('3','2'), ["<"] },
        { new Pair('3','4'), ["<<^"] },
        { new Pair('3','5'), ["<^"] },
        { new Pair('3','6'), ["^"] },
        { new Pair('3','7'), ["<<^^"] },
        { new Pair('3','8'), ["<^^"] },
        { new Pair('3','9'), ["^^"] },
                             
        { new Pair('4','A'), ["vv>>"] },
        { new Pair('4','0'), ["vv>]"] },
        { new Pair('4','1'), ["v" ] },
        { new Pair('4','2'), ["v>]"] },
        { new Pair('4','3'), ["v>>"] },
        { new Pair('4','5'), [">"] },
        { new Pair('4','6'), [">>"] },
        { new Pair('4','7'), ["^"] },
        { new Pair('4','8'), ["^>"] },
        { new Pair('4','9'), ["^>>"] },
                             
        { new Pair('5','A'), ["vv>"]},
        { new Pair('5','0'), ["vv" ]},
        { new Pair('5','1'), ["<v" ]},
        { new Pair('5','2'), ["v" ]},
        { new Pair('5','3'), ["v>"] },
        { new Pair('5','4'), ["<"] },
        { new Pair('5','6'), [">"] },
        { new Pair('5','7'), ["<^"] },
        { new Pair('5','8'), ["^"] },
        { new Pair('5','9'), ["^>"] },
                             
        { new Pair('6','A'), ["vv" ]},
        { new Pair('6','0'), ["<vv" ]},
        { new Pair('6','1'), ["<<v" ]},
        { new Pair('6','2'), ["<v" ]},
        { new Pair('6','3'), ["v" ]},
        { new Pair('6','4'), ["<<"] },
        { new Pair('6','5'), ["<"] },
        { new Pair('6','7'), ["<<^"] },
        { new Pair('6','8'), ["<^"] },
        { new Pair('6','9'), ["^"] },
                             
        { new Pair('7','A'), [">>vvv" ]},
        { new Pair('7','0'), [">vvv" ]},
        { new Pair('7','1'), ["vv" ]},
        { new Pair('7','2'), ["vv>"] },
        { new Pair('7','3'), ["vv>>" ]},
        { new Pair('7','4'), ["v" ]},
        { new Pair('7','5'), ["v>]" ]},
        { new Pair('7','6'), ["v>>" ]},
        { new Pair('7','8'), [">"] },
        { new Pair('7','9'), [">>"] },
                             
        { new Pair('8','A'), ["vvv>"] },
        { new Pair('8','0'), ["vvv"]},
        { new Pair('8','1'), ["<vv"]},
        { new Pair('8','2'), ["vv"]},
        { new Pair('8','3'), ["vv>"] },
        { new Pair('8','4'), ["<v"]},
        { new Pair('8','5'), ["v"]},
        { new Pair('8','6'), ["v>"] },
        { new Pair('8','7'), ["<"] },
        { new Pair('8','9'), [">"] },
                             
        { new Pair('9','A'), ["vvv" ]},
        { new Pair('9','0'), ["<vvv" ]},
        { new Pair('9','1'), ["<<vv" ]},
        { new Pair('9','2'), ["<vv" ]},
        { new Pair('9','3'), ["vv" ]},
        { new Pair('9','4'), ["<<v" ]},
        { new Pair('9','5'), ["<v" ]},
        { new Pair('9','6'), ["v" ]},
        { new Pair('9','7'), ["<<"] },
        { new Pair('9','8'), ["<"] },
    };

    static readonly Dictionary<Pair, string> DirectionalKeypad = new()
    {
        { new Pair('A','A'), "" },
        { new Pair('A','^'), "<" },
        { new Pair('A','<'), "<v<" },
        { new Pair('A','v'), "<v" },
        { new Pair('A','>'), "v" },

        { new Pair('^','A'), ">" },
        { new Pair('^','<'), "v<" },
        { new Pair('^','^'), "" },
        { new Pair('^','v'), "v" },
        { new Pair('^','>'), "v>" },

        { new Pair('v','A'), "^>" },
        { new Pair('v','<'), "<" },
        { new Pair('v','^'), "^" },
        { new Pair('v','v'), "" },
        { new Pair('v','>'), ">" },

        { new Pair('<','A'), ">>^" },
        { new Pair('<', '<'), "" },
        { new Pair('<','v'), ">" },
        { new Pair('<','^'), "^>" },
        { new Pair('<','>'), ">>" },

        { new Pair('>','A'), "^" },
        { new Pair('>','v'), ">" },
        { new Pair('>','^'), "<^" },
        { new Pair('>','<'), "<<" },
        { new Pair('>','>'), "" },
    };
    
    public static void Solve()
    {
        //string[] inputs = ["029A", "980A", "179A", "456A", "379A"];
        string[] inputs = ["208A", "586A", "341A", "463A", "593A"];

        long sum = 0;
        foreach (var input in inputs)
        {
            var expanded = input.Expand().ToList();
            var complexity = long.Parse(input[0..(input.Length - 1)]);

            sum += expanded.Count * complexity;

            Console.WriteLine($"{input}: {string.Join("", expanded)} {expanded.Count} * {complexity}");
        }

        Console.WriteLine($"Part 1: {sum}");
    }

    static IEnumerable<char> Expand(this string input)
    {
        var pos = 'A';
        var pos2 = 'A';
        var pos3 = 'A';

        List<List<char>> sequences = [[],[],[],[]];

        foreach (var c in input)
        {
            var p = NumericalKeypad[new Pair(pos, c)];
            foreach (var pp in p)
            { 
                foreach (var c2 in pp.ZipButton())
                {
                    foreach (var c3 in DirectionalKeypad[new Pair(pos2, c2)].ZipButton())
                    {
                        foreach (var c4 in DirectionalKeypad[new Pair(pos3, c3)].ZipButton())
                        {
                            sequences[0].Add(c4);
                            yield return c4;
                        }

                        sequences[1].Add(c3);
                        pos3 = c3;
                    }

                    sequences[2].Add(c2);
                    pos2 = c2;
                }

                sequences[3].Add(c);
                pos = c;
            }
        }

        Console.WriteLine();
        foreach (var c in sequences)
        {
            Console.WriteLine(string.Join("", c));
        }
    }

    static IEnumerable<char> ZipButton(this IEnumerable<char> sequence)
    {
        foreach (var c in sequence) yield return c;
        yield return 'A';
    }
}