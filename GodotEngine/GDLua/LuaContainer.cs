using NLua;
using System;
using System.Text;
using NLua.Exceptions;
using Cobilas.GodotEngine.GDLua.Interfaces;

namespace Cobilas.GodotEngine.GDLua;
/// <summary>Represents an in-memory Lua code builder and executor integrated with the Godot engine.</summary>
/// <remarks>
/// This class implements <see cref="ILuaFile"/> and provides methods to dynamically create,
/// manipulate, and execute Lua code through an internal <see cref="StringBuilder"/> buffer.
/// It allows the construction of Lua scripts programmatically and their execution using NLua.
/// Implements <see cref="IDisposable"/> for proper resource cleanup.
/// </remarks>
public sealed class LuaContainer : IDisposable, ILuaFile {
    private bool disposed;
    private readonly Lua lua;
    private readonly StringBuilder builder;
    /// <summary>Gets the current string content of the internal <see cref="StringBuilder"/>.</summary>
    public string Builder => builder.ToString();
    /// <summary>Initializes a new instance of the <see cref="LuaContainer"/> class with the specified configuration.</summary>
    /// <param name="confg">The configuration used to initialize the Lua state and related options.</param>
    /// <exception cref="ArgumentNullException">Thrown when a required configuration property is null.</exception>
    public LuaContainer(LuaContainerConfg confg) {
        lua = confg.LuaState is not null ? new(confg.LuaState) : new(confg.OpenLibs);
        if (confg.UseCLRPackage)
            lua.LoadCLRPackage();
        builder = new();
    }
    /// <summary>Initializes a global Lua field with a specific value.</summary>
    /// <param name="pathField">The Lua field path to initialize.</param>
    /// <param name="value">The value to assign to the field.</param>
    /// <returns>The current <see cref="LuaContainer"/> instance for chaining.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the container has been disposed.</exception>
    public LuaContainer InitField(string pathField, object value) {
        LuaFile.ObjectDisposed(disposed);
        builder.AppendFormat("{0} = {1}\r\n", pathField, value);
        return this;
    }
    /// <summary>Initializes a local Lua field with a specific value.</summary>
    /// <param name="pathField">The Lua field name to initialize locally.</param>
    /// <param name="value">The value to assign to the field.</param>
    /// <returns>The current <see cref="LuaContainer"/> instance for chaining.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the container has been disposed.</exception>
    public LuaContainer InitLoaclField(string pathField, object value) {
        LuaFile.ObjectDisposed(disposed);
        builder.AppendFormat("local {0} = {1}\r\n", pathField, value);
        return this;
    }
    /// <summary>Initializes a Lua table by appending its string representation to the buffer.</summary>
    /// <param name="tables">The Lua table object to append.</param>
    /// <returns>The current <see cref="LuaContainer"/> instance for chaining.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the container has been disposed.</exception>
    public LuaContainer InitTable(LuaTableItem tables) {
        LuaFile.ObjectDisposed(disposed);
        builder.Append(tables.ToString());
        return this;
    }
    /// <summary>Initializes a Lua function with the specified name, body, and arguments.</summary>
    /// <param name="funcName">The name of the function.</param>
    /// <param name="funcBody">The Lua code representing the function body.</param>
    /// <param name="funcArgs">The arguments of the function.</param>
    /// <returns>The current <see cref="LuaContainer"/> instance for chaining.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the container has been disposed.</exception>
    public LuaContainer InitFunction(string funcName, string funcBody, params string[] funcArgs) {
        LuaFile.ObjectDisposed(disposed);
        builder.AppendFormat("function {0}({1})\r\n{2}\r\nend\r\n", funcName, string.Join(", ", funcArgs), funcBody);
        return this;
    }
    /// <summary>Adds a CLR import statement to the Lua buffer.</summary>
    /// <param name="import">The CLR namespace or type to import.</param>
    /// <returns>The current <see cref="LuaContainer"/> instance for chaining.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the container has been disposed.</exception>
    public LuaContainer InitCLRPackage(string import) {
        LuaFile.ObjectDisposed(disposed);
        builder.AppendFormat("import('{0}')\r\n", import);
        return this;
    }
    /// <summary>Appends a raw Lua code string to the buffer.</summary>
    /// <param name="value">The Lua code string to append.</param>
    /// <returns>The current <see cref="LuaContainer"/> instance for chaining.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the container has been disposed.</exception>
    public LuaContainer DoString(string value) {
        LuaFile.ObjectDisposed(disposed);
        builder.AppendLine(value);
        return this;
    }
    /// <summary>Clears the internal Lua code buffer.</summary>
    /// <returns>The current <see cref="LuaContainer"/> instance for chaining.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the container has been disposed.</exception>
    public LuaContainer ClearBuffer() {
        LuaFile.ObjectDisposed(disposed);
        builder.Clear();
        return this;
    }
    /// <summary>Executes the current Lua buffer using the NLua interpreter.</summary>
    /// <returns>The current <see cref="LuaContainer"/> instance for chaining.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the container has been disposed.</exception>
    public LuaContainer FlushToLua() {
        LuaFile.ObjectDisposed(disposed);
        _ = lua.DoString(builder.ToString());
        return this;
    }
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown when the container has been disposed.</exception>
    public void SetField(string pathField, object value) {
        LuaFile.ObjectDisposed(disposed);
        if (ObjectToLuaTable.TryGetValue(value.GetType(), out ObjectToLuaTable func))
            func.ToLuaTable(value, lua.GetTable(pathField));
        else lua.SetObjectToPath(pathField, value);
    }
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown when the container has been disposed.</exception>
    public LuaField GetField(string pathField) {
        LuaFile.ObjectDisposed(disposed);
        object value = lua[pathField];
        if (value is LuaFunction) throw new LuaException($"{pathField} is {nameof(LuaFunction)}");
        return new(pathField, lua[pathField]);
    }
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown when the container has been disposed.</exception>
    public object[] InvokeFunction(string methodName, params object[] args) {
        LuaFile.ObjectDisposed(disposed);
        return lua[methodName] is LuaFunction lf ? lf.Call(args) : throw new LuaException($"{methodName} is {nameof(LuaField)}");
    }
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown when the container has been disposed.</exception>
    public LuaField LuaTableToObject<T>(string pathField) {
        LuaFile.ObjectDisposed(disposed);
        if (ObjectToLuaTable.TryGetValue(typeof(T), out ObjectToLuaTable table))
            return new(pathField, table.ToObject(typeof(T).Activator(), lua[pathField] as LuaTable));
        return new(pathField, lua[pathField]);
    }
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown when the container has been disposed.</exception>
    /// <exception cref="InvalidCastException">Thrown when no converter is defined for the specified type.</exception>
    public void LuaTableToObject<T>(string pathField, ref T value) {
        LuaFile.ObjectDisposed(disposed);
        if (ObjectToLuaTable.TryGetValue(typeof(T), out ObjectToLuaTable table))
            value = (T)table.ToObject(value, lua[pathField] as LuaTable);
        else throw new InvalidCastException($"The type {typeof(T)} does not have an `ObjectToLuaTable` converter defined.");
    }
	/// <summary>Gets a Lua function from the specified path.</summary>
	/// <param name="pathFunc">The path to the Lua function.</param>
	/// <returns>A <see cref="LuaFunc"/> wrapper for the Lua function.</returns>
	/// <exception cref="ObjectDisposedException">Thrown when the container has been disposed.</exception>
	/// <exception cref="LuaException">Thrown when the specified path does not point to a Lua function.</exception>
	public LuaFunc GetLuaFunc(string pathFunc) {
		LuaFile.ObjectDisposed(disposed);
		if (lua[pathFunc] is LuaFunction lf) return new(lf);
		else throw new LuaException($"{pathFunc} is {nameof(LuaField)}");
	}
	/// <inheritdoc/>
	public void Dispose() {
        LuaFile.ObjectDisposed(disposed);
        disposed = true;
        ((IDisposable)lua).Dispose();
        builder.Clear();
        builder.Capacity = 0;
    }
}
