using TurtleMineField.Core.Configuration;
using TurtleMineField.Core.Entities;
using TurtleMineField.Core.Entities.Cells;
using TurtleMineField.Core.Exceptions;
using TurtleMineField.Core.Factories;

namespace TurtleMineField.Core.Controller;

internal sealed class TurtleMineFieldGameController : ITurtleMineFieldGameController
{
    private readonly IMineField _mineField;
    private readonly ITurtle _turtle;

    public TurtleMineFieldGameController(IGameComponentFactory<ITurtle, ITurtleSettings> turtleFactory,
        IGameComponentFactory<IMineField, IMineFieldSettings> mineFieldFactory,
        ITurtleSettings turtleSettings, IMineFieldSettings mineFieldSettings)
    {
        _mineField = mineFieldFactory.Create(mineFieldSettings);
        _turtle = turtleFactory.Create(turtleSettings);
    }

    public TurtleActionResult RunAction(TurtleActionRequest action, bool visitAllCells)
    {
        // Checking if Turtle already is in consequence cell
        ICell currentCell = _mineField.VisitCell(_turtle.CurrentCoordinate);

        if (_mineField.IsActive)
        {
            switch (action.Type)
            {
                case ActionType.Move:
                    currentCell = MoveAndEvaluate(action.Turns, visitAllCells);
                    break;
                case ActionType.Rotate:
                    _turtle.Rotate90(action.Turns);
                    break;
            }
        }

        HandleConsequences(currentCell);

        return new TurtleActionResult(_mineField, currentCell, _mineField.IsActive, _turtle);
    }

    private ICell MoveAndEvaluate(int actionTurns, bool visitAllCells)
    {
        try
        {
            return visitAllCells ? MoveIteratively(actionTurns) : MoveDirectly(actionTurns);
        }
        catch (CoordinateOutOfBoundsException)
        {
            return new OutOfFieldCell();
        }
    }

    private ICell MoveDirectly(int actionTurns)
    {
        // Move turtle to determine end coordinate
        var startCoordinate = _turtle.CurrentCoordinate;
        _turtle.Move(actionTurns);
        var endCoordinate = _turtle.CurrentCoordinate;

        var largeX = Math.Max(startCoordinate.X, endCoordinate.X);
        var smallX = Math.Min(startCoordinate.X, endCoordinate.X);
        var largeY = Math.Max(startCoordinate.Y, endCoordinate.Y);
        var smallY = Math.Min(startCoordinate.Y, endCoordinate.Y);

        // Check consequences in path
        var consequencesInPath = _mineField.CoordinatesWithConsequence.Where(c =>
        {
            if(_turtle.CurrentDirection.Equals(Direction.North) || _turtle.CurrentDirection.Equals(Direction.South))
                return c.X == startCoordinate.X && c.Y >= smallY && c.Y <= largeY;

            return c.Y == startCoordinate.Y && c.X >= smallX && c.X <= largeX;
        }).ToList();

        if(consequencesInPath.Any())
            _turtle.MoveTo(consequencesInPath.FirstOrDefault());

        // Return visited cell
        var visitedCell = _mineField.VisitCell(_turtle.CurrentCoordinate);
        return visitedCell;
    }

    private ICell MoveIteratively(int actionTurns)
    {
        var currentCell = _mineField.VisitCell(_turtle.CurrentCoordinate);
        for (int i = 0; i < actionTurns; i++)
        {
            _turtle.Move();
            currentCell = _mineField.VisitCell(_turtle.CurrentCoordinate);

            HandleConsequences(currentCell);

            if (!_mineField.IsActive)
                break;
        }

        return currentCell;
    }

    private void HandleConsequences(ICell currentCell)
    {
        if (currentCell is IConsequence<IMineField> cellActingOnMine)
            cellActingOnMine.ActUpon(_mineField);

        if (currentCell is IConsequence<ITurtle> cellActingOnTurtle)
            cellActingOnTurtle.ActUpon(_turtle);
    }
}