using NLua;
using System;

namespace Cobilas.GodotEngine.GDLua;

public static class Lua_GD_CB_Extension {
    public static T GetValue<T>(this Lua? L, string fullPath) {
        if (L is null) throw new ArgumentNullException(nameof(L));
        return (T)L[fullPath];
    }
}
