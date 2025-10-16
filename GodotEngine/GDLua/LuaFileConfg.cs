using KeraLua;
using Cobilas.GodotEngine.GDLua.Interfaces;

namespace Cobilas.GodotEngine.GDLua;

public readonly struct LuaFileConfg(
    string filePath,
    Lua? luaState = null,
    bool useCLRPackage = false,
    bool refreshBuffer = false,
    bool openLibs = true) : ILuaFileConfg {

    private readonly bool openLibs = openLibs;
    private readonly Lua? luaState = luaState;
    private readonly string? filePath = filePath;
    private readonly bool refreshBuffer = refreshBuffer;
    private readonly bool useCLRPackage = useCLRPackage;

    public bool OpenLibs => openLibs;
    public Lua? LuaState => luaState;
    public string? FilePath => filePath;
    public bool RefreshBuffer => refreshBuffer;
    public bool UseCLRPackage => useCLRPackage;
}
