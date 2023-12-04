namespace Lesniak.AoC2023;

public class Day4
{
    public static void Part1()
    {
        var lines = File.ReadAllLines("4.txt");
        long sum = 0;

        foreach (var line in lines)
        {
            sum += ComputeWin(line);
        }

        Console.WriteLine(sum);
    }

    private static long ComputeWin(string line)
    {
        var nums = line.Split(":")[1];
        var parts = nums.Split("|");
        var winning = parts[0]
            .Split(" ")
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .Select(Int32.Parse)
            .ToHashSet();
        var chosen = parts[1]
            .Split(" ")
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .Select(Int32.Parse)
            .ToHashSet();

        chosen.IntersectWith(winning);
        return (long)Math.Pow(2.0, chosen.Count - 1);
    }
}