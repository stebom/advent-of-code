namespace AdventOfCode;

internal class Day20
{
    interface IPulse;
    record struct HighPulse : IPulse;
    record struct LowPulse : IPulse;

    class Module {
        public List<Module> Outputs { get; } = [];
    };

    /// <summary>
    /// Flip-flop modules (prefix %) are either on or off; they are initially off.
    /// </summary>
    class FlipFlop : Module
    {
        public bool State { get; set; } = false;
    }

    /// <summary>
    /// Conjunction modules (prefix &) remember the type of the most recent pulse received from each of their connected input modules;
    /// they initially default to remembering a low pulse for each input.
    /// </summary>
    class Conjunction : Module
    {
        public Dictionary<Module, IPulse> Inputs { get; set; } = [];
    }

    record Signal(Module Sender, Module Receiver, IPulse Pulse);

    static Signal[] Process(Signal signal)
    {
        if (signal.Receiver is FlipFlop flipFlop)
        {
            // If a flip-flop module receives a high pulse,
            // it is ignored and nothing happens.
            if (signal.Pulse is HighPulse) { return []; }

            // However, if a flip-flop module receives a low pulse,
            // it flips between on and off.

            // If it was on, it turns off and sends a low pulse.
            // If it was off, it turns on and sends a high pulse.
            IPulse p = flipFlop.State ? new LowPulse() : new HighPulse();
            flipFlop.State = !flipFlop.State;

            return flipFlop.Outputs.Select(m => new Signal(flipFlop, m, p)).ToArray();
        }
        else if (signal.Receiver is Conjunction conjunction)
        {
            // When a pulse is received, the conjunction module first updates its memory for that input.
            conjunction.Inputs[signal.Sender] = signal.Pulse;

            // Then, if it remembers high pulses for all inputs, it sends a low pulse;
            // otherwise, it sends a high pulse.
            IPulse p = conjunction.Inputs.Values.All(p => p is HighPulse) ? new LowPulse() : new HighPulse();
            return conjunction.Outputs.Select(m => new Signal(conjunction, m, p)).ToArray();
        }
        /*
        else if (signal.Receiver is Broadcast broadcast)
        {
            return broadcast.Outputs.Select(m => new Signal(broadcast, m, signal.Pulse)).ToArray();
        }
        else if (signal.Receiver is Test)
        {
            // Ignore test module output
            return [];
        }
        else throw new NotImplementedException();
        */
        return signal.Receiver.Outputs.Select(m => new Signal(signal.Receiver, m, signal.Pulse)).ToArray();
    }

    static readonly string[] TestInput = [
        //"broadcaster -> a, b, c",
        //"%a -> b",
        //"%b -> c",
        //"%c -> inv",
        //"&inv -> a",
        "broadcaster -> a",
        "%a -> inv, con",
        "&inv -> b",
        "%b -> con",
        "&con -> output",
    ];

    public static void Solve()
    {
        //var input = TestInput;
        var input = File.ReadLines(@"2023_20_input.txt");
        var moduleMapping = input.Select(line => line.Split(" -> ")).ToDictionary(key => key[0], value => value[1].Split(", "));

        Dictionary<string, Module> modules = [];

        foreach (var name in moduleMapping.Keys)
        {
            if (name == "broadcaster") { modules.Add(name, new Module()); }
            else if (name[0] == '%') { modules.Add(name[1..], new FlipFlop()); }  // Flip - flop modules(prefix %)
            else if (name[0] == '&') { modules.Add(name[1..], new Conjunction()); }  // Conjunction modules (prefix &)
            else throw new ArgumentException(name);
        }

        foreach (var kvp in moduleMapping)
        {
            var name = kvp.Key == "broadcaster" ? kvp.Key : kvp.Key[1..];
            var module = modules[name];

            foreach (var connected in kvp.Value)
            {
                if (!modules.TryGetValue(connected, out var connectedModule))
                {
                    connectedModule = new Module();
                    // Add a testing module
                    modules.Add(connected, connectedModule);
                }

                module.Outputs.Add(connectedModule);
                if (connectedModule is Conjunction conjunction)
                {
                    conjunction.Inputs.Add(module, new LowPulse());
                }
            }
        }

        List<Signal> signals = [];
        Queue<Signal> queue = new();

        var button = new Module();

        var names = modules.ToDictionary(key => key.Value, value => value.Key);
        names.Add(button, "button");

        var needle = modules.Values.Single(m => m.Outputs.Contains(modules["rx"]));
        var rxInputs = modules.Values.OfType<Conjunction>()
            .Where(c => c.Outputs.Contains(needle))
            .ToDictionary(key => (Module)key, value => (int?)null);

        var presses = 0;

        while (rxInputs.Values.Any(p => p is null))
        {
            presses++;
            var startSignal = new Signal(button, modules["broadcaster"], new LowPulse());
            queue.Enqueue(startSignal);

            while (queue.TryDequeue(out var signal))
            {
                signals.Add(signal);

                //var pulse = signal.Pulse is LowPulse ? "low" : "high";
                //Console.WriteLine($"{names[signal.Sender]} -{pulse}-> {names[signal.Receiver]}");

                if (signal.Pulse is LowPulse && rxInputs.TryGetValue(signal.Receiver, out var value) && value is null)
                {
                    Console.WriteLine($"{names[signal.Sender]} (rx input) got high pulse after {presses} presses");
                    rxInputs[signal.Receiver] = presses;
                }

                foreach (var next in Process(signal))
                {
                    queue.Enqueue(next);
                }
            }

            if (presses == 1000)
            {
                var highPulses = signals.LongCount(s => s.Pulse is HighPulse);
                var lowPulses = signals.Count - highPulses;

                Console.WriteLine($"Total of {highPulses} high pulses and {lowPulses} low pulses after {presses} runs");
                Console.WriteLine($"Part 1: {highPulses * lowPulses}");
            }
        }

        var lcm = rxInputs.Values.Select(i => (long)i!.Value).Aggregate((a, b) => a * b);
        Console.WriteLine($"Part 2: rx recieved low pulse after {lcm} presses");
    }
}