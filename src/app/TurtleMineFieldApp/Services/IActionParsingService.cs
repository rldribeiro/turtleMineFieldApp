using TurtleMineField.Core.Engine;

namespace TurtleMineField.App.Services;

public interface IActionParsingService
{
    List<TurtleActionRequest> ParseTurtleActions(string actionSequence);
}