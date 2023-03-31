﻿using TurtleMineField.App.Services;
using TurtleMineField.Core.Configuration;
using TurtleMineField.Core.Engine;
using TurtleMineField.Core.Entities;

namespace TurtleMineField.App;

internal sealed class App
{
    private readonly ITurtleMineFieldGameController _turtleMineFieldController;
    private readonly IActionParsingService _parsingService;
    private readonly IMineFieldRenderService _renderService;
    private readonly bool _renderBoard;

    public App(ITurtleMineFieldGameController turtleMineFieldController,
        IActionParsingService parsingService,
        IMineFieldRenderService renderService,
        IMineFieldSettings settings)
    {
        _turtleMineFieldController = turtleMineFieldController;
        _parsingService = parsingService;
        _renderService = renderService;
        _renderBoard = settings.FieldWidth <= 100 && settings.FieldHeight <= 100;
    }

    /// <summary>
    /// Runs the game with a sequence of actions.
    /// </summary>
    /// <param name="actionSequence"></param>
    public void RunSequence(string actionSequence)
    {
        var actions = _parsingService.ParseTurtleActions(actionSequence);
        var actionCount = actions.Count;

        for (var i = 0; i < actionCount; i++)
        {
            var sequenceCount = i + 1;
            var action = actions[i];

            var response = _turtleMineFieldController.RunAction(action);

            if (_renderBoard)
                _renderService.RenderMineField(response.FieldCells, response.Turtle);

            if (CheckIfGameEnded(response, sequenceCount, i, actionCount))
                return;
        }
    }

    private bool CheckIfGameEnded(TurtleActionResult response, int sequenceCount, int i, int actionCount)
    {
        // Check Mine hit
        if (!response.IsFieldActive && response.VisitedCell.Type == CellType.Mine)
        {
            _renderService.RenderMineHitResult(sequenceCount);
            return true;
        }

        // Check Exit reached
        if (!response.IsFieldActive && response.VisitedCell.Type == CellType.Exit)
        {
            _renderService.RenderSuccessResult(sequenceCount);
            return true;
        }

        // Check end of actions
        if (i == actionCount - 1)
        {
            _renderService.RenderLostResult(sequenceCount);
            return true;
        }

        // No endgame scenario reached
        // Action processing continues
        _renderService.RenderMovingResult(sequenceCount);
        return false;
    }
}