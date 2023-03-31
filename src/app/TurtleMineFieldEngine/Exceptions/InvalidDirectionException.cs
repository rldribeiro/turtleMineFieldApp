namespace TurtleMineField.Core.Exceptions;

[Serializable]
internal class InvalidDirectionException : Exception
{
    public InvalidDirectionException() { }
    public InvalidDirectionException(string message) : base(message) { }
    public InvalidDirectionException(string message, Exception inner) : base(message, inner) { }
    protected InvalidDirectionException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}