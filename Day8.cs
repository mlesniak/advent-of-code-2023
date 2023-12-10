namespace Lesniak.AoC2023;

record LeftRight(string Left, string Right)
{
    public string Get(char choice)
    {
        if (choice == 'R')
        {
            return Right;
        }

        return Left;
    }
}

public class Day8
{
    public static void Part1()
    {
        var lines = File.ReadAllLines("8.txt");

        var instructions = lines[0];
        var nodes = new Dictionary<string, LeftRight>();

        for (int i = 2; i < lines.Length; i++)
        {
            var line = lines[i];
            var source = line.Substring(0, 3);
            var left = line.Substring(7, 3);
            var right = line.Substring(12, 3);
            nodes[source] = new LeftRight(left, right);
        }

        var cur = "AAA";
        int step = 0;
        int count = 0;
        while (cur != "ZZZ")
        {
            cur = nodes[cur].Get(instructions[step]);
            step = (step + 1) % instructions.Length;
            count++;
        }

        Console.WriteLine(count);
    }
}
