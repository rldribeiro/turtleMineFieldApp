using FluentAssertions;
using TurtleMineField.App.Configuration;
using TurtleMineField.Core.Entities;
using TurtleMineField.Core.Exceptions;
using TurtleMineField.Core.Factories;

namespace TurtleMineField.UnitTests;

[TestClass]
public class MineFieldFactoryTests
{
    [TestMethod]
    public void WhenCallingCreateShouldReturnField()
    {
        var settings = new GameSettings
        {
            FieldHeight = 5,
            FieldWidth = 5,
            RandomMines = true,
            NumberOfMines = 3,
            ExitCoordinate = new Coordinate(4, 4)
        };

        var sut = new MineFieldFactory();

        var mineField = sut.Create(settings);
        mineField.Should().NotBeNull();
    }

    [TestMethod]
    public void WhenRandomShouldIgnoreMineCoordinates()
    {
        var settings = new GameSettings
        {
            FieldHeight = 5,
            FieldWidth = 5,
            RandomMines = true,
            NumberOfMines = 1,
            MineCoordinates = new[]
            {
                new Coordinate(1, 1),
                new Coordinate(2, 2)
            },
            ExitCoordinate = new Coordinate(4, 4)
        };

        var sut = new MineFieldFactory();
        var mineField = sut.Create(settings);

        mineField.Cells.OfType<Cell>().Count(mineFieldCell => mineFieldCell.Type == CellType.Mine)
            .Should().Be(1);
    }

    [TestMethod]
    public void WhenNotRandomShouldIgnoreNumberOfMines()
    {
        var settings = new GameSettings
        {
            FieldHeight = 5,
            FieldWidth = 5,
            RandomMines = false,
            NumberOfMines = 50,
            MineCoordinates = new[]
            {
                new Coordinate(1, 1),
                new Coordinate(2, 2)
            },
            ExitCoordinate = new Coordinate(4, 4)
        };

        var sut = new MineFieldFactory();
        var mineField = sut.Create(settings);

        mineField.Cells.OfType<Cell>().Count(mineFieldCell => mineFieldCell.Type == CellType.Mine)
            .Should().Be(2);
    }

    [TestMethod]
    public void WhenSizeIsUnderLimitShouldThrowException()
    {
        var settings = new GameSettings
        {
            FieldHeight = -1,
            FieldWidth = -1,
            RandomMines = true,
            NumberOfMines = 1,
            ExitCoordinate = new Coordinate(4, 4)
        };

        var sut = new MineFieldFactory();

        sut.Invoking(y => y.Create(settings))
            .Should().Throw<InvalidMineFieldException>()
            .WithMessage("A field with dimensions valued zero or less is not possible.");
    }

    [TestMethod]
    public void WhenSizeIsAboveLimitShouldThrowException()
    {
        var settings = new GameSettings
        {
            FieldHeight = 1001,
            FieldWidth = 1001,
            RandomMines = true,
            NumberOfMines = 1,
            ExitCoordinate = new Coordinate(4, 4)
        };

        var sut = new MineFieldFactory();

        sut.Invoking(y => y.Create(settings))
            .Should().Throw<InvalidMineFieldException>()
            .WithMessage("A field has a size limit of 1000 x 1000");
    }
}