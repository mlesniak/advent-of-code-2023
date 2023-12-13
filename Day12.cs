namespace Lesniak.AoC2023;

public class Day12
{
    public static void Part1()
    {

        var inputs = File
            .ReadAllLines("12.txt")
            .Select(line =>
            {
                var es = line.Split(" ");
                var ints = es[1].Split(",").Select(s => Int32.Parse((string)s)).ToList();
                return (es[0], ints);
            })
            .ToList();

        long sum = 0;
        foreach (var input in inputs)
        {
            sum += ComputeValid(input.Item1, input.Item2);
        }
        Console.WriteLine(sum);
    }

    private static long ComputeValid(string expr, List<int> def)
    {
        var q = new Queue<string>();
        q.Enqueue(expr);
        long valid = 0;
        while (q.Count > 0)
        {
            var e = q.Dequeue();
            // Console.WriteLine($"Handling {e}");
            var idx = e.IndexOf('?');
            if (idx == -1)
            {
                if (Valid(e, def))
                {
                    // Console.WriteLine($"Valid configuration: <{e}> against {string.Join(",", def)}");
                    valid++;
                }
            }
            else
            {
                var cs = e.ToCharArray();
                cs[idx] = '.';
                q.Enqueue(new string(cs));
                cs[idx] = '#';
                q.Enqueue(new string(cs));
            }
        }
        return valid;
    }

    private static bool Valid(string config, List<int> def)
    {
        var parts = config.Split(".").Where(s => s.Any()).ToList();
        // foreach (var part in parts)
        // {
        //     Console.WriteLine($"<{part}>");
        // }
        if (parts.Count != def.Count)
        {
            return false;
        }
        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i].Length != def[i])
            {
                return false;
            }
        }

        return true;
    }
}
