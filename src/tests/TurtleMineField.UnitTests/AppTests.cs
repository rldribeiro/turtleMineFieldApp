using NSubstitute;
using TurtleMineField.App.Configuration;
using TurtleMineField.App.Services;
using TurtleMineField.Core.Engine;
using TurtleMineField.Core.Entities;

namespace TurtleMineField.UnitTests;

[TestClass]
public class AppTests
{
    [TestMethod]
    public void WhenRunningSequenceShouldCallDependentServices()
    {
        var controller = Substitute.For<ITurtleMineFieldGameController>();
        var turtle = Substitute.For<ITurtle>();
        var response = new TurtleActionResult(new Cell[1, 1], new Cell(), true, turtle);
        controller.RunAction(Arg.Any<TurtleActionRequest>()).ReturnsForAnyArgs(response);

        var settings = new GameSettings
        {
            FieldHeight = 5,
            FieldWidth = 5,
            RandomMines = true,
            NumberOfMines = 3,
            ExitCoordinate = new Coordinate(4, 4)
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
        controller.Received(6).RunAction(Arg.Any<TurtleActionRequest>());
        renderService.Received(6).RenderMineField(Arg.Any<Cell[,]>(), Arg.Any<ITurtle>());
    }
}