using TurtleMineField.Core.Entities.Cells;

namespace TurtleMineField.Core.Entities;

public interface IMineField
{
    int Width { get; }
    int Height { get; }

    SortedSet<Coordinate> CoordinatesWithConsequence { get; }

    /// <summary>
    /// Visits and returns a Cell at a given coordinate.
    /// </summary>
    /// <param name="coordinate"></param>
    /// <returns>Cell</returns>
    ICell VisitCell(Coordinate coordinate);

    /// <summary>
    /// Returns a cell of the field at a given coordinate
    /// </summary>
    ICell GetCell(Coordinate coordinate);
}