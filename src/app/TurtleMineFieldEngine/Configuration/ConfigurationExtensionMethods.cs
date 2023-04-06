using Microsoft.Extensions.DependencyInjection;
using TurtleMineField.Core.Controller;
using TurtleMineField.Core.Entities;
using TurtleMineField.Core.Factories;

namespace TurtleMineField.Core.Configuration;

public static class ConfigurationExtensionMethods
{
    public static void RegisterTurtleMineFieldCoreServices(
        this IServiceCollection services)
    {
        services.AddSingleton<ITurtleMineFieldGameController, TurtleMineFieldGameController>();
        services.AddSingleton<ITurtle, Turtle>();
        services.AddSingleton<IMineField, MineField>();
        services.AddSingleton<IGameComponentFactory<ITurtle, ITurtleSettings>, TurtleFactory>();
        services.AddSingleton<IGameComponentFactory<IMineField, IMineFieldSettings>, MineFieldFactory>();
    }
}