using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode;

class Aoc_2016_Day_14
{
    const string input = "ngcjuoqr";
    static readonly List<string> hashes = [];
    static readonly HashSet<int> keys = [];

    internal static void Solve()
    {
        Console.WriteLine($"Part 1: {Solve(false)}");
        hashes.Clear();
        keys.Clear();

        Console.WriteLine($"Part 2: {Solve(true)}");
    }

    static int Solve(bool stretch)
    {
        int index = 0;
        while (keys.Count < 64)
        {
            var hash = GetHash(index, stretch);
            var repeat = GetRepeatingChar(hash, 3);

            if (repeat != null)
            {
                var needle = new string(repeat.Value, 5);
                for (var i = index + 1; i <= index + 1000; i++)
                {
                    if (GetHash(i, stretch).Contains(needle))
                    {
                        keys.Add(index);
                    }
                }
            }

            index++;
        }

        return keys.Last();
    }

    static string GetHash(int index, bool stretch = false)
    {
        if (index >= hashes.Count)
        {
            foreach (var e in Enumerable.Range(hashes.Count, index - hashes.Count + 100))
            {
                hashes.Add(Hash(e, stretch));
            }
        }

        return hashes[index];
    }

    static string Hash(int index, bool stretch)
    {
        var hash = Convert.ToHexString(MD5.HashData(Encoding.ASCII.GetBytes($"{input}{index}"))).ToLower();
        if (!stretch) return hash;

        foreach (var _ in Enumerable.Range(0, 2016))
        {
            hash = Convert.ToHexString(MD5.HashData(Encoding.ASCII.GetBytes(hash))).ToLower();
        }
        return hash;
    }

    static char? GetRepeatingChar(string input, int length)
    {
        char c = input[0];
        var len = 1;
        
        for (int i = 1; i < input.Length; i++)
        {
            if (c == input[i]) { len++; }
            else
            {
                if (len >= length)
                {
                    return c;
                }
                
                c = input[i];
                len = 1;
            }
        }

        if (len >= length)
        {
            return c;
        }

        return null;
    }
}