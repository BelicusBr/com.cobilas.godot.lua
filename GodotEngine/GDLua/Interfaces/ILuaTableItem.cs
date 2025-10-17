using System.Collections.Generic;

namespace Cobilas.GodotEngine.GDLua.Interfaces;
/// <summary>Represents a Lua table item that can contain multiple table elements and supports enumeration.</summary>
/// <remarks>Extends <see cref="ILuaTable"/> to provide collection capabilities for Lua table structures.</remarks>
public interface ILuaTableItem : ILuaTable, IEnumerable<ILuaTable> {
    /// <summary>Gets the number of table elements contained in this table item.</summary>
    /// <value>The total count of table elements.</value>
    long Count { get; }
    /// <summary>Gets an array of all table elements contained in this table item.</summary>
    /// <value>An array of <see cref="ILuaTable"/> elements.</value>
    ILuaTable[] Tables { get; }
    /// <summary>Gets the table element at the specified index.</summary>
    /// <param name="index">The zero-based index of the table element to get.</param>
    /// <value>The <see cref="ILuaTable"/> at the specified index.</value>
    ILuaTable this[int index] { get; }
    /// <summary>Gets the table element with the specified name.</summary>
    /// <param name="name">The name of the table element to get.</param>
    /// <value>The <see cref="ILuaTable"/> with the specified name.</value>
    ILuaTable this[string name] { get; }
}