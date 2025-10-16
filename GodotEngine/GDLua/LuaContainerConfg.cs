using KeraLua;
using Cobilas.GodotEngine.GDLua.Interfaces;

namespace Cobilas.GodotEngine.GDLua;

public readonly struct LuaContainerConfg(
    Lua? luaState = null,
    bool useCLRPackage = false,
    bool openLibs = true) : ILuaContainerConfg {

    private readonly bool openLibs = openLibs;
    private readonly Lua? luaState = luaState;
    private readonly bool useCLRPackage = useCLRPackage;

    public bool OpenLibs => openLibs;
    public Lua? LuaState => luaState;
    public bool UseCLRPackage => useCLRPackage;

    public static LuaContainerConfg Default => new(luaState: null, useCLRPackage: false, openLibs: true);
}
