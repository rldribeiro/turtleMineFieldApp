using TurtleMineField.Core.Entities;

namespace TurtleMineField.Core.Configuration;

public interface ITurtleSettings
{
    Coordinate InitCoordinate { get;  }
    char InitDirection { get;  }
}

public interface IMineFieldSettings
{
    int FieldWidth { get; }
    int FieldHeight { get; }
    Coordinate ExitCoordinate { get; }
    bool RandomMines { get; }
    int NumberOfMines { get; }
    Coordinate[] MineCoordinates { get; }
    Coordinate[][] PortalCoordinates { get; }
}