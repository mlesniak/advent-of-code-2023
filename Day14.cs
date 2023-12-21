using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Schema;

namespace Lesniak.AoC2023;

public class Day14
{
    public static void Part1()
    {
        var dishes = new List<Position>();
        var rocks = new HashSet<Position>();

        var lines = File.ReadAllLines("14.txt");
        for (var row = 0; row < lines.Length; row++)
        {
            for (var col = 0; col < lines[row].Length; col++)
            {
                switch (lines[row][col])
                {
                    case 'O':
                        dishes.Add(new(col, row));
                        break;
                    case '#':
                        rocks.Add(new(col, row));
                        break;
                }
            }
        }

        // Render(dishes, rocks);
        // Console.Out.WriteLine("");
        Move(dishes, rocks, (0, -1));
        // Render(dishes, rocks);
        var result = Score(dishes, rocks);
        Console.Out.WriteLine(result);
    }

    private static int Score(List<Position> dishes, HashSet<Position> rocks)
    {
        var maxY = Math.Max(dishes.MaxBy(p => p.Y).Y, rocks.MaxBy(p => p.Y).Y);

        var score = 0;
        foreach (var dish in dishes)
        {
            score += (maxY - dish.Y + 1);
        }
        return score;
    }

    private static void Render(List<Position> dishes, HashSet<Position> rocks)
    {
        var maxX = Math.Max(dishes.MaxBy(p => p.X).X, rocks.MaxBy(p => p.X).X);
        var maxY = Math.Max(dishes.MaxBy(p => p.Y).Y, rocks.MaxBy(p => p.Y).Y);

        for (int row = 0; row <= maxY; row++)
        {
            for (int col = 0; col <= maxX; col++)
            {
                if (dishes.Contains(new(col, row)))
                {
                    Console.Out.Write("O");
                }
                else if (rocks.Contains(new(col, row)))
                {
                    Console.Out.Write("#");
                }
                else
                {
                    Console.Out.Write(".");
                }

            }
            Console.Out.WriteLine();
        }
    }

    private static string RenderString(List<Position> dishes, HashSet<Position> rocks)
    {
        var sb = new StringBuilder();
        var maxX = Math.Max(dishes.MaxBy(p => p.X).X, rocks.MaxBy(p => p.X).X);
        var maxY = Math.Max(dishes.MaxBy(p => p.Y).Y, rocks.MaxBy(p => p.Y).Y);

        for (int row = 0; row <= maxY; row++)
        {
            for (int col = 0; col <= maxX; col++)
            {
                if (dishes.Contains(new(col, row)))
                {
                    sb.Append("O");
                }
                else if (rocks.Contains(new(col, row)))
                {
                    sb.Append("#");
                }
                else
                {
                    sb.Append(".");
                }

            }
            sb.Append("\n");
        }

        return sb.ToString();
    }

    class PositionComparerX : IComparer<Position>
    {

        public int Compare(Position? x, Position? y)
        {
            return x.X - y.X;
        }
    }

    class PositionComparerXRev : IComparer<Position>
    {

        public int Compare(Position? x, Position? y)
        {
            return y.X - x.X;
        }
    }

    class PositionComparerY : IComparer<Position>
    {

        public int Compare(Position? x, Position? y)
        {
            return x.Y - y.Y;
        }
    }

    class PositionComparerYRev : IComparer<Position>
    {

        public int Compare(Position? x, Position? y)
        {
            return y.Y - x.Y;
        }
    }


