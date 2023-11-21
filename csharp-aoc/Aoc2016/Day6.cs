
using System.Text;

namespace Aoc2016
{
    internal class Day6
    {
        public static void Run()
        {
            var lines = File.ReadAllLines("day6.txt").Where(l => l != "").ToArray();

            var columns = new string[lines[0].Length];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    columns[j] += lines[i][j];
                }
            }

            var mcc = new StringBuilder();
            var lcc = new StringBuilder();
            foreach (var column in columns)
            {
                var occurences = column.Distinct()
                                       .ToDictionary(k => k, v => column.Count(c => c == v))
                                       .OrderByDescending(c => c.Value);
                mcc.Append(occurences.First().Key);
                lcc.Append(occurences.Last().Key);
            }

            Console.WriteLine(mcc.ToString());
            Console.WriteLine(lcc.ToString());
        }
    }
}
