namespace TurtleMineField.Core.Entities;

public readonly struct Coordinate : IComparable<Coordinate>
{
    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }

    public static Coordinate Origin => new Coordinate(0, 0);

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Coordinate other = (Coordinate)obj;

        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        int hash = 24;
        hash = hash * 42 + X.GetHashCode();
        hash = hash * 42 + Y.GetHashCode();
        return hash;
    }

    public int CompareTo(Coordinate other)
    {
        var xComparison = X.CompareTo(other.X);
        if (xComparison != 0) return xComparison;
        return Y.CompareTo(other.Y);
    }
}