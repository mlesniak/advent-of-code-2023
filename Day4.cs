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

    public static void Part2()
    {
        var lines = File.ReadAllLines("4.txt");

        var cache = new Dictionary<string, (int, long)>();

        var q = new Queue<string>();
        foreach (var line in lines)
        {
            q.Enqueue(line);
        }

        var counts = new Dictionary<int, int>();
        long result = 0;
        while (q.Any())
        {
            var line = q.Dequeue();
            result++;
            (int cardNo, long correctNumbers) = ComputeCorrectNumbers(cache, line);

            counts[cardNo] = counts.GetValueOrDefault(cardNo, 0) + 1;

            // Console.WriteLine($"{cardNo}: {correctNumbers}");
            for (int i = cardNo+1; i <= lines.Length && i < cardNo + 1+ correctNumbers; i++)
            {
                // Console.WriteLine($"  {i}: {lines[i]}");
                q.Enqueue(lines[i-1]);
            }
        }

        Console.WriteLine(result);
        // foreach (var keyValuePair in counts)
        // {
            // Console.WriteLine(keyValuePair);
        // }
        // foreach (var keyValuePair in cache)
        // {
        //     Console.WriteLine(keyValuePair);
        // }
    }

    private static (int, long) ComputeCorrectNumbers(Dictionary<string, (int, long)> cache, string line)
    {
        if (cache.TryGetValue(line, out (int, long) result))
        {
            return result;
        }

        var nums = line.Split(":")[1];
        var cardNo = Int32.Parse(line.Split(":")[0].Split(" ")[^1].Trim());

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
        var computed = (cardNo, chosen.Count);
        cache[line] = computed;
        return computed;
    }

}
