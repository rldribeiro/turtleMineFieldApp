using System;
using TurtleMineField.Core.Configuration;
using TurtleMineField.Core.Entities;
using TurtleMineField.Core.Factories;

namespace TurtleMineField.Core.Engine;

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

    public TurtleActionResponse RunAction(TurtleActionRequest action)
    {
        Cell currentCell = _mineField.VisitCell(_turtle.CurrentCoordinate);

        if (currentCell.Type != CellType.Mine && currentCell.Type != CellType.Exit)
        {
            switch (action.Type)
            {
                case ActionType.Move:
                    currentCell = MoveAndEvaluate(action.Turns);
                    break;
                case ActionType.Rotate:
                    _turtle.Rotate90(action.Turns);
                    break;
            }
        }

        return new TurtleActionResponse(_mineField.Cells, currentCell, _mineField.IsActive, action.Type, _turtle);
    }

    private Cell MoveAndEvaluate(int actionTurns)
    {
        var currentCell = _mineField.VisitCell(_turtle.CurrentCoordinate);
        for (int i = 0; i < actionTurns; i++)
        {
            _turtle.Move();
            currentCell = _mineField.VisitCell(_turtle.CurrentCoordinate);
            if (currentCell.Type == CellType.Mine || currentCell.Type == CellType.Exit)
                break;
        }

        return currentCell;
    }
}