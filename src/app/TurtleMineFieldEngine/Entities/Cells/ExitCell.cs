namespace TurtleMineField.Core.Entities.Cells;

public sealed class ExitCell : ICell, IConsequence<ITurtle>
{
    public bool WasVisited { get; set; }

    public void ActUpon(ITurtle turtle)
    {
        turtle.IsActive = false;
    }
}