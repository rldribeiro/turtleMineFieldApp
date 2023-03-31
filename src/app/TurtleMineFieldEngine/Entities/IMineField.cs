namespace TurtleMineField.Core.Entities;

public interface IMineField
{
    public Cell[,] Cells { get; }

    /// <summary>
    /// Returns true if Mine Field is still active, meaning that a mine or the exit has not been visited.
    /// </summary>
    public bool IsActive { get; }


    /// <summary>
    /// Visits and returns a Cell at a given coordinate.
    /// </summary>
    /// <param name="coordinate"></param>
    /// <returns>Cell</returns>
    public Cell VisitCell(Coordinate coordinate);
}