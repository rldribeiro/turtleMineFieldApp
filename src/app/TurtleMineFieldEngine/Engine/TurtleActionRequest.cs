namespace TurtleMineField.Core.Engine;

public class TurtleActionRequest
{
    public TurtleActionRequest(ActionType type, int turns)
    {
        Type = type;
        Turns = turns;
    }

    public int Turns { get; }

    public ActionType Type { get; }
}

public enum ActionType
{
    None,
    Move,
    Rotate
}