using NSubstitute;
using TurtleMineField.App.Configuration;
using TurtleMineField.App.Services;
using TurtleMineField.Core.Controller;
using TurtleMineField.Core.Entities;
using TurtleMineField.Core.Entities.Cells;

namespace TurtleMineField.UnitTests;

[TestClass]
public class AppTests
{
    [TestMethod]
    public void WhenRunningSequenceShouldCallDependentServices()
    {
        var controller = Substitute.For<ITurtleMineFieldGameController>();
        var turtle = Substitute.For<ITurtle>();
        var response = new TurtleActionResult(new MineField(1,1, Coordinate.Origin), new EmptyCell(), true, turtle);
        controller.RunAction(Arg.Any<TurtleActionRequest>()).ReturnsForAnyArgs(response);

        var settings = new GameSettings
        {
            FieldHeight = 5,
            FieldWidth = 5,
            RandomMines = true,
            NumberOfMines = 3,
            ExitCoordinate = new Coordinate(4, 4),
            RenderField = true
        };

        var actionList = new List<TurtleActionRequest>
        {
            new(ActionType.Move, 3),
            new(ActionType.Rotate, 3),
            new(ActionType.Move, 2),
            new(ActionType.Rotate, 2),
            new(ActionType.Move, 1),
            new(ActionType.Rotate, 1),
        };
        var parsingService = Substitute.For<IActionParsingService>();
        parsingService.ParseActions(Arg.Any<string>()).ReturnsForAnyArgs(actionList);

        var readingService = Substitute.For<IInputReadingService>();
        var renderService = Substitute.For<IMineFieldRenderService>();

        var sut = new App.App(controller, parsingService, renderService, readingService, settings);

        sut.RunSequence("sequence");

        parsingService.Received(1).ParseActions(Arg.Any<string>());
        controller.Received(6).RunAction(Arg.Any<TurtleActionRequest>(), true);
        renderService.Received(6).RenderMineField(Arg.Any<MineField>(), Arg.Any<ITurtle>());
    }
}