
namespace Aoc2015
{
    internal class Day8
    {
        public static void Run()
        {
            var characters = 0;
            var eval = 0;
            var escaped = 0;

            foreach (var line in File.ReadAllLines("2015_day_8_input.txt"))
            {
                for (var i = 0; i < line.Length; i++)
                {
                    if (line[i] == '\\' && line[i + 1] == 'x')
                    {
                        i += 3;
                        escaped += 4; // part 2
                    }
                    else if (line[i] == '\\')
                    {
                        i += 1;
                        escaped += 3; // part 2
                    }

                    eval++;
                }

                escaped += 4;
                characters += line.Length;
            }

            Console.WriteLine(characters);
            Console.WriteLine(eval);
            Console.WriteLine(escaped);
            Console.WriteLine(eval - characters);
            Console.WriteLine(escaped - characters);
        }
    }
}