    private static void Move(List<Position> dishes, HashSet<Position> rocks, (int, int) movement)
    {
        var maxX = Math.Max(dishes.MaxBy(p => p.X).X, rocks.MaxBy(p => p.X).X);
        var maxY = Math.Max(dishes.MaxBy(p => p.Y).Y, rocks.MaxBy(p => p.Y).Y);

        // Sort dishes according to direction.
        if (movement == (-1, 0))
        {
            var positionComparer = new PositionComparerX();
            dishes.Sort(positionComparer);
        }
        if (movement == (1, 0))
        {
            var positionComparer = new PositionComparerXRev();
            dishes.Sort(positionComparer);
        }
        if (movement == (0, -1))
        {
            var positionComparer = new PositionComparerY();
            dishes.Sort(positionComparer);
        }
        if (movement == (0, 1))
        {
            var positionComparer = new PositionComparerYRev();
            dishes.Sort(positionComparer);
        }

        bool moved = true;
        while (moved)
        {
            moved = false;
            var tmp = new HashSet<Position>();
            foreach (var dish in dishes)
            {
                var n = dish + movement;
                if (rocks.Contains(n))
                {
                    tmp.Add(dish);
                    continue;
                }
                if (tmp.Contains(n))
                {
                    tmp.Add(dish);
                    continue;
                }
                if (n.Y < 0 || n.Y > maxY || n.X < 0 || n.X > maxX)
                {
                    tmp.Add(dish);
                    continue;
                }
                tmp.Add(n);
                moved = true;
            }

            dishes.Clear();
            foreach (var position in tmp)
            {
                dishes.Add(position);
            }
            if (movement == (-1, 0))
            {
                var positionComparer = new PositionComparerX();
                dishes.Sort(positionComparer);
            }
            if (movement == (1, 0))
            {
                var positionComparer = new PositionComparerXRev();
                dishes.Sort(positionComparer);
            }
            if (movement == (0, -1))
            {
                var positionComparer = new PositionComparerY();
                dishes.Sort(positionComparer);
            }
            if (movement == (0, 1))
            {
                var positionComparer = new PositionComparerYRev();
                dishes.Sort(positionComparer);
            }
        }
    }

    public static void Part2()
    {
        var dishes = new List<Position>();
        var rocks = new HashSet<Position>();

        var lines = File.ReadAllLines("14.txt");
        for (var row = 0; row < lines.Length; row++)
        {
            for (var col = 0; col < lines[row].Length; col++)
            {
                switch (lines[row][col])
                {
                    case 'O':
                        dishes.Add(new(col, row));
                        break;
                    case '#':
                        rocks.Add(new(col, row));
                        break;
                }
            }
        }

        // Render(dishes, rocks);
        // Console.Out.WriteLine("");
        var count = 1_000_000_000;
        var now = DateTime.Now;

        List<(int, int)> cycle = [(0, -1), (-1, 0), (0, 1), (1, 0)];
        // List<(int, int)> cycle = [(0, -1), (-1, 0)];

        // value is pos, score
        var seen = new Dictionary<string, (int, int)>();
        var rs = RenderString(dishes, rocks);
        seen.Add(rs, (0, Score(dishes, rocks)));

        var cycleLength = 0;
        var cycleStart = 0;
        var cycleScore = 0;

        for (int i = 0; i < count; i++)
        {
            // if (i % (count / 10) == 0)
            // {
            //     Console.Out.WriteLine(i);
            //     Console.Out.WriteLine(DateTime.Now - now);
            // }

            // Console.Out.WriteLine($"\n\n---- Cycle {i}");
            foreach (var direction in cycle)
            {
                // Console.Out.WriteLine($"* Spin {direction}");
                Move(dishes, rocks, direction);
                // Render(dishes, rocks);
                // Console.Out.WriteLine("");
            }
            var renderString = RenderString(dishes, rocks);
            if (seen.TryGetValue(renderString, out var posScore))
            {
                // Console.Out.WriteLine($"Loop after i={i} cycles, at position {pos}");
                cycleLength = i - posScore.Item1;
                cycleStart = posScore.Item1;
                cycleScore = posScore.Item2;
                // Console.Out.WriteLine($"\nAfter {i + 1}");
                // Render(dishes, rocks);
                break;
            }
            var sc = Score(dishes, rocks);
            seen.Add(renderString, (i, sc));
            // Console.Out.WriteLine($"\nAfter {i + 1}, stored as {i}");
            // Render(dishes, rocks);
            // Console.Out.WriteLine(sc);
        }
        // var result = Score(dishes, rocks);
        // Console.Out.WriteLine(result);
        Console.Out.WriteLine(DateTime.Now - now);

        var s = 0;
        for (int i = cycleStart + 1; i < count; i += cycleLength)
        {
            s = i;
        }
        var target = count - s + cycleStart;
        // Console.Out.WriteLine(s);
        // Console.Out.WriteLine(target);

        foreach (var pair in seen)
        {
            if (pair.Value.Item1 == target)
            {
                Console.Out.WriteLine(pair.Value.Item2);
            }
        }
    }
}
