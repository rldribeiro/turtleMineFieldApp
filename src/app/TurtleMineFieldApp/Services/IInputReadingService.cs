namespace TurtleMineField.App.Services;

public interface IInputReadingService
{
    char ReadUserInput(char[] acceptableChars);
}

internal sealed class KeyboardInputReadingService : IInputReadingService
{
    public char ReadUserInput(char[] acceptableChars)
    {
        char key;
        var keyAccepted = false;
        do
        {
            key = Console.ReadKey().KeyChar;
            if (acceptableChars.Contains(key))
                keyAccepted = true;

            // Clear the previously read character
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            Console.Write(" ");
            Console.SetCursorPosition(1, Console.CursorTop);
        } while (!keyAccepted);

        return key;
    }
}