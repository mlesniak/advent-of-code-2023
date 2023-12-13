namespace Lesniak.AoC2023;

record Position(int X, int Y)
{
    public static Position operator +(Position a, Position b)
    {
        return new Position(a.X + b.X, a.Y + b.Y);
    }
}


