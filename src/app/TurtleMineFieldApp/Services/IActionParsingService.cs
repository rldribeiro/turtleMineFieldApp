﻿using TurtleMineField.App.Exceptions;
using TurtleMineField.Core.Controller;

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
    IEnumerable<TurtleActionRequest> ParseActions(string actionSequence);

    /// <summary>
    /// Gets an action char and parses into action type
    /// </summary>
    /// <param name="currentAction"></param>
    /// <returns></returns>
    ActionType ParseActionType(char currentAction);
}