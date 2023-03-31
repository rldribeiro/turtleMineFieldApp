using TurtleMineField.Core.Exceptions;

namespace TurtleMineField.Core.Entities;

internal sealed class MineField : IMineField
{
    private readonly Coordinate _exitCoordinate;

    public MineField(int width, int height, Coordinate exitCoordinate)
    {
        if (width <= 0 || height <= 0)
            throw new InvalidMineFieldException("A field with dimensions valued zero or less is not possible.");

        _exitCoordinate = exitCoordinate;
        Cells = new Cell[width, height];
        Width = width;
        Height = height;

        InitializeField();
    }

    public Cell[,] Cells { get; }
    public int Width { get; }
    public int Height { get; }
    public bool IsActive { get; private set; }


    public void ScatterMines(List<Coordinate> mineCoordinates)
    {
        Coordinate currentCoordinate = Coordinate.Origin;
        try
        {
            foreach (var mineCoordinate in mineCoordinates)
            {
                currentCoordinate = mineCoordinate;
                Cells[mineCoordinate.X, mineCoordinate.Y].Type = CellType.Mine;
            }
        }
        catch
        {
            throw new CoordinateOutOfBoundsException(
                $"Setting mine to coordinate out of bounds: x = {currentCoordinate.X}; y = {currentCoordinate.Y}; Field size:  Width = {Width}; Height = {Height}");
        }
    }

    public void ScatterRandomMines(int numberOfMines)
    {
        if (numberOfMines < 0)
            numberOfMines = 0;
        else if (numberOfMines > Width * Height)
            numberOfMines = Width * Height - 1;

        var mineCoordinates = new HashSet<(int x, int y)>();
        for (int i = 0; i < numberOfMines; i++)
        {
            var uniqueMineCoordinate = GenerateUniqueMineCoordinate(Height, Width, mineCoordinates);
            Cells[uniqueMineCoordinate.x, uniqueMineCoordinate.y].Type = CellType.Mine;
        }
    }

    public Cell VisitCell(Coordinate coordinate)
    {
        if (!IsActive)
            throw new InvalidMineFieldException("The accessed mine field was not active.");

        try
        {
            var visitedCell = Cells[coordinate.X, coordinate.Y];
            if (visitedCell.Type.Equals(CellType.Mine) || visitedCell.Type.Equals(CellType.Exit))
                IsActive = false;

            visitedCell.WasVisited = true;
            return visitedCell;
        }
        catch
        {
            throw new CoordinateOutOfBoundsException(
                $"Visiting cell with coordinate out of bounds: x = {coordinate.X}; y = {coordinate.Y}; Field size:  Width = {Width}; Height = {Height}");
        }
    }

    private void InitializeField()
    {
        IsActive = true;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Cells[i, j] = new Cell();
            }
        }

        try
        {
            Cells[_exitCoordinate.X, _exitCoordinate.Y].Type = CellType.Exit;
        }
        catch
        {
            throw new CoordinateOutOfBoundsException(
                $"Setting exit to coordinate out of bounds: x = {_exitCoordinate.X}; y = {_exitCoordinate.Y}; Field size:  Width = {Width}; Height = {Height}");
        }
    }

    private (int x, int y) GenerateUniqueMineCoordinate(int height, int width, HashSet<(int x, int y)> mineCoordinates)
    {
        var found = false;
        int x;
        int y;
        do
        {
            var rand = new Random();
            y = rand.Next(height);
            x = rand.Next(width);

            if (!mineCoordinates.Contains((x, y)) && (x, y) != (_exitCoordinate.X, _exitCoordinate.Y))
            {
                mineCoordinates.Add((x, y));
                found = true;
            }
        } while (!found);

        return (x, y);
    }
}