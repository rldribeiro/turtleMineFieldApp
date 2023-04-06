using TurtleMineField.Core.Configuration;
using TurtleMineField.Core.Entities;

namespace TurtleMineField.App.Configuration;

internal class GameSettings : ITurtleSettings, IMineFieldSettings
{
    public bool RenderField { get; set; }

    // IMineFieldSettings implementation
    public int FieldWidth { get; set; }
    public int FieldHeight { get; set; }
    public Coordinate ExitCoordinate { get; set; } = Coordinate.Origin;
    public bool RandomMines { get; set; }
    public int NumberOfMines { get; set; }
    public Coordinate[] MineCoordinates { get; set; } = { };

    // ITurtleSettings implementation
    public Coordinate InitCoordinate { get; set; } = Coordinate.Origin;
    public char InitDirection { get; set; }
}