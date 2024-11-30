using System.Collections;
using System.Diagnostics;
using System.Net.Sockets;

namespace Day13;

public static class Day13
{

    public static void Run()
    {
        const string input = "input.txt";

        var indices = File.ReadAllText(input)
            .Split("\n\n")
            .Select((g, i) =>
            {
                var group = g.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                Debug.Assert(group.Length == 2, "bad group length");

                var left = Parse(group[0]);
                var right = Parse(group[1]);

                Console.WriteLine();
                Console.WriteLine($"== Pair {i + 1} ==");
                Console.WriteLine($"- Compare {group[0]} vs {group[1]}");

                var result = Walk(left.GetEnumerator(), right.GetEnumerator());
                Console.WriteLine($"Outcome: {result}");

                if (result == null)
                    Debug.Fail("Inclusive!");
                return result == true ? i + 1 : 0;

            }).ToArray();

        Console.WriteLine("Indices: " + string.Join(',', indices));
        Console.WriteLine(indices.Sum());

        var packets = File.ReadAllLines(input)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(Parse)
            .ToList();

        packets.Add(Parse("[[2]]"));
        packets.Add(Parse("[[6]]"));

        var orderedPackets = packets.Order(new PacketSorter());
        var prettyPackets = orderedPackets.Select(p => string.Join("", Print(p))).ToList();
        var index1 = prettyPackets.IndexOf("[[2]]") + 1;
        var index2 = prettyPackets.IndexOf("[[6]]") + 1;

        foreach (var packet in orderedPackets)
            Console.WriteLine(string.Join("", Print(packet)));

        Console.WriteLine("[[2]]: " + index1);
        Console.WriteLine("[[6]]: " + index2);
        Console.WriteLine(index1 * index2);
    }

    static bool? Walk(IEnumerator left, IEnumerator right, string prefix = "")
    {
        while (left.MoveNext())
        {
            var rightWalk = right.MoveNext();
            if (!rightWalk) {
                Console.WriteLine($"{prefix}  - Right side ran out of items, so inputs are not in the right order");
                return false;
            }

            var leftInt = left.Current is int;
            var rightInt = right.Current is int;

            if (leftInt && rightInt)
            {
                Console.WriteLine($"{prefix}  - Compare {left.Current} vs {right.Current}");

                if ((int)left.Current < (int)right.Current)
                {
                    Console.WriteLine($"{prefix}    - Left side is smaller, so inputs are in the right order");
                    return true;
                }
                else if ((int)left.Current > (int)right.Current)
                {
                    Console.WriteLine($"{prefix}    - Right side is smaller, so inputs are not in the right order");
                    return false;
                }
            }
            else
            {
                if (leftInt)
                {
                    Console.WriteLine($"{prefix}  - Mixed types; convert left to [{left.Current}] and retry comparison");
                }
                else if (rightInt)
                {
                    Console.WriteLine($"{prefix}  - Mixed types; convert right to [{right.Current}] and retry comparison");
                }

                var l = leftInt ? new List<object> { left.Current } : (List<object>)left.Current;
                var r = rightInt ? new List<object> { right.Current } : (List<object>)right.Current;

                Console.WriteLine($"{prefix}  - Compare [{string.Join("", Print(l))}] vs [{string.Join("", Print(r))}]");

                var sub = Walk(l.GetEnumerator(), r.GetEnumerator(), prefix + "  ");
                if (sub != null) return sub;
            }
        }

        if (prefix == "")
        {
            Console.WriteLine($"{prefix}  - Left side ran out of items, so inputs are in the right order");
            return true;
        }
        else
        {
            if (right.MoveNext())
            {
                Console.WriteLine($"{prefix}  - Left side ran out of items, so inputs are in the right order");
                return true;
            }
        }

        return null; // inconclusive
    }

    static IEnumerable<char> Print(List<object> list)
    {
        yield return '[';

        foreach (var o in list)
        {
            if (o is List<object>)
            {
                foreach (var s in Print((List<object>)o))
                    yield return s;
            }

            else if (o is int)
            {
                foreach (var s in o.ToString()!.Select(c => c))
                    yield return s;
            }

            if (o != list.Last())
                yield return ',';
        }

        yield return ']';
    }

    static List<object> Parse(string input)
    {
        // Packet data consists of lists and integers. Each list starts with [,
        // ends with], and contains zero or more comma - separated values (either integers
        // or other lists). Each packet is always a list and appears on its own line.
        var stack = new Stack<List<object>>();
        stack.Push(new List<object>());

        var last = 1;
        for (var i = 1; i < input.Length - 1; i++)
        {
            var c = input[i];
            if (c == '[') {
                var top = new List<object>();
                stack.Peek().Add(top);
                stack.Push(top);
            }

            if ((c == ',' || c == ']') && i - last > 0)
                stack.Peek().Add(int.Parse(input.Substring(last, i - last)));

            if (c == ']')
                stack.Pop();

            if (!char.IsNumber(c))
                last = i + 1;
        }

        if (last < input.Length - 1)
            stack.Peek().Add(int.Parse(input.Substring(last, input.Length - 1 - last)));

        return stack.First();
    }

    class PacketSorter : IComparer<List<object>>
    {
        public int Compare(List<object>? x, List<object>? y)
         => Walk(x!.GetEnumerator(), y!.GetEnumerator()) == true ? -1 : 1;
    }
}