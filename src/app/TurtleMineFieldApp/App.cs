using System.Reflection.Metadata;
using TurtleMineField.App.Services;
using TurtleMineField.Core.Configuration;
using TurtleMineField.Core.Engine;
using TurtleMineField.Core.Entities;

namespace TurtleMineField.App;

internal sealed class App
{
    private readonly ITurtleMineFieldGameController _turtleMineFieldController;
    private readonly IActionParsingService _parsingService;
    private readonly IMineFieldRenderService _renderService;
    private readonly IInputReadingService _inputReadingService;
    private readonly bool _renderBoard;

    public App(ITurtleMineFieldGameController turtleMineFieldController,
        IActionParsingService parsingService,
        IMineFieldRenderService renderService,
        IInputReadingService inputReadingService,
        IMineFieldSettings settings)
    {
        _turtleMineFieldController = turtleMineFieldController;
        _parsingService = parsingService;
        _renderService = renderService;
        _inputReadingService = inputReadingService;
        _renderBoard = settings.FieldWidth <= 100 && settings.FieldHeight <= 100;
    }

    /// <summary>
    /// Runs the game with a sequence of actions.
    /// </summary>
    /// <param name="actionSequence"></param>
    public void RunSequence(string actionSequence)
    {
        var actions = _parsingService.ParseActions(actionSequence);
        var actionCount = 0;

        foreach (var action in actions)
        {
            actionCount++;
            var response = _turtleMineFieldController.RunAction(action);

            if (_renderBoard)
                _renderService.RenderMineField(response.FieldCells, response.Turtle);

            if (CheckIfGameEnded(response, actionCount))
                return;
        }
        _renderService.RenderLostResult();
    }

    public void RunInteractive()
    {
        var fieldState = _turtleMineFieldController.RunAction(new TurtleActionRequest(ActionType.None, 0));
        var acceptableInput = new[] { 'r', 'm', 'q' };
        var count = 0;
        var gameRunning = true;
        while (gameRunning)
        {
            _renderService.RefreshRender();
            count++;
            _renderService.RenderMineField(fieldState.FieldCells, fieldState.Turtle);
            _renderService.RenderPrompt();

            var actionChar = _inputReadingService.ReadUserInput(acceptableInput);
            var actionType = _parsingService.ParseActionType(actionChar);

            switch (actionType)
            {
                case ActionType.Quit:
                    return;
                case ActionType.Rotate:
                case ActionType.Move:
                    fieldState = _turtleMineFieldController.RunAction(new TurtleActionRequest(actionType, 1));
                    break;
            }
            gameRunning = !CheckIfGameEnded(fieldState, count);
        }
    }

    private bool CheckIfGameEnded(TurtleActionResult response, int sequenceCount)
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

        // No endgame scenario reached
        // Action processing continues
        _renderService.RenderMovingResult(sequenceCount);
        return false;
    }
}