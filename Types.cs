namespace Lesniak.AoC2023;

public record Position(int X, int Y)
{
    public static Position operator +(Position a, Position b)
    {
        return new Position(a.X + b.X, a.Y + b.Y);
    }
    
    public static Position operator +(Position a, (int, int) tuple)
    {
        return new Position(a.X + tuple.Item1, a.Y + tuple.Item2);
    }

}


// Won't use generics since I'd have to adapt existing code
// from previous days and I'm too lazy.
record PositionLong(long X, long Y);

