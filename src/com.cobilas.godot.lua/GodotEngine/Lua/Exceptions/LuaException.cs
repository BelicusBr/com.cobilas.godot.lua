using System;

namespace Cobilas.GodotEngine.Lua.Exceptions;
/// <summary>Represents errors that occur during Lua script execution in the Godot Engine.</summary>
public class LuaException : Exception {
	/// <summary>Initializes a new instance of the <see cref="LuaException"/> class.</summary>
	public LuaException() { }
	/// <summary>Initializes a new instance of the <see cref="LuaException"/> class with a specified error message.</summary>
	/// <param name="message">The message that describes the error.</param>
	public LuaException(string message) : base(message) { }
	/// <summary>Initializes a new instance of the <see cref="LuaException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
	public LuaException(string message, Exception inner) : base(message, inner) { }
	/// <summary>Initializes a new instance of the <see cref="LuaException"/> class with serialized data.</summary>
	/// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
	/// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
	protected LuaException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}