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
            .Select(Int32.Parse);

        // Use the order as defined in the file.
        // Let's hope it's the right one.
        var maps = new List<RangeMap>();

        for (var i = 1; i < parts.Length; i++)
        {
            var part = parts[i];
            var rm = new RangeMap(part);
            maps.Add(rm);
        }

        var result = Int32.MaxValue;
        foreach (var seed in seeds)
        {
            int step = seed;
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
        for (int i = 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split(" ").Select(Int32.Parse).ToList();
            ranges.Add(new Range(parts[0], parts[1], parts[2]));
        }
    }

    public int ComputeDestination(int source)
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

record Range(int Destination, int Source, int Length)
{
    public bool Contains(int position)
    {
        return position >= Source && position <= Source + Length;
    }

    // 88 18 7
    // destination: 88 89 90 91 92 93 94 
    // source:      18 19 20 21 22 23 24
    public int Transpose(int position)
    {
        return Destination + (position - Source);
    }
}
