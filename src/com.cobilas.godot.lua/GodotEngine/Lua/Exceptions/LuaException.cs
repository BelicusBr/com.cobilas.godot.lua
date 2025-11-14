using System;

namespace Cobilas.GodotEngine.Lua.Exceptions;

public class LuaException : Exception {
	public LuaException() { }
	public LuaException(string message) : base(message) { }
	public LuaException(string message, Exception inner) : base(message, inner) { }
	protected LuaException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
