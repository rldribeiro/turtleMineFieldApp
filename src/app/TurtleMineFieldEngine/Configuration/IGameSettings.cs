using TurtleMineField.Core.Entities;

namespace TurtleMineField.Core.Configuration;

public interface ITurtleSettings
{
    Coordinate InitCoordinate { get; set; }
    char InitDirection { get; set; }
}

public interface IMineFieldSettings
{
    int FieldWidth { get; set; }
    int FieldHeight { get; set; }
    Coordinate ExitCoordinate { get; set; }
    bool RandomMines { get; set; }
    int NumberOfMines { get; set; }
    Coordinate[] MineCoordinates { get; set; }
}