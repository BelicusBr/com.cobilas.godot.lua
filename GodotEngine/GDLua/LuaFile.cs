using NLua;
using System;
using System.IO;
using NLua.Exceptions;
using Cobilas.GodotEngine.Utility.IO;
using Cobilas.GodotEngine.GDLua.Interfaces;
using Cobilas.GodotEngine.Utility.IO.Interfaces;

namespace Cobilas.GodotEngine.GDLua;
/// <summary>Represents a Lua file that can be loaded, executed, and manipulated within the Godot engine.</summary>
/// <remarks>
/// This class provides methods to interact with Lua scripts, including reading fields,
/// setting values, invoking functions, and converting Lua tables to C# objects.
/// Implements <see cref="IDisposable"/> for proper resource cleanup.
/// </remarks>
public sealed class LuaFile : IDisposable, ILuaFile {
    private bool disposed;
    private readonly Lua lua;
    private DateTime lastWriteTime;
    private readonly LuaFileConfg confg;
    private readonly ArchiveInfo archive;
    private readonly IGodotArchiveStream luaFile;
    /// <summary>Initializes a new instance of the <see cref="LuaFile"/> class with the specified configuration.</summary>
    /// <param name="confg">The configuration settings for the Lua file.</param>
    /// <exception cref="ArgumentNullException">Thrown when the file path in configuration is null.</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown when the directory containing the Lua file is not found.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the specified Lua file is not found.</exception>
    public LuaFile(LuaFileConfg confg) {
        this.confg = confg;
        lua = confg.LuaState is not null ? new(confg.LuaState) : new(confg.OpenLibs);
        if (confg.UseCLRPackage)
            lua.LoadCLRPackage();

        if (Archive.Exists(confg.FilePath)) {
            archive = new ArchiveInfo(confg.FilePath);
            lastWriteTime = archive.GetLastWriteTime;
            luaFile = (IGodotArchiveStream)archive.Open(FileAccess.ReadWrite, StreamType.GDStream);
        } else throw new FileNotFoundException(confg.FilePath);
        _ = lua.DoString(luaFile.Read());
    }
    /// <summary>Initializes a new instance of the <see cref="LuaFile"/> class with the specified file path.</summary>
    /// <param name="filePath">The path to the Lua file.</param>
    /// <param name="refreshBuffer">Whether to refresh the file buffer on each access.</param>
    public LuaFile(string filePath, bool refreshBuffer = false) : 
        this(new LuaFileConfg(filePath, null, useCLRPackage:false , refreshBuffer, openLibs:true)) { }
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown when the LuaFile has been disposed.</exception>
    public LuaField GetField(string pathField) {
        ObjectDisposed(disposed);
		RefreshBuffer();
		object value = lua[pathField];
        if (value is LuaFunction) throw new LuaException($"{pathField} is {nameof(LuaFunction)}");
        return new(pathField, value);
    }
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown when the LuaFile has been disposed.</exception>
    public void SetField(string pathField, object value) {
        ObjectDisposed(disposed);
		RefreshBuffer();
		if (ObjectToLuaTable.TryGetValue(value.GetType(), out ObjectToLuaTable func))
            func.ToLuaTable(value, lua.GetTable(pathField));
        else lua.SetObjectToPath(pathField, value);
    }
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown when the LuaFile has been disposed.</exception>
    public object[] InvokeFunction(string methodName, params object[] args) {
        ObjectDisposed(disposed);
		RefreshBuffer();
		return lua[methodName] is LuaFunction lf ? lf.Call(args) : throw new LuaException($"{methodName} is {nameof(LuaField)}");
    }
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown when the LuaFile has been disposed.</exception>
    public LuaField LuaTableToObject<T>(string pathField) {
        ObjectDisposed(disposed);
		RefreshBuffer();
		if (ObjectToLuaTable.TryGetValue(typeof(T), out ObjectToLuaTable table))
            return new(pathField, table.ToObject(typeof(T).Activator(), lua[pathField] as LuaTable));
        return new(pathField, lua[pathField]);
    }
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown when the LuaFile has been disposed.</exception>
    /// <exception cref="InvalidCastException">Thrown when no converter is found for the specified type.</exception>
    public void LuaTableToObject<T>(string pathField, ref T value) {
        ObjectDisposed(disposed);
        RefreshBuffer();
		if (ObjectToLuaTable.TryGetValue(typeof(T), out ObjectToLuaTable table))
            value = (T)table.ToObject(value, lua[pathField] as LuaTable);
        else throw new InvalidCastException($"The type {typeof(T)} does not have an `ObjectToLuaTable` converter defined.");
    }
    /// <summary>Releases all resources used by the LuaFile instance.</summary>
    public void Dispose() {
        ObjectDisposed(disposed);
        disposed = true;
        ((IDisposable)lua).Dispose();
		((IDisposable)archive).Dispose();
		((IDisposable)luaFile).Dispose();
	}

    private void RefreshBuffer() {
        if (!confg.RefreshBuffer) return;
        if (lastWriteTime == (lastWriteTime = archive.GetLastWriteTime)) return;
		luaFile.RefreshBuffer();
		_ = lua.DoString(luaFile.Read());
	}


	internal static void ObjectDisposed(bool disposed) {
        if (disposed) throw new ObjectDisposedException(nameof(LuaContainer));
    }
}