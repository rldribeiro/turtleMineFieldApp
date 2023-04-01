using FluentAssertions;
using TurtleMineField.Core.Entities;

namespace TurtleMineField.UnitTests;

[TestClass]
public class MineFieldTests
{
    [DataTestMethod]
    [DataRow(100)]
    [DataRow(101)]
    [DataRow(99)]
    [DataRow(200)]
    public void WhenScatteringRandomMinesShouldTrimMinesCount(int mineInputCount)
    {
        var sut = new MineField(10,10,Coordinate.Origin);

        sut.ScatterRandomMines(mineInputCount);

        // Number of mines should be maxed to total cell count - 1 for exit coordinate.
        sut.Cells.OfType<Cell>().Count(mineFieldCell => mineFieldCell.Type == CellType.Mine)
            .Should().Be(99);
    }
}