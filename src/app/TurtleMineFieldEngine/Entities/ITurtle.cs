namespace TurtleMineField.Core.Entities;

public interface ITurtle
{
    Direction CurrentDirection { get; }

    Coordinate CurrentCoordinate { get; }

    /// <summary>
    /// Rotates the turtle in 90º turns.
    /// </summary>
    /// <param name="turns">Number of 90º turns to rotate</param>
    /// <param name="clockwise">Clockwise if true. Counterclockwise if false. Default: clockwise</param>
    void Rotate90(int turns = 1, bool clockwise = true);

    /// <summary>
    /// Moves the turtle in the direction it is facing.
    /// </summary>
    /// <param name="steps">The number of steps to move</param>
    void Move(int steps = 1);

    /// <summary>
    /// Transports the turtle to the destination coordinate
    /// </summary>
    void MoveTo(Coordinate coordinate);
}