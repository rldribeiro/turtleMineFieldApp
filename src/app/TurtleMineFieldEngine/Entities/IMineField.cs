namespace TurtleMineField.Core.Entities;

public interface IMineField
{
    int Width { get; }
    int Height { get; }

    SortedSet<Coordinate> CoordinatesWithConsequence { get; }

    /// <summary>
    /// Returns true if Mine Field is still active, meaning that a mine or the exit has not been visited.
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// Visits and returns a Cell at a given coordinate.
    /// </summary>
    /// <param name="coordinate"></param>
    /// <returns>Cell</returns>
    Cell VisitCell(Coordinate coordinate);

    /// <summary>
    /// Returns a cell of the field at a given X, Y coordinate
    /// </summary>
    Cell GetCell(Coordinate coordinate);
}