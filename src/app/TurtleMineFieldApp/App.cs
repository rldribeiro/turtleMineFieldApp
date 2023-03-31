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

    public void Run(string actionSequence)
    {
        var actions = _parsingService.ParseTurtleActions(actionSequence);

        for (var i = 0; i < actions.Count; i++)
        {
            var sequenceCount = i + 1;
            var action = actions[i];

            var response = _turtleMineFieldController.RunAction(action);

            if (_renderBoard)
                _renderService.RenderMineField(response.FieldState, response.Turtle);

            if (!response.IsFieldActive && response.VisitedCell.Type == CellType.Mine)
            {
                _renderService.RenderMineHitResult(sequenceCount);
                return;
            }

            if (!response.IsFieldActive && response.VisitedCell.Type == CellType.Exit)
            {
                _renderService.RenderSuccessResult(sequenceCount);
                return;
            }

            if (i == actions.Count - 1)
            {
                _renderService.RenderLostResult(sequenceCount);
                return;
            }

            _renderService.RenderMovingResult(sequenceCount);
        }
    }
}