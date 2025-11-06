using NLua;
using System;
using System.Linq;
using Cobilas.Collections;

namespace Cobilas.GodotEngine.GDLua;
/// <summary>Represents a wrapper for a Lua function, providing safe invocation and resource management.</summary>
/// <remarks>
/// This struct encapsulates a <see cref="LuaFunction"/> and provides methods to invoke it
/// with proper exception handling and resource disposal. Implements <see cref="IDisposable"/>
/// for proper cleanup of the underlying Lua function.
/// </remarks>
public struct LuaFunc(LuaFunction? function) : IDisposable {
	private LuaFunction? function = function;
	private bool disposedValue = false;
	/// <summary>Invokes the Lua function and returns all results as an array of Lua fields.</summary>
	/// <param name="fields">The output parameter that receives the function's return values as an array of <see cref="LuaField"/>.</param>
	/// <param name="args">The arguments to pass to the Lua function.</param>
	/// <exception cref="ObjectDisposedException">Thrown when the LuaFunc has been disposed.</exception>
	/// <exception cref="ArgumentNullException">Thrown when the underlying Lua function is null.</exception>
	public readonly void Invoke(out LuaField[] fields, params object[] args) => fields = IInvoke(args);
	/// <summary>Invokes the Lua function and returns the first result as a Lua field.</summary>
	/// <param name="field">The output parameter that receives the first return value as a <see cref="LuaField"/>.</param>
	/// <param name="args">The arguments to pass to the Lua function.</param>
	/// <exception cref="ObjectDisposedException">Thrown when the LuaFunc has been disposed.</exception>
	/// <exception cref="ArgumentNullException">Thrown when the underlying Lua function is null.</exception>
	public readonly void Invoke(out LuaField field, params object[] args) => field = IInvoke(args).First();
	/// <summary>Invokes the Lua function without capturing return values.</summary>
	/// <param name="args">The arguments to pass to the Lua function.</param>
	/// <exception cref="ObjectDisposedException">Thrown when the LuaFunc has been disposed.</exception>
	/// <exception cref="ArgumentNullException">Thrown when the underlying Lua function is null.</exception>
	public readonly void Invoke(params object[] args) => _ = IInvoke(args);
	/// <summary>Releases all resources used by the LuaFunc instance.</summary>
	/// <exception cref="ObjectDisposedException">Thrown when the LuaFunc has already been disposed.</exception>
	public void Dispose() {
		if (disposedValue)
			throw new ObjectDisposedException(nameof(LuaFunc));
		disposedValue = true;
		function?.Dispose();
		function = null;
	}

	private readonly LuaField[] IInvoke(params object[] args) {
		if (disposedValue) throw new ObjectDisposedException(nameof(LuaFunc));
		else if (function is null) throw new ArgumentNullException(nameof(function));
		object[] res = function.Call(args);
		LuaField[] result = new LuaField[ArrayManipulation.ArrayLongLength(res)];
		for (long I = 0; I < result.Length; I++)
			result[I] = new($"result[{I}]", res[I]);
		return result;
	}
}