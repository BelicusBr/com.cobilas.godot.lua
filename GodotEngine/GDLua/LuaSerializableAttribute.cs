using System;

namespace Cobilas.GodotEngine.GDLua;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
public sealed class LuaSerializableAttribute : Attribute { }
