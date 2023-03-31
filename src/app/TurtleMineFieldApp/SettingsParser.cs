using Newtonsoft.Json;
using TurtleMineField.App.Configuration;
using TurtleMineField.App.Exceptions;

namespace TurtleMineField.App;

internal static class SettingsParser
{
    public static GameSettings Parse(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Settings file not found at {filePath}");

        var json = File.ReadAllText(filePath);

        var settings = JsonConvert.DeserializeObject<GameSettings>(json);

        if (settings is null)
            throw new InvalidInputException("Error parsing settings json file");

        return settings;
    }
}