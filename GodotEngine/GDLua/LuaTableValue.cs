using Cobilas.GodotEngine.GDLua.Interfaces;

namespace Cobilas.GodotEngine.GDLua;
/// <summary>Represents a basic Lua table element with a name and value.</summary>
/// <remarks>
/// This structure implements <see cref="ILuaTable"/> to provide
/// a simple key-value pair representation for Lua table elements.
/// </remarks>
/// <param name="name">The name identifier of the table element.</param>
/// <param name="value">The value stored in the table element.</param>
public readonly struct LuaTableValue(string name, object value) : ILuaTable {
    private readonly string _name = name;
    private readonly object _value = value;
    /// <inheritdoc/>
    public string Name => _name;
    /// <inheritdoc/>
    public object Value => _value;
    /// <summary>Returns a string representation of the table element in "name = value" format.</summary>
    /// <returns>A formatted string showing the name-value pair.</returns>
    public override string ToString()
        => string.Format("{0} = {1}", _name, _value);
}