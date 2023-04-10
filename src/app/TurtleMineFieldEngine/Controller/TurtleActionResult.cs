using TurtleMineField.Core.Entities;
using TurtleMineField.Core.Entities.Cells;

namespace TurtleMineField.Core.Controller;

public class TurtleActionResult
{
    /// <summary>
    /// The result of the action being performed by a turtle on a field.
    /// It represents the state of the field, the cell being visited, the state of the field and the state of the turtle.
    /// </summary>
    /// <param name="field"></param>
    /// <param name="visitedCell"></param>
    /// <param name="turtle"></param>
    public TurtleActionResult(IMineField field, ICell visitedCell, ITurtle turtle)
    {
        Field = field;
        VisitedCell = visitedCell;
        Turtle = turtle;
    }

    public IMineField Field { get; }
    public ICell VisitedCell { get; }
    public ITurtle Turtle { get; }
}