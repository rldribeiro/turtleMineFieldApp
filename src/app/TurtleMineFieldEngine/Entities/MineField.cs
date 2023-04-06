using System.Drawing;
using TurtleMineField.Core.Exceptions;

namespace TurtleMineField.Core.Entities;

internal sealed class MineField : IMineField
{
    private readonly Coordinate _exitCoordinate;
    private Cell[,] _cells;

    public MineField(int width, int height, Coordinate exitCoordinate)
    {
        if (width <= 0 || height <= 0)
            throw new InvalidMineFieldException("A field with dimensions valued zero or less is not possible.");

        if (width > 1000 || height > 1000)
            throw new InvalidMineFieldException("A field has a size limit of 1000 x 1000");

        Width = width;
        Height = height;

        _cells = new Cell[height, width];
        _exitCoordinate = exitCoordinate;
        IsActive = true;
        CoordinatesWithConsequence = new SortedSet<Coordinate>();

        InitializeCells();
    }

    public int Width { get; }
    public int Height { get; }
    public SortedSet<Coordinate> CoordinatesWithConsequence { get; }
    public bool IsActive { get; private set; }

    /// <summary>
    /// Scatters mines through the minefield from a list of coordinates.
    /// If placing mine outside of field, throws exception.
    /// If placing mine in exit coordinate, mine is ignored.
    /// </summary>
    /// <param name="mineCoordinates"></param>
    /// <exception cref="CoordinateOutOfBoundsException"></exception>
    public void ScatterMines(List<Coordinate> mineCoordinates)
    {
        Coordinate currentCoordinate = Coordinate.Origin;
        try
        {
            foreach (var mineCoordinate in mineCoordinates)
            {
                if (mineCoordinate.Equals(_exitCoordinate))
                    continue;

                currentCoordinate = mineCoordinate;
                GetCell(mineCoordinate).Type = CellType.Mine;
                CoordinatesWithConsequence.Add(mineCoordinate);
            }
        }
        catch (IndexOutOfRangeException)
        {
            throw new CoordinateOutOfBoundsException(
                $"Setting mine to coordinate out of bounds: x = {currentCoordinate.X}; y = {currentCoordinate.Y}; Field size:  Width = {Width}; Height = {Height}");
        }
    }

    /// <summary>
    /// Scatters mines through the minefield in unique random positions.
    /// It does not overflows the field.
    /// It does not override the exit coordinate.
    /// </summary>
    /// <param name="numberOfMines"></param>
    public void ScatterRandomMines(int numberOfMines)
    {
        if (numberOfMines < 0)
            numberOfMines = 0;
        else if (numberOfMines >= Width * Height)
            numberOfMines = Width * Height - 1;

        for (int i = 0; i < numberOfMines; i++)
        {
            var uniqueMineCoordinate = GenerateUniqueMineCoordinate(Height, Width);
            GetCell(uniqueMineCoordinate).Type = CellType.Mine;
            CoordinatesWithConsequence.Add(uniqueMineCoordinate);
        }
    }

    /// <summary>
    /// Move to a cell and mark it as visited
    /// </summary>
    /// <param name="coordinate"></param>
    /// <returns>The visited Cell</returns>
    /// <exception cref="InvalidMineFieldException"></exception>
    /// <exception cref="CoordinateOutOfBoundsException"></exception>
    public Cell VisitCell(Coordinate coordinate)
    {
        if (!IsActive)
            throw new InvalidMineFieldException("The accessed mine field was not active.");

        try
        {
            var visitedCell = GetCell(coordinate);
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

    /// <summary>
    /// Gets a cell without marking as visited.
    /// Has no in game consequence.
    /// </summary>
    /// <param name="coordinate"></param>
    /// <returns>Cell</returns>
    public Cell GetCell(Coordinate coordinate)
    {
        return _cells[coordinate.Y, coordinate.X];
    }

    private void InitializeCells()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                // Invert row and col to correctly map to X and Y
                _cells[j, i] = new Cell();
            }
        }

        try
        {
            GetCell(_exitCoordinate).Type = CellType.Exit;
            CoordinatesWithConsequence.Add(_exitCoordinate);
        }
        catch
        {
            throw new CoordinateOutOfBoundsException(
                $"Setting exit to coordinate out of bounds: x = {_exitCoordinate.X}; y = {_exitCoordinate.Y}; Field size:  Width = {Width}; Height = {Height}");
        }
    }

    private Coordinate GenerateUniqueMineCoordinate(int height, int width)
    {
        var found = false;
        Coordinate coor;
        do
        {
            var rand = new Random();
            var y = rand.Next(height);
            var x = rand.Next(width);
            coor = new Coordinate(x, y);

            if (!CoordinatesWithConsequence.Contains(coor) && !coor.Equals(_exitCoordinate))
            {
                found = true;
            }
        } while (!found);

        return coor;
    }
}