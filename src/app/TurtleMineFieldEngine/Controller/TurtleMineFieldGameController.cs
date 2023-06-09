﻿using TurtleMineField.Core.Configuration;
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

        switch (action.Type)
        {
            case ActionType.Move:
                currentCell = MoveAndEvaluate(action.Turns, visitAllCells);
                break;
            case ActionType.Rotate:
                _turtle.Rotate90(action.Turns);
                break;
        }

        return new TurtleActionResult(_mineField, currentCell, _turtle);
    }

    private ICell MoveAndEvaluate(int actionTurns, bool visitAllCells)
    {
        try
        {
            return visitAllCells ? MoveIteratively(actionTurns) : MoveDirectly(actionTurns);
        }
        catch (CoordinateOutOfBoundsException)
        {
            var outCell = new OutOfFieldCell();
            outCell.ActUpon(_turtle);
            return outCell;
        }
    }

    private ICell MoveDirectly(int actionTurns)
    {
        // Move turtle to determine end coordinate
        var startCoordinate = _turtle.CurrentCoordinate;
        var endCoordinate = _turtle.Move(actionTurns);

        // Check consequences in turtle's path
        var largeX = Math.Max(startCoordinate.X, endCoordinate.X);
        var smallX = Math.Min(startCoordinate.X, endCoordinate.X);
        var largeY = Math.Max(startCoordinate.Y, endCoordinate.Y);
        var smallY = Math.Min(startCoordinate.Y, endCoordinate.Y);

        var consequencesInPath = _mineField.CoordinatesWithConsequence.Where(c =>
        {
            if (_turtle.CurrentDirection.Equals(Direction.North) || _turtle.CurrentDirection.Equals(Direction.South))
                return c.X == startCoordinate.X && c.Y > smallY && c.Y < largeY;

            return c.Y == startCoordinate.Y && c.X > smallX && c.X < largeX;
        }).ToList();

        if (consequencesInPath.Any())
        {
            endCoordinate = consequencesInPath.FirstOrDefault();
            _turtle.MoveTo(endCoordinate);
        }

        var visitedCell = _mineField.VisitCell(_turtle.CurrentCoordinate);
        HandleConsequences(visitedCell);

        // Determining remaining moves: already done turns minus turns took to reach consequence cell
        var remainingMoves = actionTurns - startCoordinate.DetermineLinearDistanceTo(endCoordinate);
        if (_turtle.IsActive && remainingMoves > 0)
            return MoveDirectly(remainingMoves);

        // Turtle is inactive or has no more moves
        return visitedCell;
    }

    private ICell MoveIteratively(int actionTurns)
    {
        var currentCell = _mineField.VisitCell(_turtle.CurrentCoordinate);
        for (int i = 0; i < actionTurns; i++)
        {
            var currCoordinate = _turtle.Move();
            currentCell = _mineField.VisitCell(currCoordinate);

            HandleConsequences(currentCell);

            if (!_turtle.IsActive)
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