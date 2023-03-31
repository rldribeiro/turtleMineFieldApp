namespace TurtleMineField.Core.Entities;

public interface IMineField
{
    public Cell[,] Cells { get; }

    public int Width { get; }
    public int Height { get; }

    public bool IsActive { get; }

    public void ScatterMines(List<Coordinate> mineCoordinates);

    public void ScatterRandomMines(int numberOfMines);

    public Cell VisitCell(Coordinate coordinate);
}