﻿namespace TurtleMineField.Core.Entities.Cells;

public sealed class OutOfFieldCell : ICell, IConsequence<IMineField>
{
    public bool WasVisited { get; set; }

    public void ActUpon(IMineField field)
    {
        field.IsActive = false;
    }
}