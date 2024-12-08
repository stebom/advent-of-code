namespace Aoc2024;

public static class Day7
{
    public static void Solve()
    {
        long part1 = 0;
        long part2 = 0;

        foreach (var line in File.ReadAllLines(@"day7_input.txt"))
        {
            var delim = line.IndexOf(':');
            var sum = long.Parse(line[0..delim]);
            var values = line[(delim + 2)..].Split(' ').Select(long.Parse);

            part1 += Part1(new Queue<long>(values), sum) ? sum : 0;
            part2 += Part2(new Queue<long>(values), sum) ? sum : 0;
        }

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    private static bool Part1(Queue<long> values, long sum)
    {
        var queue = new Queue<(Queue<long>, long)>();
        queue.Enqueue((values, 0));

        while (queue.Count > 0)
        {
            var value = queue.Dequeue();
            if (value.Item1.Count == 0)
            {
                if (value.Item2 == sum)
                {
                    return true;
                }
                continue;
            }

            var next = value.Item1.Dequeue();
            queue.Enqueue((new Queue<long>(value.Item1), next * value.Item2));
            queue.Enqueue((new Queue<long>(value.Item1), next + value.Item2));
        }

        return false;
    }

    enum Operator { Mul, Add, Cat };

    interface IExpression { long Evaluate(); }

    record Branch(Operator Operator, IExpression Left, IExpression Right) : IExpression
    {
        public long Evaluate() => Operator switch
        {
            Operator.Mul => Left.Evaluate() * Right.Evaluate(),
            Operator.Add => Left.Evaluate() + Right.Evaluate(),
            Operator.Cat => long.Parse($"{Left.Evaluate()}{Right.Evaluate()}"),
            _ => throw new NotImplementedException()
        };
    }

    record Node(long Value) : IExpression
    {
        public long Evaluate() => Value;
    }

    private static bool Part2(Queue<long> values, long sum)
    {
        var node = new Node(values.Dequeue());

        foreach (var expression in Expressions(values, node))
        {
            var evaluation = expression.Evaluate();
            if (evaluation == sum)
            {
                return true;
            }
        }

        return false;
    }

    private static IEnumerable<IExpression> Expressions(Queue<long> values, IExpression parent)
    {
        if (values.Count == 0)
        {
            yield return parent;
            yield break;
        }

        var node = new Node(values.Dequeue());

        foreach (var subexpression in Expressions(new Queue<long>(values), new Branch(Operator.Mul, parent, node)))
        {
            yield return subexpression;
        }
        foreach (var subexpression in Expressions(new Queue<long>(values), new Branch(Operator.Add, parent, node)))
        {
            yield return subexpression;
        }
        foreach (var subexpression in Expressions(new Queue<long>(values), new Branch(Operator.Cat, parent, node)))
        {
            yield return subexpression;
        }
    }
}
