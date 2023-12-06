namespace Lesniak.AoC2023;

public class Day6
{
    public static void Part1()
    {
        var lines = File.ReadAllLines("6.txt");
        var times = lines[0]
            .Split(":")[1]
            .Split(" ")
            .Where(p => p.Length > 0)
            .Select(Int64.Parse)
            .ToList();
        var distances = lines[1]
            .Split(":")[1]
            .Split(" ")
            .Where(p => p.Length > 0)
            .Select(Int64.Parse)
            .ToList();

        // Console.WriteLine(string.Join(" ", times));
        // Console.WriteLine(string.Join(" ", distances));

        long result = 1;
        for (var i = 0; i < times.Count; i++)
        {
            result *= ComputeWins(times[i], distances[i]);
        }
        Console.WriteLine(result);
    }

    public static void Part2()
    {
        var lines = File.ReadAllLines("6.txt");
        var times = Int64.Parse(lines[0]
            .Split(":")[1]
            .Split(" ")
            .Where(p => p.Length > 0)
            .Aggregate((l, r) => l + r));
        var distances = Int64.Parse(lines[1]
            .Split(":")[1]
            .Split(" ")
            .Where(p => p.Length > 0)
            .Aggregate((l, r) => l + r));

        Console.WriteLine(times);
        Console.WriteLine(distances);

        long result = ComputeWins(times, distances);
        Console.WriteLine(result);
    }

    private static long ComputeWins(long time, long distance)
    {
        long wins = 0;
        for (int secondsLoading = 0; secondsLoading < time; secondsLoading++)
        {
            var speed = secondsLoading;
            var travelledDistance = (time - secondsLoading) * speed;
            if (travelledDistance > distance)
            {
                wins++;
            }
        }
        return wins;
    }
}
