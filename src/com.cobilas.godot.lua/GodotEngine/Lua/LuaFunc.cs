using System;
using MoonSharp.Interpreter;

namespace Cobilas.GodotEngine.Lua;
/// <summary>Represents a Lua function with invocation capabilities.</summary>
public struct LuaFunc : IDisposable {
	private Script? script;
	private RefIdObject? value;
	private bool disposedValue;
	private string functionName;
	/// <summary>Gets the name of the Lua function.</summary>
	/// <returns>The name identifier of the function.</returns>
	/// <exception cref="ObjectDisposedException">Thrown when the <seealso cref="LuaFunc"/> instance has been disposed.</exception>
	public readonly string FunctionName => !disposedValue ? functionName : throw new ObjectDisposedException(nameof(LuaFunc));

	internal LuaFunc(Script? script, string functionName, Closure? function) {
		value = function;
		this.script = script;
		this.functionName = functionName;
	}

	internal LuaFunc(Script? script, string functionName, CallbackFunction? function) {
		value = function;
		this.script = script;
		this.functionName = functionName;
	}
	/// <summary>Invokes the Lua function with the specified arguments.</summary>
	/// <param name="args">The arguments to pass to the function.</param>
	/// <returns>The result of the function invocation, or null if the function returns no value.</returns>
	/// <exception cref="ObjectDisposedException">Thrown when the <seealso cref="LuaFunc"/> instance has been disposed.</exception>
	/// <exception cref="ArgumentNullException">Thrown when args is null.</exception>
	public readonly object? Call(params object[]? args)
		=> value switch {
			Closure => ClosureCall(args),
			_ => CallbackFunctionCall(args),
		};
	/// <summary>Invokes the Lua function with no arguments.</summary>
	/// <returns>The result of the function invocation, or null if the function returns no value.</returns>
	public readonly object? Call() => Call([]);
	/// <inheritdoc/>
	public void Dispose() {
		if (disposedValue)
			throw new ObjectDisposedException(nameof(LuaFunc));
		disposedValue = true;
		value = null;
		script = null;
		functionName = string.Empty;
	}

	private readonly object? ClosureCall(params object[]? args) {
		if (disposedValue) throw new ObjectDisposedException(nameof(LuaFunc));
		else if (args is null) throw new ArgumentNullException(nameof(args));
		else if (value is null) throw new ArgumentNullException(nameof(value));
		return LuaScript.DynValueToObject(((Closure)value).Call(args));
	}

	private readonly object? CallbackFunctionCall(params object[]? args) {
		if (disposedValue) throw new ObjectDisposedException(nameof(LuaFunc));
		else if (args is null) throw new ArgumentNullException(nameof(args));
		else if (value is null) throw new ArgumentNullException(nameof(value));
		DynValue?[] dyns = new DynValue[args.LongLength];
		for (long I = 0; I < dyns.LongLength; I++)
			dyns[I] = LuaScript.ObjectToDynValue(script, args[I]);
		return LuaScript.DynValueToObject(((CallbackFunction)value).Invoke(null, dyns));
	}
}