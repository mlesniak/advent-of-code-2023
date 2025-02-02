using System.Text;

namespace Lesniak.AoC2023;

class Number(string value)
{
    public long Value { get; } = int.Parse(value);

    public override string ToString() => $"{Value}";
}

public class Day3
{
    public static void Part1()
    {
        var lines = File.ReadAllLines("3.txt");
        var numbers = new Dictionary<Position, Number>();
        var symbols = new Dictionary<Position, char>();

        for (var row = 0; row < lines.Length; row++)
        {
            var line = lines[row];
            for (var col = 0; col < line.Length; col++)
            {
                if (char.IsDigit(line[col]))
                {
                    var startCol = col;
                    var sb = new StringBuilder();
                    while (col < line.Length && char.IsDigit(line[col]))
                    {
                        sb.Append(line[col]);
                        col++;
                    }

                    var number = new Number(sb.ToString());
                    for (var s = startCol; s < col; s++)
                    {
                        numbers[new Position(s, row)] = number;
                    }
                }


                if (col == line.Length || line[col] == '.')
                {
                    continue;
                }

                symbols[new Position(col, row)] = line[col];
            }
        }

        // foreach (var keyValuePair in numbers)
        // {
        //     Console.WriteLine(keyValuePair);
        // }
        var nums = new HashSet<Number>();
        foreach (var symbol in symbols)
        {
            // Find surrounding numbers.
            var symPos = symbol.Key;
            var neighbors = new List<(int, int)> {(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)};
            foreach (var neighbor in neighbors)
            {
                var pot = new Position(symPos.X + neighbor.Item1, symPos.Y + neighbor.Item2);
                if (numbers.TryGetValue(pot, out Number res))
                {
                    nums.Add(res);
                }
            }
        }

        // foreach (var number in nums)
        // {
        //     Console.WriteLine($"num {number}");
        // }
        var result = nums.ToList().Select(k => k.Value).Sum();
        Console.WriteLine(result);
    }

    public static void Part2()
    {
        var lines = File.ReadAllLines("3.txt");
        var numbers = new Dictionary<Position, Number>();
        var symbols = new Dictionary<Position, char>();

        for (var row = 0; row < lines.Length; row++)
        {
            var line = lines[row];
            for (var col = 0; col < line.Length; col++)
            {
                if (char.IsDigit(line[col]))
                {
                    var startCol = col;
                    var sb = new StringBuilder();
                    while (col < line.Length && char.IsDigit(line[col]))
                    {
                        sb.Append(line[col]);
                        col++;
                    }

                    var number = new Number(sb.ToString());
                    for (var s = startCol; s < col; s++)
                    {
                        numbers[new Position(s, row)] = number;
                    }
                }


                if (col == line.Length || line[col] == '.')
                {
                    continue;
                }

                symbols[new Position(col, row)] = line[col];
            }
        }

        // foreach (var keyValuePair in numbers)
        // {
        //     Console.WriteLine(keyValuePair);
        // }
        long sum = 0;
        foreach (var symbol in symbols)
        {
            if (symbol.Value != '*')
            {
                continue;
            }

            // Find surrounding numbers.
            var symPos = symbol.Key;
            var neighbors = new List<(int, int)> {(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)};
            var nums = new HashSet<Number>();
            foreach (var neighbor in neighbors)
            {
                var pot = new Position(symPos.X + neighbor.Item1, symPos.Y + neighbor.Item2);
                if (numbers.TryGetValue(pot, out Number res))
                {
                    nums.Add(res);
                }
            }

            if (nums.Count != 2)
            {
                continue;
            }

            var list = nums.Select(n => n.Value).ToList();
            sum += list[0] * list[1];
        }

        // foreach (var number in nums)
        // {
        //     Console.WriteLine($"num {number}");
        // }
        // var result = nums.ToList().Select(k => k.Value).Sum();
        // Console.WriteLine(result);
        Console.WriteLine(sum);
    }

}
