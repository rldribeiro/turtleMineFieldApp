namespace TurtleMineField.Core.Exceptions;

[Serializable]
internal class InvalidActionException : Exception
{
    public InvalidActionException() { }
    public InvalidActionException(string message) : base(message) { }
    public InvalidActionException(string message, Exception inner) : base(message, inner) { }
    protected InvalidActionException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}