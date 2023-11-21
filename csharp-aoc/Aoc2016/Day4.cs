namespace Aoc2016
{
    internal class Day4
    {
        static void Run()
        {
            var content = File.ReadAllLines("Day4.txt");

            static char Rotate(char c, int shifts) => (char)(((c - 'a' + shifts) % 26) + 'a');

            // Each room consists of an encrypted name (lowercase letters separated by dashes)
            // followed by a dash, a sector ID, and a checksum in square brackets.
            // A room is real (not a decoy) if the checksum is the five most common
            // letters in the encrypted name, in order, with ties broken by alphabetization.

            var sum = 0;

            foreach (var item in content)
            {
                // aaaaa-bbb-z-y-x-123[abxyz]
                // aaaaa-bbb-z-y-x-123, abxyz
                var tokens = item.Split('[', ']').ToArray();

                var chars = string.Concat(tokens[0].Where(char.IsLetter));
                var sectorID = int.Parse(string.Concat(tokens[0].Where(char.IsNumber)));

                var checksum = tokens[1];

                var precedence = chars.Distinct().OrderByDescending(c => chars.Count(x => c == x))
                                                 .ThenBy(c => c).ToArray();

                var real = true;
                for (var i = 0; i < checksum.Length; ++i)
                {
                    if (checksum[i] != precedence[i])
                    {
                        real = false;
                        break;
                    }
                }

                if (real)
                {
                    sum += sectorID;

                    var decrypted = string.Concat(chars.Select(c => Rotate(c, sectorID)));
                    if (decrypted.Contains("northpole"))
                    {
                        Console.WriteLine($"object storage: {sectorID}");
                    }
                }
            }

            Console.WriteLine($"sum: {sum}");

        }
    }
}
