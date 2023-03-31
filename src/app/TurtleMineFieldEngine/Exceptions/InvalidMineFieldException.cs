namespace TurtleMineField.Core.Exceptions;

[Serializable]
internal class InvalidMineFieldException : Exception
{
    public InvalidMineFieldException() { }
    public InvalidMineFieldException(string message) : base(message) { }
    public InvalidMineFieldException(string message, Exception inner) : base(message, inner) { }
    protected InvalidMineFieldException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}