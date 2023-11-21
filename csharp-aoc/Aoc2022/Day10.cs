using System.Text;

namespace Day10;

public static class Day10
{
    public static void Run()
    {
        var queue = new Queue<Op>();
        foreach (var op in File.ReadAllLines("input_day10.txt")
                               .Select(l => l.StartsWith("noop") ? (Op)new Noop() : new Add(int.Parse(l.Substring(5)))))
          queue.Enqueue(op);

        var cycle = 1L;
        var register = 1;
        var signal = 0L;
        var crt = new StringBuilder();

        while (queue.Any())
        {
            if (cycle == 20 || (cycle - 20) % 60 == 0)
                signal += cycle * register;

            if (cycle % 40 == 0)
                crt.AppendLine();
            else
            {
                var position = cycle % 40 - 1;
                var lit = register - 1 <= position && position <= register + 1;
                crt.Append(lit ? "#" : " ");
            }   

            var instruction = queue.Peek();
            if (instruction.ProcessCycle())
            {
                register += instruction.Value;
                queue.Dequeue();
            }

            cycle++;
        }

        Console.WriteLine(crt.ToString());
        Console.WriteLine($"Processing complete at {cycle} cycles X={register} S={signal}");
    }

    class Add : Op
    {
        private int cycles_;

        public Add(int value)
        {
            Value = value;
            cycles_ = 2;
        }

        public int Value { get; }
        public bool ProcessCycle() => --cycles_ == 0;
    }

    class Noop : Op
    {
        public int Value => 0;
        public bool ProcessCycle() => true;
    }

    interface Op
    {
        int Value { get; }
        bool ProcessCycle();
    }
}