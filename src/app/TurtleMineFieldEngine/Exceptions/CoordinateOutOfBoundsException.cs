﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleMineField.Core.Exceptions;

[Serializable]
public class CoordinateOutOfBoundsException : Exception
{
    public CoordinateOutOfBoundsException() { }
    public CoordinateOutOfBoundsException(string message) : base(message) { }
    public CoordinateOutOfBoundsException(string message, Exception inner) : base(message, inner) { }
    protected CoordinateOutOfBoundsException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}