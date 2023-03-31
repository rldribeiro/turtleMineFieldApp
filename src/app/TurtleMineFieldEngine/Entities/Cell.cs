namespace TurtleMineField.Core.Entities;

public sealed class Cell
{
    public CellType Type { get; set; }
    public bool WasVisited { get; set; }
}

public enum CellType
{
    Empty,
    Mine,
    Exit
}