using NLua;
using System;
using System.Collections.Generic;

namespace Cobilas.GodotEngine.GDLua;

/// <summary>Provides a base class for converting between C# objects and Lua tables.</summary>
/// <remarks>
/// This abstract class serves as the foundation for type-specific converters
/// that handle serialization and deserialization between C# objects and Lua tables
/// in the Godot engine's Lua integration system.
/// </remarks>
public abstract class ObjectToLuaTable {
    private static readonly Dictionary<Type, ObjectToLuaTable> converters = GetConverters();
    /// <summary>Converts a C# object to a Lua table.</summary>
    /// <param name="obj">The C# object to convert.</param>
    /// <param name="table">The Lua table to populate with the object's data.</param>
    public abstract void ToLuaTable(object? obj, LuaTable? table);
    /// <summary>Converts a Lua table back to a C# object.</summary>
    /// <param name="obj">The original object instance (may be used for context).</param>
    /// <param name="table">The Lua table containing the serialized data.</param>
    /// <returns>A C# object reconstructed from the Lua table data.</returns>
    public abstract object ToObject(object? obj, LuaTable? table);
    /// <summary>Attempts to retrieve a converter for the specified type.</summary>
    /// <param name="type">The type to find a converter for.</param>
    /// <param name="value">When this method returns, contains the converter associated with the specified type, if found; otherwise, null.</param>
    /// <returns>true if a converter for the specified type was found; otherwise, false.</returns>
    public static bool TryGetValue(Type type, out ObjectToLuaTable value)
        => converters.TryGetValue(type, out value);

    // Private method not documented as requested
    private static Dictionary<Type, ObjectToLuaTable> GetConverters() {
        Dictionary<Type, ObjectToLuaTable> result = [];
        foreach (Type item in TypeUtilitarian.GetTypes())
            if (item.CompareTypeAndSubType<ObjectToLuaTable>()) {
                ObjectToLuaTable obj2lt = item.Activator<ObjectToLuaTable>();
                LuaSerializableAttribute[] attributes = item.GetAttributes<LuaSerializableAttribute>();
                if (attributes is not null)
                    foreach (LuaSerializableAttribute item2 in attributes)
                        result.Add(item2.TypeTarget, obj2lt);
            }
        return result;
    }
}