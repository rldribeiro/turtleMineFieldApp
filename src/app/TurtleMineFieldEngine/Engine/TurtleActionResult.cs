using TurtleMineField.Core.Entities;

namespace TurtleMineField.Core.Engine;

public class TurtleActionResult
{
    /// <summary>
    /// The result of the action being performed by a turtle on a field.
    /// It represents the state of the field, the cell being visited, the state of the field and the state of the turtle.
    /// </summary>
    /// <param name="fieldCells"></param>
    /// <param name="visitedCell"></param>
    /// <param name="isFieldActive"></param>
    /// <param name="actionType"></param>
    /// <param name="turtle"></param>
    public TurtleActionResult(IMineField field, Cell visitedCell, bool isFieldActive, ITurtle turtle)
    {
        Field = field;
        VisitedCell = visitedCell;
        IsFieldActive = isFieldActive;
        Turtle = turtle;
    }

    public IMineField Field { get; }
    public Cell VisitedCell { get; }
    public bool IsFieldActive { get; }
    public ITurtle Turtle { get; }
}