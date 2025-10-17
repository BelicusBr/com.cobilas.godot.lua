using NLua;
using System;
using System.Collections.Generic;

namespace Cobilas.GodotEngine.GDLua;

public abstract class ObjectToLuaTable {
    private static readonly Dictionary<Type, ObjectToLuaTable> converters = GetConverters();

    public abstract void ToLuaTable(object obj, LuaTable table);
    public abstract object ToObject(object obj, LuaTable table);

    public static bool TryGetValue(Type type, out ObjectToLuaTable value)
        => converters.TryGetValue(type, out value);

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
