namespace TurtleMineField.Core.Engine;

public interface ITurtleMineFieldGameController
{
    TurtleActionResult RunAction(TurtleActionRequest action);
}