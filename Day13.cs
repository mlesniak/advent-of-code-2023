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
            var result = Compute(field);
            // Console.Out.WriteLine($"{result}");
            sum += result;
        }

        Console.Out.WriteLine(sum);
    }


    private static long Compute(string field)
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
                    // Starting at 1.
                    return i + 1;
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
                    // Starting at 1.
                    return (i + 1) * 100;
                }
            }
        }

        return -1;
    }
}