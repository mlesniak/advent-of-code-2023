using System.Diagnostics;

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
        var commands = File
            .ReadAllText("15.txt")
            .Split(",");

        var boxes = new Dictionary<int, List<Lense>>();
        for (int i = 0; i < 255; i++)
        {
            boxes[i] = new List<Lense>();
        }

        foreach (var command in commands)
        {
            Console.Out.WriteLine($"\n{command}");
            Process(boxes, command);
            Debug(boxes);
        }

        Console.Out.WriteLine("\nFinal State");
        Debug(boxes);
    }

    private static void Debug(Dictionary<int, List<Lense>> boxes)
    {

        for (int i = 0; i < 255; i++)
        {
            var l = boxes[i];
            if (l.Count == 0)
            {
                continue;
            }
            var s = string.Join(", ", l);
            Console.Out.WriteLine($"{i}: {s}");
        }
    }

    static void Process(Dictionary<int, List<Lense>> boxes, string command)
    {
        var parts = command.Split("=");
        if (parts.Length == 1)
        {
            // Removal command.
            var label = command.Substring(0, command.Length - 1);
            var position = Hash(label);
            var list = boxes[position];
            list.RemoveAll(l => { return l.Name == label; });
        }
        else
        {
            // Addition command.
            var label = parts[0];
            var position = Hash(label);
            var value = Int32.Parse(parts[1]);
            var lense = new Lense(label, value);
            // Try to Replace.
            var list = boxes[position];

            // Try to replace.
            bool found = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == label)
                {
                    list[i] = lense;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                list.Add(lense);
            }
        }
    }
}

public record Lense(string Name, int Focal)
{
    public override string ToString() => $"[{Name} {Focal}]";
}
