using System;

namespace Cobilas.GodotEngine.GDLua;
/// <summary>Indicates that a type is serializable for Lua interoperability.</summary>
/// <remarks>
/// Apply this attribute to classes or structures to mark them as serializable
/// for communication between C# and Lua environments in the Godot engine.
/// The attribute can be applied multiple times to handle multiple target types.
/// </remarks>
/// <param name="typeTarget">The target type that this serialization converter supports.</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
public sealed class LuaSerializableAttribute(Type typeTarget) : Attribute {
    /// <summary>Gets the target type that this serialization converter supports.</summary>
    /// <value>The <see cref="Type"/> that can be serialized to and from Lua tables.</value>
    public Type TypeTarget { get; private set; } = typeTarget;
}