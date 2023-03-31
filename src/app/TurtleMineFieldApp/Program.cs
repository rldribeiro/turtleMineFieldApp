using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TurtleMineField.App;
using TurtleMineField.App.Exceptions;
using TurtleMineField.App.Services;
using TurtleMineField.Core.Configuration;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        var settings = SettingsParser.Parse(args[0]);

        services.AddScoped<App>();

        services.AddSingleton<ITurtleSettings>(_ => settings);
        services.AddSingleton<IMineFieldSettings>(_ => settings);

        services.AddSingleton<IMineFieldRenderService, MineFieldRenderInConsoleService>();
        services.AddSingleton<IActionParsingService, ActionParsingService>();

        services.RegisterTurtleMineFieldCoreServices();
    })
    .Build();

try
{
    if (args.Length != 2)
        throw new InvalidInputException("Not enough arguments. Usage:\nTurtleMineFieldApp.exe <path-to-settings-file> <path-to-actions-file>");

    var actionsFilePath = args[1];
    if (!File.Exists(actionsFilePath))
        throw new InvalidInputException("Actions file not found");

    var actionSequence = File.ReadAllText(actionsFilePath);

    var app = host.Services.GetRequiredService<App>();
    app.Run(actionSequence);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
