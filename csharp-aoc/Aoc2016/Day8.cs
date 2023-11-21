
namespace Aoc2016
{
    internal static class Day8
    {
        internal static void Run()
        {
            // The screen is 50 pixels wide and 6 pixels tall, all of which start off
            var width = 50;
            var height = 6;
            //var width = 7;
            //var height = 3;
            var pixels = new bool[width * height];

            void PrintGrid()
            {
                Console.WriteLine();
                for (int r = 0; r < height; ++r)
                {
                    for (int c = 0; c < width; ++c)
                    {
                        Console.Write(pixels[r * width + c] ? '░' : ' ');
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            foreach (var line in File.ReadAllLines("Day8.txt"))
            {
                /// rect AxB
                ///   turns on all of the pixels in a rectangle at the top-left of the screen which is A wide and B tall.
                bool ParseRect(int cols, int rows)
                {
                    Console.WriteLine($"rect {cols} by {rows}");
                    for (int r = 0; r < rows; ++r)
                    {
                        for (int c = 0; c < cols; ++c)
                        {
                            var index = width * r + c;
                            pixels[index] = true;
                        }
                    }

                    return true;
                }

                /// rotate row y = A by B
                /// shifts all of the pixels in row A (0 is the top row) right by B pixels.
                /// Pixels that would fall off the right end appear at the left end of the row.
                bool ParseRotateRow(int row, int shift)
                {
                    Console.WriteLine($"rotate row {row} by {shift}");

                    var offset = row * width;
                    var slice = pixels.Skip(offset).Take(width).ToArray();

                    for (int i = offset; i < offset + width; ++i)
                    {
                        var pos = ((i - shift) + width) % width;
                        pixels[i] = slice[pos];
                    }

                    return true;
                }

                /// rotate column x = A by B
                /// shifts all of the pixels in column A (0 is the left column) down by B pixels.
                /// Pixels that would fall off the bottom appear at the top of the column.
                bool ParseRotateColumn(int column, int shift)
                {
                    Console.WriteLine($"rotate column {column} {shift}");

                    var slice = pixels.Where((p, idx) => idx % width == column).ToArray();

                    for (int r = 0; r < height; ++r)
                    {
                        var index = r * width + column;
                        var pos = ((r - shift) + height) % height;
                        pixels[index] = slice[pos];
                    }
                    return true;
                }

                var tokens = line.Split(new[] { ' ', '=', 'x', 'y' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                _ = tokens switch
                {
                    ["rect", var a, var b] => ParseRect(int.Parse(a), int.Parse(b)),
                    ["rotate", "row", var a, "b", var b] => ParseRotateRow(int.Parse(a), int.Parse(b)),
                    ["rotate", "column", var a, "b", var b] => ParseRotateColumn(int.Parse(a), int.Parse(b)),
                    _ => throw new NotImplementedException()
                };
            }

            Console.WriteLine($"Lit pixels: {pixels.Where(p => p).Count()}");
            PrintGrid();
        }
    }
}
