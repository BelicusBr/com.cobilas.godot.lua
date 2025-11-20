namespace Cobilas.GodotEngine.Lua.Interfaces;
/// <summary>Represents a basic Lua table element with a name and value.</summary>
public interface ILuaTable {
    /// <summary>Gets the name of the Lua table element.</summary>
    /// <value>The name identifier of the table element.</value>
    string Name { get; }
    /// <summary>Gets the value stored in the Lua table element.</summary>
    /// <value>The value contained in the table element.</value>
    object Value { get; }
}