namespace Lesniak.AoC2023;

public class Day2
{
    public static void Part1()
    {
        var result = File
            .ReadAllLines("2.txt")
            .Select(IsValid)
            .Where(n => n > 0)
            .Sum();
        Console.WriteLine(result);
    }
    
    public static void Part2()
    {
        var result = File
            .ReadAllLines("2.txt")
            .Select(MinimumValidChoice)
            .Where(n => n > 0)
            .Sum();
        Console.WriteLine(result);
    }

    private static int MinimumValidChoice(string line)
    {
        Console.WriteLine(line);
        var parts = line.Split(":");
        var choices = parts[1].Split(";").Select(s => s.Trim());

        // rgb
        foreach (var choice in choices)
        {
            Console.WriteLine(choice);
        }
        List<List<int>> maxs = choices.Select(ComputeMax).ToList();
        
        // max of r, max of g, max of b
        var maxr = maxs.Select(l => l[0]).Max();
        var maxg = maxs.Select(l => l[1]).Max();
        var maxb = maxs.Select(l => l[2]).Max();

        return maxr * maxg * maxb;
    }
    
    // rgb
    private static List<int> ComputeMax(string choice)
    {
        int red = 0;
        int green = 0;
        int blue = 0;

        var elems = choice.Split(",").Select(s => s.Trim());
        foreach (var elem in elems)
        {
            var es = elem.Split(" ");
            var n = Int32.Parse(es[0]);
            switch (es[1])
            {
                case "green":
                    green = n;
                    break;
                case "blue":
                    blue = n;
                    break;
                case "red":
                    red = n;
                    break;
            }
        }

        return new List<int> {red, green, blue};
    }

    // Returns the game id if this is a valid configuration,
    // -1 otherwise.
    private static int IsValid(string line)
    {
        // Console.WriteLine(line);
        var parts = line.Split(":");
        var choices = parts[1].Split(";").Select(s => s.Trim());

        if (choices.All(IsValidChoice))
        {
            var gameId = Int32.Parse(parts[0].Split(" ")[1]);
            return gameId;
        }

        return -1;
    }

    private static bool IsValidChoice(string choice)
    {
        // Console.WriteLine(choice);
        const int maxRed = 12;
        const int maxGreen = 13;
        const int maxBlue = 14;

        int red = 0;
        int green = 0;
        int blue = 0;

        var elems = choice.Split(",").Select(s => s.Trim());
        foreach (var elem in elems)
        {
            var es = elem.Split(" ");
            var n = Int32.Parse(es[0]);
            switch (es[1])
            {
                case "green":
                    green = n;
                    break;
                case "blue":
                    blue = n;
                    break;
                case "red":
                    red = n;
                    break;
            }
        }

        return maxRed >= red && maxGreen >= green && maxBlue >= blue;
    }
}
