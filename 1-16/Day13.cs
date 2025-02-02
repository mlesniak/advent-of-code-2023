using System.Text;

namespace Lesniak.AoC2023;

public static class Day13
{
    public static void Part1()
    {
        var fields = File
            .ReadAllText("13.txt")
            .Split("\r\n\r\n");

        long sum = 0;
        foreach (var field in fields)
        {
            // Console.Out.WriteLine($"FIELD:\n{field}");
            var result = Compute(field, (-1, -1));
            // Console.Out.WriteLine($"{result}");
            sum += result;
        }

        Console.Out.WriteLine(sum);
    }

    private static long Compute(string field, (long, long) ignoreIndex)
    {
        var lines = field.Split("\r\n");

        // Check for similar column.
        var columns = Enumerable.Range(0, lines[0].Length)
            .Select(i => new string(lines.Select(s => s[i]).ToArray()))
            .ToArray();

        // Check for similar columns.
        for (var i = 0; i < columns.Length - 1; i++)
        {
            if (i == ignoreIndex.Item1)
            {
                continue;
            }
            if (columns[i] == columns[i + 1])
            {
                // Check if all available columns on the outside
                // are mirrored as well.
                int l = i - 1;
                int r = i + 2;
                bool ok = true;
                while (l >= 0 && r < columns.Length)
                {
                    if (columns[l] != columns[r])
                    {
                        ok = false;
                        break;
                    }
                    l--;
                    r++;
                }

                if (ok)
                {
                    // Starting at 1.
                    return i + 1;
                }
            }
        }

        // Check for similar row.
        for (var i = 0; i < lines.Length - 1; i++)
        {
            if (ignoreIndex.Item2 == i)
            {
                continue;
            }
            if (lines[i] == lines[i + 1])
            {
                // Check if all available columns on the outside
                // are mirrored as well.
                int l = i - 1;
                int r = i + 2;
                bool ok = true;
                while (l >= 0 && r < lines.Length)
                {
                    if (lines[l] != lines[r])
                    {
                        ok = false;
                        break;
                    }
                    l--;
                    r++;
                }

                if (ok)
                {
                    // Starting at 1.
                    return (i + 1) * 100;
                }
            }
        }

        return -1;
    }

    public static void Part2()
    {
        var fields = File
            .ReadAllText("13.txt")
            .Split("\r\n\r\n");

        long sum = 0;
        foreach (var field in fields)
        {
            // Console.Out.WriteLine($"FIELD:\n{field}");
            // If it's < 1000, it's col, if it's > 1000, it's row
            (long, long) ignoreIndex = Compute2(field);

            int i = 0;
            while (true)
            {
                var sb = new StringBuilder(field);
                if (sb[i] == '#')
                {
                    sb[i] = '.';
                }
                else if (sb[i] == '.')
                {
                    sb[i] = '#';
                }
                else
                {
                    // Newline or so.
                    i++;
                    continue;
                }
                var newField = sb.ToString();

                // Console.Out.WriteLine($"\nChecking\n{newField}");
                var result = Compute(newField, ignoreIndex);
                // Console.Out.WriteLine($"-> {result}");
                if (result == -1)
                {
                    i++;
                    continue;
                }

                // Console.Out.WriteLine($"{result}");
                sum += result;
                break;
            }
        }

        Console.Out.WriteLine(sum);
    }

    // Return index to ignore.
    // (column, row)
    // -1 if not available.
    private static (long, long) Compute2(string field)
    {
        var lines = field.Split("\r\n");

        // Check for similar column.
        var columns = Enumerable.Range(0, lines[0].Length)
            .Select(i => new string(lines.Select(s => s[i]).ToArray()))
            .ToArray();

        // Check for similar columns.
        for (var i = 0; i < columns.Length - 1; i++)
        {
            if (columns[i] == columns[i + 1])
            {
                // Check if all available columns on the outside
                // are mirrored as well.
                int l = i - 1;
                int r = i + 2;
                bool ok = true;
                while (l >= 0 && r < columns.Length)
                {
                    if (columns[l] != columns[r])
                    {
                        ok = false;
                        break;
                    }
                    l--;
                    r++;
                }

                if (ok)
                {
                    return (i, -1);
                }
            }
        }

        // Check for similar row.
        for (var i = 0; i < lines.Length - 1; i++)
        {
            if (lines[i] == lines[i + 1])
            {
                // Check if all available columns on the outside
                // are mirrored as well.
                int l = i - 1;
                int r = i + 2;
                bool ok = true;
                while (l >= 0 && r < lines.Length)
                {
                    if (lines[l] != lines[r])
                    {
                        ok = false;
                        break;
                    }
                    l--;
                    r++;
                }

                if (ok)
                {
                    return (-1, i);
                }
            }
        }

        // TODO(mlesniak) can this actually happen more often?
        return (-1, -1);
    }

    private static bool NearlyEqual(string a, string b)
    {
        int differences = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
            {
                differences++;
            }
        }
        return differences <= 1;
    }
}
