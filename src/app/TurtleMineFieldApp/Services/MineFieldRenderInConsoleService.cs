using TurtleMineField.App.Exceptions;
using TurtleMineField.Core.Entities;
using TurtleMineField.Core.Entities.Cells;

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
    private const string Portal = "H";
    private const string Empty = "-";

    public void RenderMineField(IMineField field, ITurtle turtle)
    {
        Console.WriteLine();
        for (int y = 0; y < field.Height; y++)
        {
            for (int x = 0; x < field.Width; x++)
            {
                var coord = new Coordinate(x, y);
                var cell = field.GetCell(coord);

                RenderCellAndTurtle(cell, turtle, coord);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public void RenderMovingResult(int sequenceCount)
    {
        RenderActionResult("Still moving, still safe...", ConsoleColor.White, sequenceCount);
    }

    public void RenderLostResult()
    {
        RenderActionResult("Actions ended and your turtle is still lost in the field...", ConsoleColor.Yellow, 0);
    }

    public void RenderMineHitResult(int sequenceCount)
    {
        RenderActionResult("KABUMMM!!! Your turtle hit a mine!", ConsoleColor.Red, sequenceCount);
    }

    public void RenderOutOfFieldResult(int sequenceCount)
    {
        RenderActionResult("Ooppsss!!! Your turtle left the field, never to return!", ConsoleColor.Red, sequenceCount);
    }

    public void RenderSuccessResult(int sequenceCount)
    {
        RenderActionResult("FREEDOM!!! Your turtle found the exit!", ConsoleColor.Cyan, sequenceCount);
    }


    public void RenderPrompt()
    {
        var previousColor = Console.ForegroundColor;
        Console.WriteLine("------------------------------");
        Console.WriteLine("Enter a letter for an action:");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("[M]ove ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("[R]otate ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[Q]uit ");

        Console.ForegroundColor = previousColor;
        Console.WriteLine();
        Console.Write(">");
    }

    public void RefreshRender()
    {
        Console.CursorVisible = false;
        Console.SetCursorPosition(0,0);
    }

    private void RenderActionResult(string message, ConsoleColor color, int sequenceCount)
    {
        if(sequenceCount > 0)
            Console.Write($"Sequence {sequenceCount}:");

        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = previousColor;
    }

    private void RenderCellAndTurtle(ICell cell, ITurtle turtle, Coordinate coord)
    {
        var previousColor = Console.ForegroundColor;
        var isOccupiedCell = turtle.CurrentCoordinate.Equals(coord);

        var color = previousColor;
        var graphic = Empty;

        if (cell is MineCell && isOccupiedCell)
        {
            color = ConsoleColor.Red;
            graphic = Mine;
        }
        else if (cell is MineCell)
        {
            color = ConsoleColor.DarkGray;
            graphic = Mine;
        }
        else if (cell is ExitCell)
        {
            color = ConsoleColor.Yellow;
            graphic = Exit;
        }
        else if (isOccupiedCell)
        {
            color = ConsoleColor.Magenta;
            graphic = RenderTurtle(turtle.CurrentDirection);
        }
        else if (cell is PortalCell)
        {
            color = ConsoleColor.Cyan;
            graphic = Portal;
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