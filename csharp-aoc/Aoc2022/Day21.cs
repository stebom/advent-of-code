namespace Day21;
public class Day21
{
    public static void Run()
    {
        var lines = File.ReadAllLines("input_day21.txt").Select(l => l.Split(": ")).ToArray();

        Console.WriteLine(Solve(lines, null)["root"]);
        Console.WriteLine(BinarySearch(lines, Solve(lines, 0)["root_2"]));
    }

    private static Dictionary<string, double> Solve(string[][] lines, double? h)
    {
        var monkey = new Dictionary<string, double>();
        if (h.HasValue) monkey["humn"] = h.Value;

        while (!monkey.ContainsKey("root"))
        {
            foreach (var line in lines)
            {
                if (monkey.ContainsKey(line[0]))
                    continue;

                if (line[1].All(char.IsDigit))
                    monkey[line[0]] = double.Parse(line[1]);
                else
                {
                    var words = line[1].Split(" ");
                    if (monkey.ContainsKey(words[0]) && monkey.ContainsKey(words[2]))
                    {
                        if (line[0] == "root")
                        {
                            monkey["p2"] = Math.Sign(operate("-", monkey[words[0]], monkey[words[2]]));
                            monkey["root_1"] = monkey[words[0]];
                            monkey["root_2"] = monkey[words[2]];
                        }
                        monkey[line[0]] = operate(words[1], monkey[words[0]], monkey[words[2]]);
                    }
                }
            }
        }
        return monkey;
    }

    private static double operate(string op, double v1, double v2) => op switch
    {
        "*" => v1 * v2,
        "/" => v1 / v2,
        "+" => v1 + v2,
        "-" => v1 - v2,
        _ => throw new NotImplementedException()
    };

    public static double BinarySearch(string[][] lines, double target)
    {
        var low = 0L;
        var high = long.MaxValue;
        while (low < high)
        {
            var mid = (low + high) / 2;
            var score = target - Solve(lines, mid)["root_1"];

            if (score < 0) low = mid;
            else if (score == 0) return mid;
            else high = mid;
        }
        return double.NaN;
    }
}
