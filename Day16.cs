using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

namespace Lesniak.AoC2023;

public enum Direction
{
    North,
    East,
    South,
    West
}

public record Beam(Position Pos, Direction Direction)
{
    public Position MoveAlongDirection()
    {
        switch (Direction)
        {
            case Direction.North:
                return Pos with {Y = Pos.Y - 1};
            case Direction.East:
                return Pos with {X = Pos.X + 1};
            case Direction.South:
                return Pos with {Y = Pos.Y + 1};
            case Direction.West:
                return Pos with {X = Pos.X - 1};
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public class Day16
{
    private static int maxWidth;
    private static int maxHeight;

    public static void Part1()
    {
        var mirrors = new Dictionary<Position, char>();
        var lines = File.ReadAllLines("16.txt");
        maxHeight = lines.Length - 1;
        maxWidth = lines[0].Length - 1;
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

        var beam = new Beam(new Position(0, 0), Direction.East);
        var beams = new List<Beam> {beam};
        var tiles = new HashSet<Position>();
        while (beams.Count > 0)
        {
            for (int i = beams.Count - 1; i >= 0; i--)
            {
                var curBeam = beams[i];
                beams.RemoveAt(i);
                while (curBeam != null)
                {
                    tiles.Add(curBeam.Pos);
                    curBeam = Compute(curBeam, mirrors, beams);
                }

            }
        }

        Console.WriteLine("Result");
        Console.WriteLine(tiles.Count);
        foreach (var position in tiles)
        {
            Console.WriteLine(position);
        }
    }

    // Beams allows to add new beams when splitting.
    private static Beam? Compute(Beam beam, Dictionary<Position, char> mirrors, List<Beam> beams)
    {
        var nextPos = beam.MoveAlongDirection();
        if (nextPos.X < 0 || nextPos.Y < 0 || nextPos.X > maxWidth || nextPos.Y > maxHeight)
        {
            return null;
        }

        // Check if we hit a mirror.
        if (mirrors.TryGetValue(nextPos, out var mirror))
        {
            switch (mirror)
            {
                case '/':
                    switch (beam.Direction)
                    {
                        case Direction.North:
                            return new Beam(nextPos, Direction.East);
                        case Direction.East:
                            return new Beam(nextPos, Direction.North);
                        case Direction.South:
                            return new Beam(nextPos, Direction.West);
                        case Direction.West:
                            return new Beam(nextPos, Direction.South);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case '\\':
                    switch (beam.Direction)
                    {
                        case Direction.North:
                            return new Beam(nextPos, Direction.West);
                        case Direction.East:
                            return new Beam(nextPos, Direction.South);
                        case Direction.South:
                            return new Beam(nextPos, Direction.East);
                        case Direction.West:
                            return new Beam(nextPos, Direction.North);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case '|':
                    switch (beam.Direction)
                    {
                        case Direction.North:
                        case Direction.South:
                            return beam with {Pos = nextPos};
                        case Direction.East:
                        case Direction.West:
                            beams.Add(new Beam(nextPos with {Y = nextPos.Y - 1}, Direction.North));
                            return new Beam(nextPos with {Y = nextPos.Y + 1}, Direction.South);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    case '-':
                    switch (beam.Direction)
                    {
                        case Direction.North:
                        case Direction.South:
                            beams.Add(new Beam(nextPos with {X = nextPos.X - 1}, Direction.West));
                            return new Beam(nextPos with {X = nextPos.X + 1}, Direction.East);
                        case Direction.East:
                        case Direction.West:
                            return beam with {Pos = nextPos};
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Otherwise, just move along.
        return beam with {Pos = nextPos};
    }

}
