using System;
using System.IO;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using Cobilas.GodotEngine.Utility.IO;
using Cobilas.GodotEngine.Utility.IO.Interfaces;

namespace Cobilas.GodotEngine.GDLua;
public sealed class LuaScriptFile : LuaScript {
	private Script? script;
	private ArchiveInfo? info;
	private bool disposedValue;
	private DateTime _LastWriteTimeUtc;
	private IGodotArchiveStream? archiveStream;

	public bool RefreshLuaScript { get; set; }
	public override IScriptLoader ScriptLoader {
		get => GetScript().Options.ScriptLoader;
		set => GetScript().Options.ScriptLoader = value;
	}
	public override Func<string, string> DebugInput {
		get => GetScript().Options.DebugInput;
		set => GetScript().Options.DebugInput = value;
	}
	public override Action<string> DebugPrint {
		get => GetScript().Options.DebugPrint;
		set => GetScript().Options.DebugPrint = value;
	}

	public LuaScriptFile(string? path) {
		if (path is null) throw new ArgumentNullException(nameof(path));
		RefreshLuaScript = false;
		info = new(path);
		_LastWriteTimeUtc = info.GetLastWriteTimeUtc;
		archiveStream = (IGodotArchiveStream)info.Open(FileAccess.Read, StreamType.GDStream);
		archiveStream.Read(out string stg);
		_ = (script = new()).DoString(stg);
	}

	public override LuaField GetField(string? pathField) {
		RefershBuffer();
		if (string.IsNullOrEmpty(pathField)) 
			throw new ArgumentNullException(nameof(pathField), $"The argument {nameof(pathField)} is null or empty!");
		DynValue dyn = GetScript().Globals.Get(pathField);
		return new(pathField!, DynValueToObject(dyn));
	}

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

	public override LuaFunc GetFunction(string? pathFunction) {
		RefershBuffer();
		if (string.IsNullOrEmpty(pathFunction))
			throw new ArgumentNullException(nameof(pathFunction), $"The argument {nameof(pathFunction)} is null or empty!");
		return new();
	}

	public override void SetField(string? pathField, object? value) {
		RefershBuffer();
		if (string.IsNullOrEmpty(pathField))
			throw new ArgumentNullException(nameof(pathField), $"The argument {nameof(pathField)} is null or empty!");
		else if (value is null)
			throw new ArgumentNullException(nameof(value));
		GetScript().Globals.Set(pathField, ObjectToDynValue(value));
	}

	public override void SetTuplaField(string[]? pathFields, object[]? value) {
		RefershBuffer();
		if (pathFields is null)
			throw new ArgumentNullException(nameof(pathFields));
		else if (value is null)
			throw new ArgumentNullException(nameof(value));
		DynValue?[] tupla = new DynValue[value.LongLength];
		for (long I = 0; I < value.LongLength; I++)
			tupla[I] = ObjectToDynValue(value[I]);
		GetScript().Globals.Set(pathFields, DynValue.NewTuple(tupla));
	}

	public override void SetFunction(string? pathFunction, Delegate? value) {
		RefershBuffer();
		if (string.IsNullOrEmpty(pathFunction))
			throw new ArgumentNullException(nameof(pathFunction), $"The argument {nameof(pathFunction)} is null or empty!");
		GetScript().Globals.Set(pathFunction, DynValue.FromObject(GetScript(), value));
	}

	public override void Dispose() {
		ObjectDisposed();
		disposedValue = true;
		info?.Dispose();
		archiveStream?.Dispose();
		info = null;
		script = null;
		archiveStream = null;
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