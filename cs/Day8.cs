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

    public static void Part2()
    {
        // Parsing.
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

        // Computation.
        var nexts = nodes.Keys.Where(n => n[^1] == 'A').ToList();

        // Wild guess: The paths are all independent.
        // Compute path to first '..Z' separately. Since
        // they are independent, the LCM of all path length
        // is the solution where they all 'meet'.

        var distances = new List<long>();
        foreach (var next in nexts)
        {
            var d = ComputeDistance(nodes, instructions, next);
            distances.Add(d);
            Console.WriteLine(next);
            Console.WriteLine(d);
        }

        var result = distances.Aggregate(lcm);
        Console.WriteLine(result);
    }

    static long gcf(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static long lcm(long a, long b)
    {
        return (a / gcf(a, b)) * b;
    }

    private static long ComputeDistance(Dictionary<string, LeftRight> nodes, string instructions, string cur)
    {
        int step = 0;
        long count = 0;
        while (cur[^1] != 'Z')
        {
            cur = nodes[cur].Get(instructions[step]);
            step = (step + 1) % instructions.Length;
            count++;
        }
        return count;
    }

}
