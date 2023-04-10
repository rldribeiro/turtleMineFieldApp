using TurtleMineField.App.Configuration;
using TurtleMineField.App.Services;
using TurtleMineField.Core.Controller;
using TurtleMineField.Core.Entities.Cells;

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
            var result = _turtleMineFieldController.RunAction(action, _renderField);

            //if (_renderField)
            _renderService.RenderMineField(result.Field, result.Turtle);

            if (CheckIfGameEnded(result, actionCount))
                return;
        }
        // We ran out of actions with the turtle still active
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
            count++;
            _renderService.RefreshRender();
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

    private bool CheckIfGameEnded(TurtleActionResult result, int sequenceCount)
    {
        // Check Mine hit
        if (!result.Turtle.IsActive)
        {
            switch (result.VisitedCell)
            {
                case ExitCell:
                    _renderService.RenderSuccessResult(sequenceCount);
                    break;
                case MineCell:
                    _renderService.RenderMineHitResult(sequenceCount);
                    break;
                case OutOfFieldCell:
                    _renderService.RenderOutOfFieldResult(sequenceCount);
                    break;
            }
            return true;
        }

        // No endgame scenario reached
        // Action processing continues
        _renderService.RenderMovingResult(sequenceCount);
        return false;
    }
}