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

    public void Move(int steps)
    {
        var coordUpdate = GenerateCoordinateUpdate(steps);
        CurrentCoordinate = new Coordinate(CurrentCoordinate.X + coordUpdate.X, CurrentCoordinate.Y + coordUpdate.Y);
    }

    public void MoveTo(Coordinate coordinate)
    {
        CurrentCoordinate = coordinate;
    }

    private Coordinate GenerateCoordinateUpdate(int steps)
    {
        switch (CurrentDirection)
        {
            case Direction.North:
                return new Coordinate(0, -steps);
            case Direction.South:
                return new Coordinate(0, steps);
            case Direction.East:
                return new Coordinate(steps, 0);
            case Direction.West:
                return new Coordinate(-steps, 0);
            default:
                return Coordinate.Origin;
        }
    }
}