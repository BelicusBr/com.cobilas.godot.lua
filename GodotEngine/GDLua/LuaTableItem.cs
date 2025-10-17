using System.Text;
using System.Collections;
using Cobilas.Collections;
using System.Collections.Generic;
using Cobilas.GodotEngine.GDLua.Interfaces;

namespace Cobilas.GodotEngine.GDLua;
/// <summary>Represents a Lua table item that can contain multiple table elements and supports enumeration.</summary>
/// <remarks>
/// This structure implements <see cref="ILuaTableItem"/> to provide collection capabilities
/// for Lua table structures with hierarchical organization and string representation.
/// </remarks>
/// <param name="name">The name identifier for the table item.</param>
/// <param name="items">The array of Lua table elements to initialize the table with.</param>
public readonly struct LuaTableItem(string name, params ILuaTable[] items) : ILuaTableItem {
    private readonly string _name = name;
    private readonly ILuaTable[] _items = items;
    /// <inheritdoc/>
    public string Name => _name;
    /// <inheritdoc/>
    public ILuaTable[] Tables => _items;
    /// <inheritdoc/>
    public long Count => ArrayManipulation.ArrayLongLength(_items);
    /// <inheritdoc/>
    object ILuaTable.Value => _items;
    /// <inheritdoc/>
    public ILuaTable this[int index] => _items[index];
    /// <inheritdoc/>
    public ILuaTable this[string name] => _items[ArrayManipulation.FindIndex(_items, (lt) => lt.Name == name)];
    /// <summary>Returns a string representation of the Lua table hierarchy.</summary>
    /// <returns>A formatted string showing the table structure with proper indentation.</returns>
    public override string ToString() {
        StringBuilder builder = new();
        ToString(builder, 0, this);
        return builder.ToString();
    }
    /// <inheritdoc/>
    public IEnumerator<ILuaTable> GetEnumerator() => new ArrayToIEnumerator<ILuaTable>(_items);
    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => new ArrayToIEnumerator<ILuaTable>(_items);

    private static void ToString(StringBuilder builder, int tab, LuaTableItem item) {
        builder.AppendFormat("{0}{1} = {{\r\n", GetTab(tab), item._name);
        for (int I = 0; I < item.Count; I++) {
            switch (item[I]) {
                case LuaTableItem lti:
                    ToString(builder, tab + 1, lti);
                    break;
                default:
                    builder.AppendFormat("{0}{1} = {2}{3}\r\n", GetTab(tab + 1), item[I].Name,
                        item[I].Value, I < item.Count - 1 ? "," : string.Empty);
                    break;
            }
        }
        builder.AppendFormat("{0}}}{1}\r\n", GetTab(tab), tab != 0 ? "," : string.Empty);
    }

    private static string GetTab(int tab) => string.Empty.PadLeft(tab, '\t');
}