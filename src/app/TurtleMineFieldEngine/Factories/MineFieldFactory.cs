using TurtleMineField.Core.Configuration;
using TurtleMineField.Core.Entities;

namespace TurtleMineField.Core.Factories;

internal sealed class MineFieldFactory : IGameComponentFactory<IMineField, IMineFieldSettings>
{
    public IMineField Create(IMineFieldSettings settings)
    {
        var mineField = new MineField(settings.FieldWidth, settings.FieldHeight, settings.ExitCoordinate);

        if (settings.RandomMines)
        {
            mineField.ScatterRandomMines(settings.NumberOfMines);
        }
        else
        {
            mineField.ScatterMines(settings.MineCoordinates.ToList());
        }

        return mineField;
    }
}