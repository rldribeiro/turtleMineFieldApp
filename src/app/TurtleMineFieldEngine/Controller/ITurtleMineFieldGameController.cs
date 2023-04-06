namespace TurtleMineField.Core.Controller;

public interface ITurtleMineFieldGameController
{
    /// <summary>
    /// Runs one action composed of a movement or a rotation.
    /// </summary>
    /// <param name="action">The action request</param>
    /// <param name="visitAllCells">Indicates if the action requires all cells in movement path to be marked as visited</param>
    /// <returns>The result of the action</returns>
    TurtleActionResult RunAction(TurtleActionRequest action, bool visitAllCells = true);
}