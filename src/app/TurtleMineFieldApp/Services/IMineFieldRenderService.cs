using TurtleMineField.Core.Entities;

namespace TurtleMineField.App.Services;

public interface IMineFieldRenderService
{
    /// <summary>
    /// Renders the field in the terminal.
    /// Represents all elements of the field: the turtle, the path, the cells and the type of the cell
    /// </summary>
    /// <param name="cells"></param>
    /// <param name="turtle"></param>
    void RenderMineField(IMineField field, ITurtle turtle);

    /// <summary>
    /// Renders a message when the turtle ends in an empty cell, but still has actions to perform
    /// </summary>
    /// <param name="sequenceCount"></param>
    void RenderMovingResult(int sequenceCount);

    /// <summary>
    /// Renders a message when the turtle ended its movement, but haven't found a mine or the exit
    /// </summary>
    /// <param name="sequenceCount"></param>
    void RenderLostResult();

    /// <summary>
    /// Renders a message when the turtle finds the exit
    /// </summary>
    /// <param name="sequenceCount"></param>
    void RenderSuccessResult(int sequenceCount);

    /// <summary>
    /// Renders a message when the turtle hits a mine
    /// </summary>
    /// <param name="sequenceCount"></param>
    void RenderMineHitResult(int sequenceCount);

    /// <summary>
    /// Renders a message when the turtle leaves the field
    /// </summary>
    /// <param name="sequenceCount"></param>
    void RenderOutOfFieldResult(int sequenceCount);

    /// <summary>
    /// Renders a prompt to the player
    /// </summary>
    void RenderPrompt();

    void RefreshRender();
}