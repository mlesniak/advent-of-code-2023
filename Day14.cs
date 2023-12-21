using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.CompilerServices;

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
        var count = 1_000_000;
        var now = DateTime.Now;
        
        List<(int, int)> cycle = [(0, -1), (-1, 0), (0, 1), (1, 0)];
        // List<(int, int)> cycle = [(0, -1), (-1, 0)];
        
        for (int i = 0; i < count; i++)
        {
            if (i % (count / 10) == 0)
            {
                Console.Out.WriteLine(i);
            }
            
            // Console.Out.WriteLine($"\n\n---- Cycle {i}");
            foreach (var direction in cycle)
            {
                // Console.Out.WriteLine($"* Spin {direction}");
                Move(dishes, rocks, direction);
                // Render(dishes, rocks);
                // Console.Out.WriteLine("");
            }
            // Console.Out.WriteLine($"\nAfter {i + 1}");
            // Render(dishes, rocks);
        }
        var result = Score(dishes, rocks);
        Console.Out.WriteLine(result);
        var dur = DateTime.Now - now;
        Console.Out.WriteLine(dur);
    }
}
