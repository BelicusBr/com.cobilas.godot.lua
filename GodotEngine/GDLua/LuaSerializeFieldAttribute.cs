using System;

namespace Cobilas.GodotEngine.GDLua;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class LuaSerializeFieldAttribute : Attribute { }
