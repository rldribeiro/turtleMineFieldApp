using FluentAssertions;
using TurtleMineField.App.Exceptions;
using TurtleMineField.App.Services;
using TurtleMineField.Core.Engine;

namespace TurtleMineField.UnitTests;

[TestClass]
public class ActionParsingServiceTests
{
    [TestMethod]
    public void WhenParsingValidSequenceShouldReturnValidActions()
    {
        var sut = new ActionParsingService();
        var sequence = "mmmrrrmmrrmr";
        var expectedActionList = new List<TurtleActionRequest>
        {
            new(ActionType.Move, 3),
            new(ActionType.Rotate, 3),
            new(ActionType.Move, 2),
            new(ActionType.Rotate, 2),
            new(ActionType.Move, 1),
            new(ActionType.Rotate, 1),
        };

        var resultActionList = sut.ParseActions(sequence);

        resultActionList.Should().BeEquivalentTo(expectedActionList);
    }

    [TestMethod]
    public void WhenParsingValidSequenceWithSpacesShouldReturnValidActions()
    {
        var sut = new ActionParsingService();

        var sequence = "mmm rrr mm r r m r";
        var expectedActionList = new List<TurtleActionRequest>
        {
            new(ActionType.Move, 3),
            new(ActionType.Rotate, 3),
            new(ActionType.Move, 2),
            new(ActionType.Rotate, 2),
            new(ActionType.Move, 1),
            new(ActionType.Rotate, 1),
        };

        var resultActionList = sut.ParseActions(sequence);

        resultActionList.Should().BeEquivalentTo(expectedActionList);
    }

    [TestMethod]
    public void WhenParsingInvalidSequenceShouldThrowException()
    {
        var sut = new ActionParsingService();

        var sequence = "mmm a rrr mm r r m r";

        sut.Invoking(y =>
            {
                foreach (var action in y.ParseActions(sequence))
                {
                    action.Should().NotBeNull();
                }
            })
            .Should().Throw<InvalidInputException>()
            .WithMessage("Found invalid char 'a' in sequence. Only 'm' and 'r' are valid.");
    }
}