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
            // // Console.WriteLine($"Handling {e}");
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
        //     // Console.WriteLine($"<{part}>");
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

    public static void Part2()
    {
        var inputs = File
            .ReadAllLines("12.txt")
            .Select(line =>
            {
                var es = line.Split(" ");
                var ints = es[1].Split(",").Select(s => Int32.Parse(s)).ToList();
                // return (es[0], ints);
                return (Explode(es[0]), Explode(ints));
            })
            .ToList();

        long sum = 0;
        var cache = new Dictionary<string, long>();
        foreach (var input in inputs)
        {
            // // Console.WriteLine(input.Item1);
            // // Console.WriteLine(string.Join(",", input.Item2));
            sum += Recurse(cache, input.Item1, input.Item2);
        }
        Console.WriteLine(sum);
    }

    private static long Recurse(Dictionary<string, long> cache, string input, List<int> defs, int curSpring = 0,
        string history = "")
    {
        // var key = $"{input}\tdefs={string.Join(",", defs)} curSpring={curSpring}, {history}";
        // Console.WriteLine(key);
        var cacheKey = $"{input}-{string.Join(",", defs)}-{curSpring}";
        if (cache.TryGetValue(cacheKey, out long res))
        {
            return res;
        }

        if (string.IsNullOrEmpty(input))
        {
            if (defs.Count > 1)
            {
                cache[cacheKey] = 0;
                return 0;
            }
            if (defs.Count == 0 && curSpring == 0)
            {
                // Console.WriteLine($"\t\t\t\t\tall zero, {history}");
                cache[cacheKey] = 1;
                return 1;
            }
            if (defs.Count > 0 && defs[0] == curSpring)
            {
                // Console.WriteLine($"\t\t\t\t\tfinal spring, {history}");
                cache[cacheKey] = 1;
                return 1;
            }

            cache[cacheKey] = 0;
            return 0;
        }

        // Still input left, check if the state can be valid at all.
        if (defs.Count == 0 && curSpring > 0)
        {
            cache[cacheKey] = 0;
            return 0;
        }
        if (curSpring > 0 && curSpring > defs[0])
        {
            cache[cacheKey] = 0;
            return 0;
        }

        // Handle inputs.
        if (input[0] == '#')
        {
            return Recurse(cache, input.Substring(1), defs, curSpring + 1, history + input[0]);
        }

        if (input[0] == '.')
        {
            // Do we mark the end of a string of springs from previous char?
            if (curSpring > 0)
            {
                if (curSpring == defs[0])
                {
                    // all fine, continue.
                    return Recurse(cache, input.Substring(1), defs.Slice(1, defs.Count - 1), 0, history + input[0]);
                }

                // Is not a valid state.
                cache[cacheKey] = 0;
                return 0;
            }

            return Recurse(cache, input.Substring(1), defs, curSpring, history + input[0]);
        }

        // .###.##.#...

        if (input[0] == '?')
        {
            long dot = 0;
            if (curSpring > 0)
            {
                // Previous char was a spring. We would end the chain now.
                //
                // All fine, i.e. chain is valid? Reset chain and continue.
                if (curSpring == defs[0])
                {
                    dot = Recurse(cache, input.Substring(1), defs.Slice(1, defs.Count - 1), 0, history + ".");
                }
                else
                {
                    // Did not match with expectation. Abort.
                    dot = 0;
                }


            }
            else
            {
                // Previous char was a dot as well, no running spring count.
                dot = Recurse(cache, input.Substring(1), defs, curSpring, history + ".");
            }

            long spring = Recurse(cache, input.Substring(1), defs, curSpring + 1, history + "#");

            var sum = dot + spring;
            cache[cacheKey] = sum;
            return sum;
        }

        // Console.WriteLine("Reached unknown state");
        return 0;
    }

    private static List<int> Explode(List<int> l)
    {
        var times = 5;
        return Enumerable.Repeat(l, times).SelectMany(i => i).ToList();
    }

    private static string Explode(string s)
    {
        var times = 5;
        return string.Join("?", Enumerable.Repeat(s, times));
    }

}
