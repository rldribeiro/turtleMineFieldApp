namespace TurtleMineField.Core.Exceptions;

[Serializable]
internal class InactiveMineFieldException : Exception
{
    public InactiveMineFieldException() { }
    public InactiveMineFieldException(string message) : base(message) { }
    public InactiveMineFieldException(string message, Exception inner) : base(message, inner) { }
    protected InactiveMineFieldException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}