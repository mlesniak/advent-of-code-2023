namespace Lesniak.AoC2023;

public class Day11
{
    public static void Part1()
    {
        var lines = File.ReadAllLines("11.txt");

        var galaxies = new HashSet<Position>();
        for (var y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '#')
                {
                    galaxies.Add(new(x, y));
                } 
            } 
        }

        var rows = new HashSet<int>();
        for (var y = 0; y < lines.Length; y++)
        {
            if (lines[y].All(c => c == '.'))
            {
                rows.Add(y);
            }
        }
        
        var cols = new HashSet<int>();
        for (var x = 0; x < lines[0].Length; x++)
        {
            var ok = true;
            foreach (var line in lines)
            {
                if (line[x] != '.')
                {
                    ok = false;
                    break;
                }
            }
            if (ok)
            {
                cols.Add(x);
            }
        }

        Console.WriteLine("");
    }
}
