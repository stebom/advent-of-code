using System.Security.Cryptography;
using System.Text;

namespace Aoc2016
{
    static class Day5
    {
        static readonly string input = "ojvtpuvg";
        internal static void Run()
        {
            var positions = new Dictionary<int, char>();
            var doorId = new StringBuilder();
            var counter = 0;

            while (true)
            {
                var hashBytes = MD5.HashData(Encoding.ASCII.GetBytes($"{input}{counter++}"));
                var checksumStr = Convert.ToHexString(hashBytes);

                if (checksumStr.StartsWith("00000"))
                {
                    if (doorId.Length < 8)
                    {
                        Console.WriteLine($"Appending {checksumStr[5]} to doorId");
                        doorId.Append(checksumStr[5]);
                    }

                    if (char.IsNumber(checksumStr[5]))
                    {
                        var pos = checksumStr[5] - '0';
                        if (!positions.ContainsKey(pos) && pos < 8)
                        {
                            Console.WriteLine($"Appending {pos} => {checksumStr[6]} to password");
                            positions[pos] = checksumStr[6];
                        }
                    }

                    if (positions.Count == 8 && doorId.Length == 8)
                    {
                        break;
                    }
                }
            }

            Console.WriteLine($"part1: {doorId.ToString().ToLower()}");

            var password = new string(positions.OrderBy(p => p.Key).Select(p => p.Value).ToArray());
            Console.WriteLine($"part2: {password.ToLower()}");
        }
    }
}
