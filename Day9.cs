namespace Lesniak.AoC2023;

public class Day9
{

    public static void Part1()
    {
        var result = File.ReadAllLines("9.txt")
            .Select(line => line.Split(" "))
            .Select(list => list.Select(elem => Int64.Parse(elem)).ToList())
            .Select(ComputeLastNumber)
            .Sum();
        Console.WriteLine(result);
    }

    private static long ComputeLastNumber(List<long> arg)
    {
        List<List<long>> lists = new ();
        lists.Add(arg);

        // Create sublists.
        while (lists[^1].Any(e => e != 0))
        {
            var nl = new List<long>();
            var nums = lists[^1];
            for (int i = 0; i < nums.Count - 1; i++)
            {
                nl.Add(nums[i + 1] - nums[i]);
            }
            lists.Add(nl);
        }


        // Iterate up to compute last number.
        for (int i = lists.Count - 1 - 1; i >= 0; i--)
        {
            var val = lists[i + 1][^1] + lists[i][^1];
            lists[i].Add(val);
        }

        return lists[0][^1];
    }

    public static void Part2()
    {
        throw new NotImplementedException();
    }
}
