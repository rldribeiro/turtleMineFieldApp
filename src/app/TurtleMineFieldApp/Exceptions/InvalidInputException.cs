﻿namespace TurtleMineField.App.Exceptions;

[Serializable]
public class InvalidInputException : Exception
{
    public InvalidInputException() { }
    public InvalidInputException(string message) : base(message) { }
    public InvalidInputException(string message, Exception inner) : base(message, inner) { }
    protected InvalidInputException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}