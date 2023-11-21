using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Xml.Schema;

namespace Day15;

public static class Day15
{
    public static void Run()
    {
        const string input = "input.txt";

        var beacons = new HashSet<(int x, int y)>();
        var sensors = new Dictionary<(int x, int y), int>();

        foreach (var l in File.ReadAllText(input).Split("\n"))
        {
            var g = l.Substring(10).Split(':');
            var g1 = g[0].Split(new[] { '=', ',' });
            var g2 = g[1].Substring(23).Split(new[] { '=', ',' });

            var s = (int.Parse(g1[1]), int.Parse(g1[3]));
            var b = (int.Parse(g2[1]), int.Parse(g2[3]));

            sensors.Add(s, Distance(s, b));
            beacons.Add(b);
        };

        var grid = new HashSet<(int x, int y)>();

        var scanline = input == "testinput.txt" ? 10 : 2000000;

        Console.WriteLine($"Scanning line y={scanline}");

        foreach (var p in sensors)
        {
            Console.WriteLine($"  Sensor at x={p.Key.x}, y={p.Key.y} proximity {p.Value}");
            var dy = Math.Abs(scanline - p.Key.y);
            if (dy > p.Value)
            {
                Console.WriteLine("    Sensor can't reach scan line...");
                continue; // sensor can't reach scanline
            }

            var box = Math.Abs(p.Value - dy); // box size (limits to scanned x positions)

            Console.WriteLine($"    Scanning x={p.Key.x - box}-{p.Key.x + box} ({(p.Key.x + box) - (p.Key.x - box)})");
            for (var x = p.Key.x - box; x <= p.Key.x + box; x++)
                grid.Add((x, scanline));
        }

        Console.WriteLine();
        Console.WriteLine($"part 1: {grid.Except(sensors.Keys).Except(beacons).Count()}");
        Console.WriteLine();

        var limit = input == "testinput.txt" ? 20 : 4000000;


        Console.WriteLine($"Scanning sensor data (0,0) to ({limit},{limit})");
        var scanlines = new Dictionary<int, List<(int,int)>>();
        foreach (var p in sensors)
        {
            Console.WriteLine($"  Sensor at x={p.Key.x}, y={p.Key.y} proximity {p.Value}");

            var startx = Math.Max(0, p.Key.x - p.Value);
            var endx = Math.Min(limit, p.Key.x + p.Value);

            var starty = Math.Max(0, p.Key.y - p.Value);
            var endy = Math.Min(limit, p.Key.y + p.Value);

            for (var y = starty; y <= endy; y++)
            {
                var padding = p.Value - Math.Abs(y - p.Key.y);

                var windowLeft = Math.Max(startx, p.Key.x - padding);
                var windowRight = Math.Min(endx, p.Key.x + padding);
                Debug.Assert(windowLeft <= windowRight, $"{windowLeft} < {windowRight} ({windowRight - windowLeft})");

                if (!scanlines.ContainsKey(y))
                {
                    scanlines[y] = new List<(int, int)> { (windowLeft, windowRight) };
                }
                else if (scanlines[y].Any(line => line.Item1 <= windowLeft && windowRight <= line.Item2))
                {
                    // Window already contained
                }
                else
                {
                    var merge = scanlines[y].FirstOrDefault(line => windowLeft <= line.Item1 && line.Item2 <= windowRight, (-1,-1));
                    if (merge.Item1 != -1)
                    {
                        scanlines[y].Remove(merge);
                        scanlines[y].Add((windowLeft, windowRight));
                    }
                    else
                    {
                        scanlines[y].Add((windowLeft, windowRight));
                    }
                }
            }
        }

        Console.WriteLine($"Merging scan lines");
        foreach (var kvp in scanlines)
        {
            var windows = kvp.Value.ToList();
            while (windows.Any())
            {
                var window = windows.First();
                var overlapping = kvp.Value.FirstOrDefault(w => w != window && w.Item1 <= window.Item2 && w.Item2 >= window.Item1, (-1, -1));
                if (overlapping.Item1 != -1)
                {
                    kvp.Value.Remove(overlapping);
                    kvp.Value.Remove(window);
                    windows.Remove(overlapping);

                    var merged = (Math.Min(window.Item1, overlapping.Item1), Math.Max(overlapping.Item2, window.Item2));
                    kvp.Value.Add(merged);
                    windows.Add(merged);
                }
                else
                {
                    var adjacent = kvp.Value.FirstOrDefault(w => w != window && w.Item2 == window.Item1 - 1 || window.Item2 + 1 == w.Item1, (-1, -1));
                    if (adjacent.Item1 != -1)
                    {
                        kvp.Value.Remove(adjacent);
                        kvp.Value.Remove(window);
                        windows.Remove(adjacent);

                        var merged = (Math.Min(window.Item1, adjacent.Item1), Math.Max(adjacent.Item2, window.Item2));
                        kvp.Value.Add(merged);
                        windows.Add(merged);
                    }
                }

                windows.Remove(window);
            }
        }

        Console.WriteLine($"Finding missing link...");

        var row = scanlines.Single(l => l.Value.Count > 1);
        var scanned = Enumerable.Range(0, limit).ToDictionary(k => k, v => 0);

        Console.WriteLine($"Building list of possible cells");

        foreach (var x in row.Value)
            foreach (var i in Enumerable.Range(x.Item1, x.Item2 - x.Item1 + 1))
                scanned[i] = 1;
        var hit = scanned.Single(s => s.Value == 0);

        Console.WriteLine();
        Console.WriteLine($"part 2: {TuningFrequency((hit.Key, row.Key))}");
        Console.WriteLine();
    }


    public static int Distance((int x, int y) s, (int x,int y) b)
        => Distance(s.x, b.x, s.y, b.y);

    public static int Distance(int x1, int x2, int y1, int y2)
        => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        
    public static long TuningFrequency((int x, int y) p) => (long)p.x * 4000000 + p.y;
}