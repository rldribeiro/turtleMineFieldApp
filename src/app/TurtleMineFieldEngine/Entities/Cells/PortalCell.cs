namespace TurtleMineField.Core.Entities.Cells;

public sealed class PortalCell : ICell, IConsequence<ITurtle>
{
    private readonly Coordinate _destCoordinate;

    public PortalCell(Coordinate destCoordinate)
    {
        _destCoordinate = destCoordinate;
    }

    public bool WasVisited { get; set; }


    public void ActUpon(ITurtle turtle)
    {
        turtle.MoveTo(_destCoordinate);
    }
}