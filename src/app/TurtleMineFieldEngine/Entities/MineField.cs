using TurtleMineField.Core.Entities.Cells;
using TurtleMineField.Core.Exceptions;

namespace TurtleMineField.Core.Entities;

internal sealed class MineField : IMineField
{
    private readonly Coordinate _exitCoordinate;
    private readonly ICell[,] _cells;

    public MineField(int width, int height, Coordinate exitCoordinate)
    {
        if (width <= 0 || height <= 0)
            throw new InvalidMineFieldException("A field with dimensions valued zero or less is not possible.");

        if (width > 1000 || height > 1000)
            throw new InvalidMineFieldException("A field has a size limit of 1000 x 1000");

        Width = width;
        Height = height;

        _cells = new ICell[height, width];
        _exitCoordinate = exitCoordinate;
        CoordinatesWithConsequence = new SortedSet<Coordinate>();

        InitializeCells();
    }

    public int Width { get; }
    public int Height { get; }
    public SortedSet<Coordinate> CoordinatesWithConsequence { get; }

    /// <summary>
    /// Scatters mines through the minefield from a list of coordinates.
    /// If placing mine outside of field, throws exception.
    /// If placing mine in exit coordinate, mine is ignored.
    /// </summary>
    /// <param name="mineCoordinates"></param>
    /// <exception cref="CoordinateOutOfBoundsException"></exception>
    public void ScatterMines(List<Coordinate> mineCoordinates)
    {
        foreach (var mineCoordinate in mineCoordinates)
        {
            if (mineCoordinate.Equals(_exitCoordinate))
                continue;

            ReplaceCell(mineCoordinate, new MineCell());
            CoordinatesWithConsequence.Add(mineCoordinate);
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
            ReplaceCell(uniqueMineCoordinate, new MineCell());
            CoordinatesWithConsequence.Add(uniqueMineCoordinate);
        }
    }

    public void ScatterPortals(Coordinate[][] portalCoordinates)
    {
        foreach (var portalCoordinate in portalCoordinates)
        {
            // Each portal has the other coordinate as destination
            // Hence creating pairs of portals interconnected
            var coordinate1 = portalCoordinate[0];
            var coordinate2 = portalCoordinate[1];

            var portal1 = new PortalCell(coordinate2);
            var portal2 = new PortalCell(coordinate1);

            ReplaceCell(coordinate1, portal1);
            ReplaceCell(coordinate2, portal2);

            CoordinatesWithConsequence.Add(coordinate1);
            CoordinatesWithConsequence.Add(coordinate2);
        }
    }

    /// <summary>
    /// Move to a cell and mark it as visited
    /// </summary>
    /// <param name="coordinate"></param>
    /// <returns>The visited Cell</returns>
    /// <exception cref="InvalidMineFieldException"></exception>
    /// <exception cref="CoordinateOutOfBoundsException"></exception>
    public ICell VisitCell(Coordinate coordinate)
    {
        var visitedCell = GetCell(coordinate);
        visitedCell.WasVisited = true;
        return visitedCell;
    }

    /// <summary>
    /// Gets a cell without marking as visited.
    /// Has no in game consequence.
    /// </summary>
    /// <param name="coordinate"></param>
    /// <returns>Cell</returns>
    public ICell GetCell(Coordinate coordinate)
    {
        try
        {
            return _cells[coordinate.Y, coordinate.X];
        }
        catch
        {
            throw new CoordinateOutOfBoundsException(
                $"Trying to get cell with coordinate out of bounds: x = {coordinate.X}; y = {coordinate.Y}; Field size:  Width = {Width}; Height = {Height}");
        }
    }

    private void InitializeCells()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                // Invert row and col to correctly map to X and Y
                _cells[j, i] = new EmptyCell();
            }
        }

        ReplaceCell(_exitCoordinate, new ExitCell());
        CoordinatesWithConsequence.Add(_exitCoordinate);
    }

    private void ReplaceCell(Coordinate coordinate, ICell newCell)
    {
        try
        {
            _cells[coordinate.Y, coordinate.X] = newCell;
        }
        catch
        {
            throw new CoordinateOutOfBoundsException(
                $"Setting cell to coordinate out of bounds: x = {_exitCoordinate.X}; y = {_exitCoordinate.Y}; Field size:  Width = {Width}; Height = {Height}");
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