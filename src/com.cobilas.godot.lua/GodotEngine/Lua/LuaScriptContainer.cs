using System;
using System.Text;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace Cobilas.GodotEngine.Lua;
/// <summary>Represents an in-memory Lua script container that can be built from strings or builders.</summary>
/// <remarks>
/// This class provides a way to execute Lua scripts from code strings, <see cref="StringBuilder"/>,
/// or <see cref="LuaScriptContainerBuilder"/> instances without requiring external files.
/// </remarks>
public sealed class LuaScriptContainer : LuaScript {
	private Script? script;
	private bool disposedValue;
	/// <inheritdoc/>
	public override IScriptLoader ScriptLoader {
		get => GetScript().Options.ScriptLoader;
		set => GetScript().Options.ScriptLoader = value;
	}
	/// <inheritdoc/>
	public override Func<string, string> DebugInput {
		get => GetScript().Options.DebugInput;
		set => GetScript().Options.DebugInput = value;
	}
	/// <inheritdoc/>
	public override Action<string> DebugPrint {
		get => GetScript().Options.DebugPrint;
		set => GetScript().Options.DebugPrint = value;
	}
	/// <summary>Initializes a new instance of the <see cref="LuaScriptContainer"/> class with the specified Lua code.</summary>
	/// <param name="code">The Lua code to execute.</param>
	/// <exception cref="ArgumentNullException">Thrown when code is null.</exception>
	public LuaScriptContainer(string? code) {
		if (code is null) throw new ArgumentNullException(nameof(code));
		_ = (script = new()).DoString(code);
	}
	/// <summary>Initializes a new instance of the <see cref="LuaScriptContainer"/> class from a <see cref="StringBuilder"/>.</summary>
	/// <param name="builder">The string builder containing Lua code.</param>
	/// <exception cref="ArgumentNullException">Thrown when builder is null.</exception>
	public LuaScriptContainer(StringBuilder? builder) :
		this((builder ?? throw new ArgumentNullException(nameof(builder))).ToString())
	{ }
	/// <summary>Initializes a new instance of the <see cref="LuaScriptContainer"/> class from a <see cref="LuaScriptContainerBuilder"/>.</summary>
	/// <param name="builder">The script container builder containing Lua code.</param>
	/// <exception cref="ArgumentNullException">Thrown when builder is null.</exception>
	public LuaScriptContainer(LuaScriptContainerBuilder? builder) :
		this((builder ?? throw new ArgumentNullException(nameof(builder))).ToString())
	{ }
	/// <inheritdoc/>
	public override Table CreateTable(string? pathField) {
		Table result = new(script);
		GetScript().Globals[pathField] = result;
		return result;
	}
	/// <inheritdoc/>
	public override LuaField GetField(string? pathField) {
		if (string.IsNullOrEmpty(pathField))
			throw new ArgumentNullException(nameof(pathField), $"The argument {nameof(pathField)} is null or empty!");
		DynValue dyn = GetScript().Globals.Get(pathField);
		return new(pathField!, DynValueToObject(dyn));
	}
	/// <inheritdoc/>
	public override LuaFunc GetFunction(string? pathFunction) {
		if (string.IsNullOrEmpty(pathFunction))
			throw new ArgumentNullException(nameof(pathFunction), $"The argument {nameof(pathFunction)} is null or empty!");
		RefIdObject? closure = (RefIdObject?)DynValueToObject(GetScript().Globals.Get(pathFunction));
		if (closure is Closure clr)
			return new(script, pathFunction!, clr);
		return new(script, pathFunction!, closure as CallbackFunction);
	}
	/// <inheritdoc/>
	public override LuaField[] GetTuplaField(string[]? pathFields) {
		if (pathFields is null)
			throw new ArgumentNullException(nameof(pathFields));
		DynValue dyn = GetScript().Globals.Get(pathFields);
		DynValue[] tupla = dyn.Tuple;
		LuaField[] fields = new LuaField[tupla.LongLength];
		for (long I = 0; I < fields.LongLength; I++)
			fields[I] = new(pathFields[I], DynValueToObject(tupla[I]));
		return fields;
	}
	/// <inheritdoc/>
	public override void SetField(string? pathField, object? value) {
		if (string.IsNullOrEmpty(pathField))
			throw new ArgumentNullException(nameof(pathField), $"The argument {nameof(pathField)} is null or empty!");
		else if (value is null)
			throw new ArgumentNullException(nameof(value));
		GetScript().Globals.Set(pathField, ObjectToDynValue(script, value));
	}
	/// <inheritdoc/>
	public override void SetFunction(string? pathFunction, Delegate? value) {
		if (string.IsNullOrEmpty(pathFunction))
			throw new ArgumentNullException(nameof(pathFunction), $"The argument {nameof(pathFunction)} is null or empty!");
		GetScript().Globals.Set(pathFunction, DynValue.FromObject(GetScript(), value));
	}
	/// <inheritdoc/>
	public override void SetTuplaField(string[]? pathFields, object[]? value) {
		if (pathFields is null)
			throw new ArgumentNullException(nameof(pathFields));
		else if (value is null)
			throw new ArgumentNullException(nameof(value));
		DynValue?[] tupla = new DynValue[value.LongLength];
		for (long I = 0; I < value.LongLength; I++)
			tupla[I] = ObjectToDynValue(script, value[I]);
		GetScript().Globals.Set(pathFields, DynValue.NewTuple(tupla));
	}
	/// <inheritdoc/>
	protected override void Dispose(bool disposing) {
		if (!disposedValue) {
			if (disposing) {
				script = null;
			}
			disposedValue = true;
		}
		else ObjectDisposed();
	}

	private void ObjectDisposed() {
		if (disposedValue)
			throw new ObjectDisposedException(nameof(LuaScriptFile));
	}

	private Script GetScript() {
		ObjectDisposed();
		return script ?? throw new NullReferenceException($"The field of type 'MoonSharp.Interpreter.Script' {nameof(script)} is null!");
	}
}