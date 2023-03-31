using Newtonsoft.Json;
using TurtleMineField.App.Configuration;
using TurtleMineField.App.Exceptions;

namespace TurtleMineField.App;

internal static class SettingsParser
{
    /// <summary>
    /// Reads a filepath for a JSON file and parses it to GameSettings class, composed of both
    /// ITurtleSettings and IMineFieldSettings
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="InvalidInputException"></exception>
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