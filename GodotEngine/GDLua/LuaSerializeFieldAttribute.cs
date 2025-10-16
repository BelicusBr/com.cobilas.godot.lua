using System;

namespace Cobilas.GodotEngine.GDLua;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class LuaSerializeFieldAttribute(string? fieldName) : Attribute {
    public string? FieldName { get; private set; } = fieldName;

    public LuaSerializeFieldAttribute() : this(null) { }
}
