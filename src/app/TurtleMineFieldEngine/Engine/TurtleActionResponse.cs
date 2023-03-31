using TurtleMineField.Core.Entities;

namespace TurtleMineField.Core.Engine;

public class TurtleActionResponse
{
    public TurtleActionResponse(Cell[,] fieldState, Cell visitedCell, bool isFieldActive, ActionType actionType, ITurtle turtle)
    {
        FieldState = fieldState;
        VisitedCell = visitedCell;
        IsFieldActive = isFieldActive;
        ActionType = actionType;
        Turtle = turtle;
    }

    public Cell[,] FieldState { get; }
    public Cell VisitedCell { get; }
    public bool IsFieldActive { get; }
    public ActionType ActionType { get; }
    public ITurtle Turtle { get; }
}