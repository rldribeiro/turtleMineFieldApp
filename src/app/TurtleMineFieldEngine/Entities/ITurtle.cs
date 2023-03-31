namespace TurtleMineField.Core.Entities;

public interface ITurtle
{
    Direction CurrentDirection { get; }

    Coordinate CurrentCoordinate { get; }

    void Rotate90(int turns = 1, bool clockwise = true);

    void Move();
}