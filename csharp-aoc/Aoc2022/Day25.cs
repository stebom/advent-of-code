namespace Day25;

public class Day25
{
    private static readonly bool Test = false;

    public static void Run()
    {
        if (Test) Console.WriteLine($"{"Raw",10} {"Base 10",10} {"Base 5",10} {"SNAFU",10}");

        var totalSum = 0L;
        foreach (var line in File.ReadAllLines(Test ? "testinput_day25.txt" : "input_day25.txt"))
        {
            if (Test) Console.WriteLine($"{line,10} {ToBase10(line), 10} {ToBase5(ToBase10(line)), 10} {ToSnafu(ToBase10(line)), 10}");
            totalSum += ToBase10(line);
        }

        Console.WriteLine($"totalSum: {totalSum} {ToSnafu(totalSum)}");
    }

    private static long ToBase10(string val)
    {
        static int Val(char c) => c switch {
            '-' => -1,
            '=' => -2,
            _ => int.Parse(new[] { c })
        };

        var sum = 0L;
        for (var i = 0; i < val.Length; i++)
        {
            var c = val[val.Length - 1 - i];
            sum += (long)Math.Pow(5, i) * Val(c);
        }
        return sum;
    }

    private static string ToSnafu(long value)
    {
        var q = new Stack<string>();
        while (value > 0)
        {
            var rem = value % 5;
            value /= 5; 

            if (rem <= 2) {
                q.Push(rem.ToString());
            } else {
                q.Push(rem == 3 ? "=" : "-");
                value += 1;
            }
        }

        return string.Join("", q);
    }

    private static string ToBase5(long value)
    {
        // Step 1 − Divide the decimal number to be converted by the value of the new base.
        // Step 2 − Get the remainder from Step 1 as the rightmost digit (least significant digit) of new base number.
        // Step 3 − Divide the quotient of the previous divide by the new base.
        // Step 4 − Record the remainder from Step 3 as the next digit(to the left) of the new base number.
        // Repeat Steps 3 and 4, getting remainders from right to left, until the quotient becomes zero in Step 3.

        var q = new Stack<long>();
        while (value > 0)
        {
            var r = value / 5;  // step 1, 3
            q.Push(value % 5);  // step 2
            value = r;          // step 4
        }

        return string.Join("", q);
    }
}