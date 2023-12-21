using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Lesniak.AoC2023;

public class Day14
{
    public static void Part1()
    {
        var dishes = new HashSet<Position>();
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

        Render(dishes, rocks);
        Console.Out.WriteLine("");
        Move(dishes, rocks, (0, -1));
        Render(dishes, rocks);
    }

    private static void Render(HashSet<Position> dishes, HashSet<Position> rocks)
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

    private static void Move(HashSet<Position> dishes, HashSet<Position> rocks, (int, int) movement)
    {
        var maxX = Math.Max(dishes.MaxBy(p => p.X).X, rocks.MaxBy(p => p.X).X);
        var maxY = Math.Max(dishes.MaxBy(p => p.Y).Y, rocks.MaxBy(p => p.Y).Y);

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
                if (n.Y < 0 || n.Y >= maxY || n.X < 0 || n.X > maxX)
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
        }
    }

    public static void Part2()
    {
    }
}
