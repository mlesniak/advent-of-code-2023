namespace Lesniak.AoC2023;

public class Day16
{
    public static void Part1()
    {
        var mirrors = new Dictionary<Position, char>();
        var lines = File.ReadAllLines("16.txt");
        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                if (c == '.')
                {
                    continue;
                }
                
                mirrors.Add(new Position(x, y), line[x]); 
            }
        }
        
        foreach (var keyValuePair in mirrors)
        {
            Console.WriteLine(keyValuePair);    
        }
    }

    public static void Part2()
    {
       
    }
}
