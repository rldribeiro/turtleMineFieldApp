using TurtleMineField.Core.Entities;

namespace TurtleMineField.App.Services;

public interface IMineFieldRenderService
{
    void RenderMineField(Cell[,] cells, ITurtle turtle);

    void RenderMovingResult(int sequenceCount);

    void RenderLostResult(int sequenceCount);

    void RenderSuccessResult(int sequenceCount);

    void RenderMineHitResult(int sequenceCount);
}