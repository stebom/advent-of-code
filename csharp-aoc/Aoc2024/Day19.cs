using System.Data;
using System.Text;

namespace AdventOfCode;

internal static class Day19
{

    static readonly string[][] TestInput = [
        [
            "px{a<2006:qkq,m>2090:A,rfg}",
            "pv{a>1716:R,A}",
            "lnx{m>1548:A,A}",
            "rfg{s<537:gd,x>2440:R,A}",
            "qs{s>3448:A,lnx}",
            "qkq{x<1416:A,crn}",
            "crn{x>2662:A,R}",
            "in{s<1351:px,qqz}",
            "qqz{s>2770:qs,m<1801:hdj,R}",
            "gd{a>3333:R,R}",
            "hdj{m>838:A,pv}",
        ],
        [
            "{x=787,m=2655,a=1222,s=2876}",
            "{x=1679,m=44,a=2067,s=496}",
            "{x=2036,m=264,a=79,s=2244}",
            "{x=2461,m=1339,a=466,s=291}",
            "{x=2127,m=1623,a=2188,s=1013}",
        ]
    ];

    record Workflow(string Name, IInstruction[] Instructions);

    interface IInstruction;

    record Test(char Parameter, char Operation, int Value, string To) : IInstruction;
    record Proxy(string To) : IInstruction;

    static Workflow ToWorkflow(this string s)
    {
        var span = s.AsSpan();
        var lbrace = span.IndexOf('{');
        var name = span[0..lbrace];
        var rest = span[(lbrace + 1)..(span.Length - 1)];

        Span<Range> instructions = stackalloc Range[5];

        var numInstructions = rest.Split(instructions, ',');
        var ins = new IInstruction[numInstructions];

        for (var i = 0; i < numInstructions; i++)
        {
            var current = rest[instructions[i]];

            if (current.Length == 1)
            {
                ins[i] = new Proxy(current.ToString());
            }
            else
            {
                ins[i] = current[1] switch
                {
                    '>' or '<' => new Test(current[0],
                                           current[1],
                                           int.Parse(current[2..current.IndexOf(':')]),
                                           current[(current.IndexOf(':')+1)..].ToString()),
                    _ => new Proxy(current.ToString())
                };
            }
        }

        return new(name.ToString(), ins);
    }

    record Part(int x, int m, int a, int s);

    static Part ToPart(this string s)
    {
        var span = s.AsSpan()[1..(s.Length-1)];
        Span<Range> destination = stackalloc Range[4];
        span.Split(destination, ',');
        return new(
            int.Parse(span[(destination[0].Start.Value + 2)..(destination[0].End.Value)]),
            int.Parse(span[(destination[1].Start.Value + 2)..(destination[1].End.Value)]),
            int.Parse(span[(destination[2].Start.Value + 2)..(destination[2].End.Value)]),
            int.Parse(span[(destination[3].Start.Value + 2)..(destination[3].End.Value)])
        );
    }

