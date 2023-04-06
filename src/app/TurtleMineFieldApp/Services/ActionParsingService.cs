using System.Text.RegularExpressions;
using TurtleMineField.App.Exceptions;
using TurtleMineField.Core.Controller;

namespace TurtleMineField.App.Services;

internal sealed class ActionParsingService : IActionParsingService
{
    public IEnumerable<TurtleActionRequest> ParseActions(string actionSequence)
    {
        if (string.IsNullOrEmpty(actionSequence))
            throw new InvalidInputException("Action sequence was null or empty");

        var trimmedSequence = Regex.Replace(actionSequence, @"\s+", string.Empty);


        char currentType = trimmedSequence[0];
        int currentTurns = 1;

        for (int i = 1; i < trimmedSequence.Length; i++)
        {
            if (trimmedSequence[i] == currentType)
            {
                currentTurns++;
            }
            else
            {
                yield return new TurtleActionRequest(ParseActionType(currentType), currentTurns);
                currentType = trimmedSequence[i];
                currentTurns = 1;
            }
        }

        yield return new TurtleActionRequest(ParseActionType(currentType), currentTurns);
    }

    public ActionType ParseActionType(char currentAction)
    {
        switch (currentAction)
        {
            case 'm':
                return ActionType.Move;
            case 'r':
                return ActionType.Rotate;
            case 'q':
                return ActionType.Quit;
            default:
                throw new InvalidInputException($"Found invalid char '{currentAction}' in sequence. Only 'm' and 'r' are valid.");

        }
    }
}