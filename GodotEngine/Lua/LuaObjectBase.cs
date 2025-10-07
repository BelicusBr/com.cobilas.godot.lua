using System;
using LuaC = NLua.Lua;

namespace Cobilas.GodotEngine.Lua;

public class LuaObjectBase : IDisposable
{
    private LuaC? _lua;

    public LuaObjectBase()
    {

    }

    protected void Init(bool openLibs = true) => _lua = new(openLibs);

    public void Dispose()
    {
        ((IDisposable?)_lua)?.Dispose();
    }
}
