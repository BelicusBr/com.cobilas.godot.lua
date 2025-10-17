using System.Text;
using System.Collections;
using Cobilas.Collections;
using System.Collections.Generic;
using Cobilas.GodotEngine.GDLua.Interfaces;

namespace Cobilas.GodotEngine.GDLua;

public readonly struct LuaTableItem : ILuaTableItem {
    private readonly string _name;
    private readonly ILuaTable[] _items;

    public string Name => _name;
    public ILuaTable[] Tables => _items;
    public long Count => ArrayManipulation.ArrayLongLength(_items);

    object ILuaTable.Value => _items;

    public ILuaTable this[int index] => _items[index];
    public ILuaTable this[string name] => _items[ArrayManipulation.FindIndex(_items, (lt) => lt.Name == name)];

    public LuaTableItem(string name, params ILuaTable[] items) {
        _name = name;
        _items = items;
    }

    public override string ToString() {
        StringBuilder builder = new();
        ToString(builder, 0, this);
        return builder.ToString();
    }

    public IEnumerator<ILuaTable> GetEnumerator()
        => new ArrayToIEnumerator<ILuaTable>(_items);

    IEnumerator IEnumerable.GetEnumerator()
        => new ArrayToIEnumerator<ILuaTable>(_items);

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
