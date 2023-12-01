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
}
