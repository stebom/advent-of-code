
using System.Linq;

namespace Aoc2016
{
    internal class Day7
    {
        internal record struct Token(string Value, bool Inside);

        internal static List<Token> ParseLine(string line)
        {
            var tokens = new List<Token>();

            var inside = false;
            var start = 0;

            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] == '[' || line[i] == ']')
                {
                    var substr = line.Substring(start, i - start);
                    tokens.Add(new Token(substr, inside));

                    inside = line[i] == '[';
                    start = i + 1;
                }
            }

            {
                var substr = line.Substring(start, line.Length - start);
                tokens.Add(new Token(substr, inside));
            }

            return tokens;
        }

        internal static bool ContainsAbba(in Token token)
        {
            for (var i = 2; i < token.Value.Length - 1; i++)
            {
                if ((token.Value[i - 2] == token.Value[i + 1]) &&
                    (token.Value[i - 1] == token.Value[i]) &&
                    (token.Value[i] != token.Value[i + 1]))
                {
                    return true;
                }
            }

            return false;
        }

        internal static List<string> GetAbas(in Token token)
        {
            var abas = new List<string>();
            // An ABA is any three-character sequence which consists of the same character twice with a different
            // character between them, such as xyx or aba.
            // A corresponding BAB is the same characters but in reversed positions: yxy and bab, respectively.
            for (var i = 1; i < token.Value.Length - 1; i++)
            {
                if ((token.Value[i - 1] == token.Value[i + 1]) &&
                    (token.Value[i] != token.Value[i - 1]))
                {
                    abas.Add(token.Value.Substring(i-1,3));
                }
            }

            return abas;
        }

        internal static string ToBab(in string aba) => new(new[] { aba[1],aba[0], aba[1] });

        public static void Run()
        {
            var lines = File.ReadAllLines("day7.txt");
            var countTls = 0;
            var countSsl = 0;

            foreach (var line in lines)
            {
                var tokens = ParseLine(line);
                var hypernets = tokens.Where(t => t.Inside).ToArray();
                var supernets = tokens.Where(t => !t.Inside).ToArray();

                if (hypernets.All(t => !ContainsAbba(t)) &&
                    supernets.Any(t => ContainsAbba(t)))
                { countTls++; }

                if (supernets.Any(supernet => GetAbas(supernet)
                                          .Select(aba => ToBab(aba))
                                          .Any(bab => hypernets.Any(hypernet => hypernet.Value.Contains(bab)))))
                { countSsl++; }

            }

            Console.WriteLine($"TLS: {countTls}");
            Console.WriteLine($"SSL: {countSsl}");
        }
    }
}
