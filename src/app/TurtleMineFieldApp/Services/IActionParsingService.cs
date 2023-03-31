using TurtleMineField.App.Exceptions;
using TurtleMineField.Core.Engine;

namespace TurtleMineField.App.Services;

public interface IActionParsingService
{
    /// <summary>
    /// Gets a sequence of action chars (r and m) and transforms them into a ordered collection of Actions.
    /// Repeated chars are translated into the number os turns for the respective action.
    /// </summary>
    /// <param name="actionSequence">A string composed of 'r', 'm' and white spaces.</param>
    /// <returns>An ordered list of Actions</returns>
    /// <exception cref="InvalidInputException"></exception>
    List<TurtleActionRequest> ParseTurtleActions(string actionSequence);
}