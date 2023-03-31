namespace TurtleMineField.Core.Engine;

public interface ITurtleMineFieldGameController
{
    TurtleActionResponse RunAction(TurtleActionRequest action);
}