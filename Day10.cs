namespace Lesniak.AoC2023;

public class Day10
{
    public static void Part1()
    {
        var lines = File.ReadAllLines("10.txt");
        var map = new Dictionary<(int, int), char>();
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                map[(x, y)] = lines[y][x];
            }
        }

        var startPos = map.First(pair => pair.Value == 'S').Key;
        // Console.WriteLine(startPos);

        var pipes = "-|LF7J";
        foreach (var pipe in pipes)
        {
            map[startPos] = pipe;
            if (ValidMap(map, startPos))
            {
                // Console.WriteLine($"Potential start found for char {pipe}");
                var (steps, _) = ValidLoop(map, startPos);
                if (steps != -1)
                {
                    // Console.WriteLine($"Valid loop for {pipe}: steps={steps}, maxDist={steps / 2}");
                    Console.WriteLine(steps / 2);
                }
            }
        }

        // A value is valid, if it leads to the starting position again
        // while following the pipes.
    }

    private static (int, HashSet<(int, int)>) ValidLoop(Dictionary<(int, int), char> map, (int, int) startPos)
    {
        var cur = startPos;
        var visited = new HashSet<(int, int)>();
        visited.Add(cur);
        // Console.WriteLine(cur);

        var steps = 1;
        while (true)
        {
            // Determine new cur.
            var deltas = new List<(int, int)> {(-1, 0), (1, 0), (0, -1), (0, 1)};

            bool found = false;
            foreach (var delta in deltas)
            {
                var next = (cur.Item1 + delta.Item1, cur.Item2 + delta.Item2);
                if (steps > 2 && next == startPos)
                {
                    // We are back at the starting position.
                    return (steps, visited);
                }
                if (!map.ContainsKey(cur) || !map.ContainsKey(next))
                {
                    continue;
                }
                if (!visited.Contains(next) && ValidMove(delta, map[cur], map[next]))
                {
                    cur = next;
                    steps++;
                    // Console.WriteLine(cur);
                    visited.Add(cur);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                // No valid non-visited move found at all.
                return (-1, visited);
            }
        }
    }

    private static bool ValidMove((int, int) move, char from, char to)
    {
        var x = move.Item1;
        var y = move.Item2;
        switch (from)
        {
            case '-':
                if (x == -1)
                {
                    return "-FL".Contains(to);
                }
                if (x == 1)
                {
                    return "-J7".Contains(to);
                }
                return false;
            case '|':
                if (y == -1)
                {
                    return "|F7".Contains(to);
                }
                if (y == 1)
                {
                    return "|LJ".Contains(to);
                }
                return false;
            case 'L':
                if (x == 1)
                {
                    return "-7J".Contains(to);
                }
                if (y == -1)
                {
                    return "|F7".Contains(to);
                }
                return false;
            case 'F':
                if (x == 1)
                {
                    return "-7J".Contains(to);
                }
                if (y == 1)
                {
                    return "|LJ".Contains(to);
                }
                return false;
            case 'J':
                if (x == -1)
                {
                    return "-LF".Contains(to);
                }
                if (y == -1)
                {
                    return "|F7".Contains(to);
                }
                return false;
            case '7':
                if (x == -1)
                {
                    return "-LF".Contains(to);
                }
                if (y == 1)
                {
                    return "|LJ".Contains(to);
                }
                return false;
        }
        return false;
    }

    private static bool ValidMap(Dictionary<(int, int), char> map, (int, int) startPos)
    {
        var cur = map[startPos];
        var (x, y) = startPos;
        switch (cur)
        {
            case '-':
                return OnMap(map, (x - 1, y), "-FL") && OnMap(map, (x + 1, y), "-J7");
            case '|':
                // Might be reversed?
                return OnMap(map, (x, y - 1), "|F7") && OnMap(map, (x, y + 1), "|LJ");
            case 'L':
                return OnMap(map, (x + 1, y), "-7J") && OnMap(map, (x, y - 1), "|F7");
            case 'F':
                return OnMap(map, (x + 1, y), "-7J") && OnMap(map, (x, y + 1), "|LJ");
            case 'J':
                return OnMap(map, (x - 1, y), "-LF") && OnMap(map, (x, y - 1), "|F7");
            case '7':
                return OnMap(map, (x - 1, y), "-LF") && OnMap(map, (x, y + 1), "|LJ");
        }

        return true;
    }

    private static bool OnMap(Dictionary<(int, int), char> map, (int, int y) pos, string allowed)
    {
        if (map.TryGetValue((pos), out char c))
        {
            return allowed.Contains(c);
        }

        return false;
    }

    // Idea: double spacing. 
    // Create a new map with just the path tiles, but double spacing.
    // Flood fill.
    // Count at all the remaining, which have not been flooded, but divide by 2*2.
    public static void Part2()
    {
        var lines = File.ReadAllLines("10.txt");
        var map = new Dictionary<(int, int), char>();
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                map[(x, y)] = lines[y][x];
            }
        }

        var startPos = map.First(pair => pair.Value == 'S').Key;
        var pipes = "-|LF7J";
        HashSet<(int, int)> path = null;
        foreach (var pipe in pipes)
        {
            map[startPos] = pipe;
            if (ValidMap(map, startPos))
            {
                var (steps, p) = ValidLoop(map, startPos);
                if (steps != -1)
                {
                    path = p;
                    break;
                }
            }
        }

        var maxWidth = lines[0].Length;
        var maxHeight = lines.Length;
        var floodMap = new Dictionary<(int, int), char>();
        for (int row = 0; row < maxHeight; row++)
        {
            for (int col = 0; col < maxWidth; col++)
            {
                floodMap[(col, row)] = '.';
            }
        }

        var s = string.Join(", ", path.ToList());
        Console.WriteLine(s);
        foreach (var pair in path)
        {
            floodMap[pair] = map[pair];
        }

        // Flood fill via BFS.
        var queue = new Queue<(int, int)>();
        var start = (0, 0);
        queue.Enqueue(start);
        floodMap[start] = '#';

        var visited = new HashSet<(int, int)>();
        while (queue.Any())
        {
            var cur = queue.Dequeue();
            visited.Add(cur);

            var deltas = new List<(int, int)> {(-1, 0), (1, 0), (0, -1), (0, 1)};
            foreach (var delta in deltas)
            {
                var p = (cur.Item1 + delta.Item1, cur.Item2 + delta.Item2);
                if (floodMap.TryGetValue(p, out char c))
                {
                    if (c == '.' && !visited.Contains(p))
                    {
                        queue.Enqueue(p);
                        floodMap[p] = '#';
                    }
                }
            }
        }

        // Debugging.
        for (int row = 0; row < maxHeight; row++)
        {
            for (int col = 0; col < maxWidth; col++)
            {
                var c = floodMap[(col, row)];
                Console.Write(c);
            }
            Console.WriteLine();
        }
    }
}
