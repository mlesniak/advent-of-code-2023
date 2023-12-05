using System.Text;

namespace Lesniak.AoC2023;

public class Day5
{
    public static void Part1()
    {
        var input = File.ReadAllText("5.txt");
        var parts = input.Split("\n\n");

        var seeds = parts[0]
            .Split(":")[1]
            .Split(" ")
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .Select(Int64.Parse);

        // Use the order as defined in the file.
        // Let's hope it's the right one.
        var maps = new List<RangeMap>();

        for (var i = 1; i < parts.Length; i++)
        {
            var part = parts[i];
            var rm = new RangeMap(part);
            maps.Add(rm);
        }

        long result = Int64.MaxValue;
        foreach (var seed in seeds)
        {
            long step = seed;
            for (int i = 0; i < maps.Count; i++)
            {
                // Console.WriteLine("\nNEXT STEP");
                // Console.WriteLine(maps[i]);
                step = maps[i].ComputeDestination(step);
                // Console.WriteLine(step);
            }
            result = Math.Min(result, step);
        }

        Console.WriteLine(result);
    }

    public static void Part2()
    {
        var input = File.ReadAllText("5.txt");
        var parts = input.Split("\n\n");

        List<(long, long)> seeds = new();

        var nums = parts[0]
            .Split(":")[1]
            .Split(" ")
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .Select(Int64.Parse)
            .ToList();
        for (var i = 0; i < nums.Count; i += 2)
        {
            seeds.Add((nums[i], nums[i] + nums[i + 1]));
        }

        // Use the order as defined in the file.
        // Let's hope it's the right one.
        var maps = new List<RangeMap>();

        for (var i = 1; i < parts.Length; i++)
        {
            var part = parts[i];
            var rm = new RangeMap(part);
            maps.Add(rm);
        }

        long result = Int64.MaxValue;
        foreach (var seedPair in seeds)
        {
            for (long seed = seedPair.Item1; seed <= seedPair.Item2; seed++)
            {
                long step = seed;
                for (int i = 0; i < maps.Count; i++)
                {
                    // Console.WriteLine("\nNEXT STEP");
                    // Console.WriteLine(maps[i]);
                    step = maps[i].ComputeDestination(step);
                    // Console.WriteLine(step);
                }
                result = Math.Min(result, step);
            }
        }

        Console.WriteLine(result);
    }
}

// Map for a single type.
class RangeMap
{
    public string Name { get; init; }
    private List<Range> ranges = new();

    public RangeMap(string source)
    {
        var lines = source.Split("\n");
        Name = lines[0].Split(" ")[0];
        for (long i = 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split(" ").Select(Int64.Parse).ToList();
            ranges.Add(new Range(parts[0], parts[1], parts[2]));
        }
    }

    public long ComputeDestination(long source)
    {
        foreach (var range in ranges)
        {
            if (range.Contains(source))
            {
                return range.Transpose(source);
            }
        }

        return source;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"{Name}\n");
        foreach (var range in ranges)
        {
            sb.Append($"{range}\n");
        }
        sb.Remove(sb.Length - 1, 1);

        return sb.ToString();
    }
}

record Range(long Destination, long Source, long Length)
{
    public bool Contains(long position)
    {
        return position >= Source && position <= Source + Length;
    }

    // 88 18 7
    // destination: 88 89 90 91 92 93 94 
    // source:      18 19 20 21 22 23 24
    public long Transpose(long position)
    {
        return Destination + (position - Source);
    }
}