    public static void Solve()
    {
        Part1(TestInput);
        Part2(TestInput);
        
        var input = File.ReadAllText(@"2023_19_input.txt")
            .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToArray())
            .ToArray();
        Part1(input);
        Part2(input);
    }

    static void Part1(string[][] input)
    {
        var parts = input[1].Select(ToPart).ToList();
        var workflows = input[0].Select(ToWorkflow).ToDictionary(key => key.Name, value => value);

        var accepted = new List<Part>();

        foreach (var part in parts)
        {
            string workflow = "in";

            while (workflow != "R" && workflow != "A")
            {
                foreach (var instruction in workflows[workflow].Instructions)
                {
                    if (instruction is Test test)
                    {
                        var parameter = test.Parameter switch { 'x' => part.x, 'm' => part.m, 'a' => part.a, 's' => part.s, _ => throw new NotImplementedException() };
                        var outcome = test.Operation switch { '>' => parameter > test.Value, '<' => parameter < test.Value, _ => throw new NotImplementedException() };

                        if (outcome)
                        {
                            workflow = test.To;
                            break;
                        }
                    }
                    else
                    {
                        workflow = ((Proxy)instruction).To;
                        break;
                    }
                }
            }

            if (workflow == "A") { accepted.Add(part); }
        }

        Console.WriteLine($"Sum: {accepted.Sum(p => p.x + p.m+ p.a+ p.s)}");
    }

    record QueueItem(Dictionary<char, Range> Ranges, string Workflow, int InstructionIndex);
    
    static void Part2(string[][] input)
    {
        var workflows = input[0].Select(ToWorkflow).ToDictionary(key => key.Name, value => value);

        List<Dictionary<char, Range>> accepted = [];

        Stack<QueueItem> queue = new();

        queue.Push(new(
            Ranges: new() {
                ['x'] = new(1, 4000),
                ['m'] = new(1, 4000),
                ['a'] = new(1, 4000),
                ['s'] = new(1, 4000)
            },
            Workflow: "in",
            InstructionIndex: 0
        ));

        while (queue.TryPop(out var current))
        {
            var workflow = current.Workflow;

            if (workflow == "A")
            {
                accepted.Add(current.Ranges);
            }
            else if (workflow == "R")
            {
                // Discard range
            }
            else
            {
                var instruction = workflows[workflow].Instructions[current.InstructionIndex];

                if (instruction is Test test)
                {
                    var range = current.Ranges[test.Parameter];

                    if (test.Operation == '>')
                    {
                        if (range.Start.Value > test.Value)
                        {
                            // Entirely >, go to next workflow
                            queue.Push(new(current.Ranges, test.To, 0));
                        }
                        else if ((range.Start.Value + range.End.Value - 1) <= test.Value)
                        {
                            // Entirely <=, go to next rule
                            queue.Push(new(current.Ranges, workflow, current.InstructionIndex + 1));
                        }
                        else
                        {
                            // Overlap
                            queue.Push(new(
                                Ranges: new(current.Ranges) { [test.Parameter] = new(test.Value + 1, range.Start.Value + range.End.Value - test.Value - 1) },
                                Workflow: test.To,
                                InstructionIndex: 0)
                            );

                            // Leftover
                            queue.Push(new(
                                Ranges: new(current.Ranges) { [test.Parameter] = new(range.Start.Value, test.Value - range.Start.Value + 1) },
                                Workflow: workflow,
                                InstructionIndex: current.InstructionIndex + 1)
                            );
                        }
                    }
                    else if (test.Operation == '<')
                    {
                        if (range.Start.Value < test.Value)
                        {
                            if ((range.Start.Value + range.End.Value) <= test.Value)
                            {
                                // Entirely < than, go to target
                                queue.Push(new(current.Ranges, test.To, 0));
                            }
                            else
                            {
                                // Overlap
                                queue.Push(new(
                                    Ranges: new(current.Ranges) { [test.Parameter] = new(range.Start.Value, test.Value - range.Start.Value) },
                                    Workflow: test.To,
                                    InstructionIndex: 0)
                                );

                                // Leftover
                                queue.Push(new(
                                    Ranges: new(current.Ranges) { [test.Parameter] = new(test.Value, range.Start.Value + range.End.Value - test.Value) },
                                    Workflow: workflow,
                                    InstructionIndex: current.InstructionIndex + 1)
                                );
                            }
                        }
                        else
                        {
                            // Entirely >=, go to next rule
                            queue.Push(new(current.Ranges, workflow, current.InstructionIndex + 1));
                        }
                    }
                    else throw new NotImplementedException();
                }
                else if (instruction is Proxy proxy)
                {
                    // Transition to next workflow without modification
                    queue.Push(new(current.Ranges, proxy.To, 0));
                }
                else throw new NotImplementedException();
            }
        }

        long sum = 0;
        foreach (var range in accepted) {
            sum += (range.TryGetValue('x', out var x) ? (long)x.End.Value : 1) *
                   (range.TryGetValue('m', out var m) ? m.End.Value : 1) *
                   (range.TryGetValue('a', out var a) ? a.End.Value : 1) *
                   (range.TryGetValue('s', out var s) ? s.End.Value : 1);
        }

        Console.WriteLine($"Sum: {sum}");
    }
}
