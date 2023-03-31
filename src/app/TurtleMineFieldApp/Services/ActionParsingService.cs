using System.Text.RegularExpressions;
using TurtleMineField.App.Exceptions;
using TurtleMineField.Core.Engine;

namespace TurtleMineField.App.Services;

internal sealed class ActionParsingService : IActionParsingService
{
    public List<TurtleActionRequest> ParseTurtleActions(string actionSequence)
    {
        if (string.IsNullOrEmpty(actionSequence))
            throw new InvalidInputException("Action sequence was null or empty");

        var trimmedSequence = Regex.Replace(actionSequence, @"\s+", string.Empty);

        var actions = new List<TurtleActionRequest>();

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
                actions.Add(new TurtleActionRequest(ParseAction(currentType), currentTurns));
                currentType = trimmedSequence[i];
                currentTurns = 1;
            }
        }

        actions.Add(new TurtleActionRequest(ParseAction(currentType), currentTurns));
        return actions;
    }

    private ActionType ParseAction(char currentAction)
    {
        switch (currentAction)
        {
            case 'm':
                return ActionType.Move;
            case 'r':
                return ActionType.Rotate;
            default:
                throw new InvalidInputException($"Found invalid char '{currentAction}' in sequence. Only 'm' and 'r' are valid.");

        }
    }
}