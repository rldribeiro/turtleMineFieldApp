using TurtleMineField.App.Exceptions;
using TurtleMineField.Core.Entities;

namespace TurtleMineField.App.Services;

internal sealed class MineFieldRenderInConsoleService : IMineFieldRenderService
{
    private const string UpArrow = "^";
    private const string DownArrow = "v";
    private const string LeftArrow = "<";
    private const string RightArrow = ">";
    private const string Mine = "#";
    private const string Exit = "O";
    private const string Visited = "*";
    private const string Empty = "-";

    public void RenderMineField(Cell[,] cells, ITurtle turtle)
    {
        var width = cells.GetLength(0);
        var height = cells.GetLength(1);

        Console.WriteLine();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var coord = new Coordinate(i, j);
                var cell = cells[i, j];

                RenderCellAndTurtle(cell, turtle, coord);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private void RenderCellAndTurtle(Cell cell, ITurtle turtle, Coordinate coord)
    {
        var previousColor = Console.ForegroundColor;
        var isOccupiedCell = turtle.CurrentCoordinate.Equals(coord);


        var color = previousColor;
        var graphic = Empty;
        if (cell.Type == CellType.Mine && isOccupiedCell)
        {
            color = ConsoleColor.Red;
            graphic = Mine;
        }
        else if (cell.Type == CellType.Mine)
        {
            color = ConsoleColor.DarkGray;
            graphic = Mine;
        }
        else if (cell.Type == CellType.Exit)
        {
            color = ConsoleColor.Yellow;
            graphic = Exit;
        }
        else if (isOccupiedCell)
        {
            color = ConsoleColor.Magenta;
            graphic = RenderTurtle(turtle.CurrentDirection);
        }
        else if (cell.WasVisited)
        {
            color = ConsoleColor.Cyan;
            graphic = Visited;
        }

        Console.ForegroundColor = color;
        Console.Write(graphic);
        Console.ForegroundColor = previousColor;
    }

    public void RenderMovingResult(int sequenceCount)
    {
        RenderActionResult("Still moving, still safe...", ConsoleColor.White, sequenceCount);
    }

    public void RenderMineHitResult(int sequenceCount)
    {
        RenderActionResult("KABUMMM!!! Your turtle hit a mine!", ConsoleColor.Red, sequenceCount);
    }

    public void RenderLostResult(int sequenceCount)
    {
        RenderActionResult("Your turtle is still lost in the field...", ConsoleColor.Yellow, sequenceCount);
    }

    public void RenderSuccessResult(int sequenceCount)
    {
        RenderActionResult("FREEDOM!!! Your turtle found the exit!", ConsoleColor.Cyan, sequenceCount);
    }

    private void RenderActionResult(string message, ConsoleColor color, int sequenceCount)
    {
        Console.Write($"Sequence {sequenceCount}:");
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = previousColor;
    }

    private string RenderTurtle(Direction turtleDirection)
    {
        switch (turtleDirection)
        {
            case Direction.North:
                return UpArrow;
            case Direction.South:
                return DownArrow;
            case Direction.East:
                return RightArrow;
            case Direction.West:
                return LeftArrow;
            default:
                throw new InvalidInputException("Invalid direction while rendering");
        }
    }
}