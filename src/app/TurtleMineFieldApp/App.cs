using TurtleMineField.App.Configuration;
using TurtleMineField.App.Services;
using TurtleMineField.Core.Configuration;
using TurtleMineField.Core.Controller;
using TurtleMineField.Core.Entities;

namespace TurtleMineField.App;

internal sealed class App
{
    private const int FieldSizeRenderLimit = 100;
    private readonly ITurtleMineFieldGameController _turtleMineFieldController;
    private readonly IActionParsingService _parsingService;
    private readonly IMineFieldRenderService _renderService;
    private readonly IInputReadingService _inputReadingService;
    private readonly bool _renderField;

    public App(ITurtleMineFieldGameController turtleMineFieldController,
        IActionParsingService parsingService,
        IMineFieldRenderService renderService,
        IInputReadingService inputReadingService,
        GameSettings settings)
    {
        _turtleMineFieldController = turtleMineFieldController;
        _parsingService = parsingService;
        _renderService = renderService;
        _inputReadingService = inputReadingService;

        _renderField = settings.RenderField
                       && settings.FieldWidth <= FieldSizeRenderLimit
                       && settings.FieldHeight <= FieldSizeRenderLimit;
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

            // If the field is rendered, we must visit all fields to draw the path
            var response = _turtleMineFieldController.RunAction(action, _renderField);

            if (_renderField)
                _renderService.RenderMineField(response.Field, response.Turtle);

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
            _renderService.RenderMineField(fieldState.Field, fieldState.Turtle);
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

        // Check Turtle Left the field
        if (response.VisitedCell.Type == CellType.OutOfBonds)
        {
            _renderService.RenderOutOfFieldResult(sequenceCount);
            return true;
        }

        // No endgame scenario reached
        // Action processing continues
        _renderService.RenderMovingResult(sequenceCount);
        return false;
    }
}