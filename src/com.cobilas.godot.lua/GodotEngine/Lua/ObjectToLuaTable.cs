using MoonSharp.Interpreter;

namespace Cobilas.GodotEngine.Lua;
/// <summary>Provides a base class for converting between C# objects and Lua tables.</summary>
/// <remarks>
/// This abstract class serves as the foundation for type-specific converters
/// that handle serialization and deserialization between C# objects and Lua tables
/// in the Godot engine's Lua integration system.
/// </remarks>
public abstract class ObjectToLuaTable {
    /// <summary>Converts a C# object to a Lua table.</summary>
    /// <param name="obj">The C# object to convert.</param>
    /// <param name="table">The Lua table to populate with the object's data.</param>
    public abstract void ToLuaTable(object? obj, Table? table);
    /// <summary>Converts a Lua table back to a C# object.</summary>
    /// <param name="obj">The original object instance (may be used for context).</param>
    /// <param name="table">The Lua table containing the serialized data.</param>
    /// <returns>A C# object reconstructed from the Lua table data.</returns>
    public abstract object ToObject(object? obj, Table? table);
}