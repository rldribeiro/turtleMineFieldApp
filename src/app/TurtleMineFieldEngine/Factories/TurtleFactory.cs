using TurtleMineField.Core.Configuration;
using TurtleMineField.Core.Entities;
using TurtleMineField.Core.Exceptions;

namespace TurtleMineField.Core.Factories;

internal sealed class TurtleFactory : IGameComponentFactory<ITurtle, ITurtleSettings>
{
    private readonly List<char> _allowedDirections = new() { 'N', 'S', 'E', 'W' };
    public ITurtle Create(ITurtleSettings settings)
    {
        if (!_allowedDirections.Contains(settings.InitDirection))
            throw new InvalidDirectionException("Invalid direction in game settings");

        Direction initDirection = Direction.North;
        switch (settings.InitDirection)
        {
            case 'S':
                initDirection = Direction.South;
                break;
            case 'E':
                initDirection = Direction.East;
                break;
            case 'W':
                initDirection = Direction.West;
                break;
        }

        return new Turtle(settings.InitCoordinate, initDirection);
    }
}