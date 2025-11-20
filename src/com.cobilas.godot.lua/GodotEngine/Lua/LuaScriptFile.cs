using System;
using System.IO;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using Cobilas.GodotEngine.Utility.IO;
using Cobilas.GodotEngine.Utility.IO.Interfaces;

namespace Cobilas.GodotEngine.Lua;
/// <summary>Represents a Lua script loaded from a file with automatic refresh capabilities.</summary>
/// <remarks>
/// This class provides file-based Lua script execution with optional automatic
/// reloading when the source file changes. It inherits from <see cref="LuaScript"/> to provide
/// full Lua scripting capabilities while adding file monitoring features.
/// </remarks>
public sealed class LuaScriptFile : LuaScript {
	private Script? script;
	private ArchiveInfo? info;
	private bool disposedValue;
	private DateTime _LastWriteTimeUtc;
	private IGodotArchiveStream? archiveStream;
	/// <summary>Gets or sets a value indicating whether the Lua script should be automatically refreshed when the source file changes.</summary>
	/// <returns>True if the script should be refreshed on file changes; otherwise, false.</returns>
	/// <value>True to enable automatic script refreshing; false to disable it.</value>
	/// <remarks>
	/// When set to true, the script will automatically reload from the source file
	/// when changes are detected. This is useful during development when frequently
	/// modifying Lua scripts without restarting the application.
	/// </remarks>
	public bool RefreshLuaScript { get; set; }
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
	/// <summary>Initializes a new instance of the <see cref="LuaScriptFile"/> class with the specified file path.</summary>
	/// <param name="path">The path to the Lua script file.</param>
	/// <exception cref="ArgumentNullException">Thrown when path is null.</exception>
	public LuaScriptFile(string? path) {
		if (path is null) throw new ArgumentNullException(nameof(path));
		RefreshLuaScript = false;
		info = new(path);
		_LastWriteTimeUtc = info.GetLastWriteTimeUtc;
		archiveStream = (IGodotArchiveStream)info.Open(FileAccess.Read, StreamType.GDStream);
		archiveStream.Read(out string stg);
		_ = (script = new()).DoString(stg);
	}
	/// <summary>Initializes a new instance of the <see cref="LuaScriptFile"/> class with optional path globalization.</summary>
	/// <param name="path">The path to the Lua script file.</param>
	/// <param name="isGlobalizePath">Whether to globalize the path using <see cref="GodotPath.GlobalizePath(string)"/>.</param>
	/// <exception cref="ArgumentNullException">Thrown when path is null.</exception>
	public LuaScriptFile(string? path, bool isGlobalizePath) :
		this(isGlobalizePath ?
			GodotPath.GlobalizePath(path ?? throw new ArgumentNullException(nameof(path))) :
			path)
	{ }
	/// <inheritdoc/>
	public override LuaField GetField(string? pathField) {
		RefershBuffer();
		if (string.IsNullOrEmpty(pathField))
			throw new ArgumentNullException(nameof(pathField), $"The argument {nameof(pathField)} is null or empty!");
		DynValue dyn = GetScript().Globals.Get(pathField);
		return new(pathField!, DynValueToObject(dyn));
	}
	/// <inheritdoc/>
	public override LuaField[] GetTuplaField(string[]? pathFields) {
		RefershBuffer();
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
	public override LuaFunc GetFunction(string? pathFunction) {
		RefershBuffer();
		if (string.IsNullOrEmpty(pathFunction))
			throw new ArgumentNullException(nameof(pathFunction), $"The argument {nameof(pathFunction)} is null or empty!");
		RefIdObject? closure = (RefIdObject?)DynValueToObject(GetScript().Globals.Get(pathFunction));
		if (closure is Closure clr)
			return new(script, pathFunction!, clr);
		return new(script, pathFunction!, closure as CallbackFunction);
	}
	/// <inheritdoc/>
	public override void SetField(string? pathField, object? value) {
		RefershBuffer();
		if (string.IsNullOrEmpty(pathField))
			throw new ArgumentNullException(nameof(pathField), $"The argument {nameof(pathField)} is null or empty!");
		else if (value is null)
			throw new ArgumentNullException(nameof(value));
		GetScript().Globals.Set(pathField, ObjectToDynValue(script, value));
	}
	/// <inheritdoc/>
	public override void SetTuplaField(string[]? pathFields, object[]? value) {
		RefershBuffer();
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
	public override void SetFunction(string? pathFunction, Delegate? value) {
		RefershBuffer();
		if (string.IsNullOrEmpty(pathFunction))
			throw new ArgumentNullException(nameof(pathFunction), $"The argument {nameof(pathFunction)} is null or empty!");
		GetScript().Globals.Set(pathFunction, DynValue.FromObject(GetScript(), value));
	}
	/// <inheritdoc/>
	public override Table CreateTable(string? pathField) {
		Table result = new(script);
		GetScript().Globals[pathField] = result;
		return result;
	}
	/// <inheritdoc/>
	protected override void Dispose(bool disposing) {
		if (!disposedValue) {
			if (disposing) {
				info?.Dispose();
				archiveStream?.Dispose();
				info = null;
				script = null;
				archiveStream = null;
			}
			disposedValue = true;
		}
		else ObjectDisposed();
	}

	private void ObjectDisposed() {
		if (disposedValue)
			throw new ObjectDisposedException(nameof(LuaScriptFile));
	}

	private void RefershBuffer() {
		ObjectDisposed();
		if (!RefreshLuaScript) return;
		else if (_LastWriteTimeUtc == (_LastWriteTimeUtc = info!.GetLastWriteTimeUtc)) return;
		archiveStream!.Read(out string stg);
		_ = (script = new()).DoString(stg);
	}

	private Script GetScript() {
		ObjectDisposed();
		return script ?? throw new NullReferenceException($"The field of type 'MoonSharp.Interpreter.Script' {nameof(script)} is null!");
	}
}