using System.Collections.Generic;

namespace Cobilas.GodotEngine.GDLua.Interfaces;

public interface ILuaTableItem : ILuaTable, IEnumerable<ILuaTable> {
    long Count { get; }
    ILuaTable[] Tables { get; }

    ILuaTable this[int index] { get; }
    ILuaTable this[string name] { get; }
}
