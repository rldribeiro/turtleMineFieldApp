using FluentAssertions;
using TurtleMineField.App.Configuration;
using TurtleMineField.Core.Entities;
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
}