using Cobilas.GodotEngine.GDLua.Interfaces;

namespace Cobilas.GodotEngine.GDLua;

public readonly struct LuaTableValue : ILuaTable {
    private readonly string _name;
    private readonly object _value;

    public string Name => _name;
    public object Value => _value;

    public LuaTableValue(string name, object value) {
        _name = name;
        _value = value;
    }

    public override string ToString()
        => string.Format("{0} = {1}", _name, _value);
}
