using System;
using MoonSharp.Interpreter;

namespace Cobilas.GodotEngine.Lua;

public struct LuaFunc : IDisposable {
	private Script? script;
	private RefIdObject? value;
	private bool disposedValue;
	private string functionName;

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

	public object? Call(params object[]? args)
		=> value switch {
			Closure => ClosureCall(args),
			_ => CallbackFunctionCall(args),
		};

	public object? Call() => Call([]);
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