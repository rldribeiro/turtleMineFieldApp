namespace TurtleMineField.Core.Factories;

internal interface IGameComponentFactory<out TComponent, in TSettings>
{
    TComponent Create(TSettings settings);
}