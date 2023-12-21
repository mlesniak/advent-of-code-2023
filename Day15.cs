namespace Lesniak.AoC2023;

public class Day15
{
    public static void Part1()
    {
        var result = File
            .ReadAllText("15.txt")
            .Split(",")
            .Select(p => Hash(p))
            .Sum();
        Console.Out.WriteLine(result);
    }

    static int Hash(string s) => s.Aggregate(0, (c, acc) => (c + acc) * 17 % 256);

    public static void Part2()
    {

    }
}
