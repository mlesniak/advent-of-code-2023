namespace Lesniak.AoC2023;

public class Day1
{
    public static void Part1()
    {
        var result = File
            .ReadAllLines("1.txt")
            .Select(line =>
            {
                return line
                    .Where(char.IsDigit)
                    .Select(c => c - '0')
                    .ToArray();
            })
            // .Select(k =>
            // {
            //     Console.WriteLine(string.Join(' ', k));
            //     return k;
            // })
            .Select(ints => ints[0] * 10 + ints[^1])
            .Sum();
        Console.WriteLine(result);
    }

    public static void Part2()
    {
        var mapping = new Dictionary<string, int>
        {
            {"one", 1},
            {"two", 2},
            {"three", 3},
            {"four", 4},
            {"five", 5},
            {"six", 6},
            {"seven", 7},
            {"eight", 8},
            {"nine", 9}
        };

        var result = File
            .ReadAllLines("1.txt")
            .Select(line => line
                .Select((c, i) =>
                {
                    if (char.IsDigit(c))
                    {
                        var num = c - '0';
                        return num;
                    }

                    foreach (KeyValuePair<string, int> pair in mapping)
                    {
                        if (line.AsSpan(i).StartsWith(pair.Key))
                        {
                            return pair.Value;
                        }
                    }

                    return -1;

                })
                .Where(n => n != -1)
                .ToArray())
            // .Select(k =>
            // {
            //     Console.WriteLine(string.Join(' ', k));
            //     return k;
            // })
            .Select(ints => ints[0] * 10 + ints[^1])
            .Sum();
        Console.WriteLine(result);
    }
}
