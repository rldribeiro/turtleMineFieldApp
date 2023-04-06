namespace TurtleMineField.Core.Entities.Cells;

public interface ICell
{
    bool WasVisited { get; set; }
}

public interface IConsequence<T>
{
    void ActUpon(T actedOn);
}