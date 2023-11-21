namespace Fibonacci;

using System.Collections.Generic;
using System.Diagnostics;

public class Fibonacci
{
    public static IDictionary<int, int> m = new Dictionary<int, int>() {
        { 0, 0 },
        { 1, 1 }
    };

    public static void Run()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        Enumerable.Range(0, 10000).Select(Fib_topdown).ToArray();
        stopwatch.Stop();
        Console.WriteLine("Fib_topdown " + stopwatch.Elapsed);

        stopwatch.Restart();
        Enumerable.Range(0, 10000).Select(Fib_topdown_global).ToArray();
        stopwatch.Stop();
        Console.WriteLine("Fib_topdown_global " + stopwatch.Elapsed);

        stopwatch.Restart();
        Enumerable.Range(0, 10000).Select(Fib_buttomup).ToArray();
        stopwatch.Stop();
        Console.WriteLine("Fib_buttomup " + stopwatch.Elapsed);

        stopwatch.Restart();
        Fib_normal(1000);
        stopwatch.Stop();
        Console.WriteLine("Fib_normal " + stopwatch.Elapsed);
    }

    public static int Fib_normal(int n)
    {
        if (n == 0) return 0;
        if (n == 1) return 1;
        return Fib_normal(n - 1) + Fib_normal(n - 2);
    }

    public static int Fib_topdown(int n)
    {
        var m = new Dictionary<int, int>() {
            { 0, 0 },
            { 1, 1 }
        };
        int Fib(int n)
        {
            if (!m.ContainsKey(n))
                m[n] = Fib(n - 1) + Fib(n - 2);
            return m[n];
        }

        return Fib(n);
    }


    public static int Fib_topdown_global(int n)
    {
        if (!m.ContainsKey(n))
            m[n] = Fib_topdown_global(n - 1) + Fib_topdown_global(n - 2);
        return m[n];
    }

    public static int Fib_buttomup(int n)
    {
        if (n == 0)
            return 0;
        if (n == 1)
            return 1;

        var prev = 0;
        var curr = 1;

        for (var i = 0; i < n - 1; i++)
        {
            var temp = prev + curr;
            prev = curr;
            curr = temp;
        }

        return curr;
    }
}
