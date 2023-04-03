namespace TurtleMineField.Core.Entities;

internal sealed class Turtle : ITurtle
{
    public Turtle(Coordinate coordinate, Direction direction)
    {
        CurrentCoordinate = coordinate;
        CurrentDirection = direction;
    }

    public Direction CurrentDirection { get; private set; }

    public Coordinate CurrentCoordinate { get; private set; }

    public void Rotate90(int turns, bool clockwise)
    {
        int directionMultiplier = clockwise ? 1 : -1;
        CurrentDirection = (Direction)(((int)CurrentDirection + turns * directionMultiplier) % 4);
    }

    public void Move()
    {
        var coordUpdate = GenerateCoordinateUpdate();
        CurrentCoordinate = new Coordinate(CurrentCoordinate.X + coordUpdate.X, CurrentCoordinate.Y + coordUpdate.Y);
    }

    private Coordinate GenerateCoordinateUpdate()
    {
        if (CurrentDirection.Equals(Direction.North))
            return new Coordinate(0, -1);
        if (CurrentDirection.Equals(Direction.South))
            return new Coordinate(0, 1);
        if (CurrentDirection.Equals(Direction.East))
            return new Coordinate(1, 0);
        if (CurrentDirection.Equals(Direction.West))
            return new Coordinate(-1, 0);

        return Coordinate.Origin;
    }
}