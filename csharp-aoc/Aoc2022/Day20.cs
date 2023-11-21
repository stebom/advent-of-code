namespace Day20;

public class Day20
{
    public static HashSet<Link> Links = new HashSet<Link>();
    public static void Run()
    {
        Link start = null;
        Link current = null;

        var part2 = true;

        foreach (var val in File.ReadAllLines("input_Day20.txt").Select(long.Parse))
        {
            var link = new Link(val * (part2 ? 811589153 : 1));
            Links.Add(link);

            if (start == null) start = link;

            if (current == null) current = link;
            else
            {
                current.Right = link;
                link.Left = current;
                current = link;
            }
        }
        current.Right = start;
        start.Left = current;

        PrintLink(start);

        for (var i = 0; i < (part2 ? 10 : 0); i++)
        {
            foreach (var link in Links)
            {
                if (link == start)
                    if (link.Value > 0) start = link.Right;
                    else start = link.Left;

                if (link.Value == 0)
                    continue;

                // disonnect link
                link.Left.Right = link.Right;
                link.Right.Left = link.Left;

                // <-N-R-> <-L-N1-R-> <-L-N2->
                var insertion = Walk(link);

                if (link == insertion)
                    insertion = link.Right;

                link.Left.Right = link.Right;
                link.Right.Left = link.Left;

                link.Left = insertion;
                link.Right = insertion.Right;
                insertion.Right.Left = link;
                insertion.Right = link;

                //PrintLink();
            }
        }
        PrintLink(start);

        var groove = Links.Single(l => l.Value == 0);
        var g1 = WalkRight(groove, 1000);
        var g2 = WalkRight(groove, 2000);
        var g3 = WalkRight(groove, 3000);

        Console.WriteLine($"Groove coordinates: {g1.Value} + {g2.Value} + {g3.Value} = {(long)g1.Value + g2.Value + g3.Value}");

    }

    public static Link Walk(Link current)
    {
        var left = current.Value < 0;
        var steps = Math.Abs(current.Value);

        steps %= Links.Count - 1;
        for (var i = 0; i < steps; i++)
        {
            if (left) current = current.Left;
            else current = current.Right;
        }
        if (left) return current.Left;
        return current;
    }

    public static Link WalkRight(Link current, int steps)
    {
        for (var i = 0; i < steps; i++)
            current = current.Right;
        return current;
    }

    public static void PrintLink(Link start)
    {
        var iterator = start;
        var seen = new HashSet<Link>();
        while (true)
        {
            if (seen.Contains(iterator))
                break;

            Console.Write($"{iterator.Value}, ");

            seen.Add(iterator);

            iterator = iterator.Right;
        }

        Console.WriteLine();
    }

    public class Link
    {
        public Link(long value) => Value = value;
        public long Value { get; }
        public Link? Left { get; set; }
        public Link? Right { get; set; }
    }
}