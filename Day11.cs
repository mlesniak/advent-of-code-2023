namespace Lesniak.AoC2023;

public class Day11
{
    public static void Part1()
    {
        var lines = File.ReadAllLines("11.txt");

        var rows = new HashSet<long>();
        for (var y = 0; y < lines.Length; y++)
        {
            if (lines[y].All(c => c == '.'))
            {
                rows.Add(y);
            }
        }

        var cols = new HashSet<long>();
        for (var x = 0; x < lines[0].Length; x++)
        {
            var ok = true;
            foreach (var line in lines)
            {
                if (line[x] != '.')
                {
                    ok = false;
                    break;
                }
            }
            if (ok)
            {
                cols.Add(x);
            }
        }

        // TODO(mlesniak) add spacing based on empty rows and columns
        var galaxies = new List<Position>();
        var factor = 1;

        var addX = 0;
        var addY = 0;

        for (var y = 0; y < lines.Length; y++)
        {
            // Are we passing an empty row?
            if (rows.Contains(y))
            {
                addY += factor;
                continue;
            }

            for (var x = 0; x < lines[y].Length; x++)
            {
                // Are we passing an empty col?
                if (cols.Contains(x))
                {
                    addX += factor;
                    continue;
                }

                if (lines[y][x] == '#')
                {
                    galaxies.Add(new(x + addX, y + addY));
                }
            }
            addX = 0;
        }

        // foreach (var position in galaxies)
        // {
        //     Console.WriteLine(position);
        // }
        // var maxX = galaxies.Select(p => p.X).Max() + 1;
        // var maxY = galaxies.Select(p => p.Y).Max() + 1;
        // var n = 1;
        // for (long y = 0; y < maxY; y++)
        // {
        //     for (long x = 0; x < maxX; x++)
        //     {
        //         if (galaxies.Contains(new(x, y)))
        //         {
        //             if (n > 9)
        //             {
        //                 Console.Write("#");
        //             }
        //             else
        //             {
        //                 Console.Write($"{n++}");
        //             }
        //         }
        //         else
        //         {
        //             Console.Write(".");
        //         }
        //     }
        //     Console.WriteLine();
        // }

        long sum = 0;
        for (int i = 0; i < galaxies.Count; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                var g1 = galaxies[i];
                var g2 = galaxies[j];
                // Console.WriteLine($"\nCompute {g1} and {g2}");
                sum += Compute(rows, cols, g1, g2);
            }
        }
        Console.WriteLine(sum);

        // for (long i = 0; i < galaxies.Count; i++)
        // {
        //     Console.WriteLine($"{i+1} {galaxies[i]}");
        // }
    }

    private static long Compute(HashSet<long> rows, HashSet<long> cols, Position g1, Position g2)
    {
        return Math.Abs(g1.X - g2.X) + Math.Abs(g1.Y - g2.Y);
    }
}
