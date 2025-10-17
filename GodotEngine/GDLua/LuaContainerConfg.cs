using KeraLua;
using Cobilas.GodotEngine.GDLua.Interfaces;

namespace Cobilas.GodotEngine.GDLua;
/// <summary>Represents the configuration used to initialize a <see cref="LuaContainer"/> instance.</summary>
/// <remarks>
/// This structure defines options for setting up the Lua environment,
/// including whether to load CLR packages, open Lua libraries,
/// and reuse an existing <see cref="Lua"/> state.
/// </remarks>
public readonly struct LuaContainerConfg(
    Lua? luaState = null,
    bool useCLRPackage = false,
    bool openLibs = true) : ILuaContainerConfg {
    private readonly bool openLibs = openLibs;
    private readonly Lua? luaState = luaState;
    private readonly bool useCLRPackage = useCLRPackage;
    /// <inheritdoc/>
    public bool OpenLibs => openLibs;
    /// <inheritdoc/>
    public Lua? LuaState => luaState;
    /// <inheritdoc/>
    public bool UseCLRPackage => useCLRPackage;
    /// <summary>Gets a default configuration for the <see cref="LuaContainer"/>.</summary>
    /// <remarks>
    /// The default configuration uses no predefined Lua state,
    /// disables CLR package loading, and enables standard Lua libraries.
    /// </remarks>
    public static LuaContainerConfg Default => new(luaState: null, useCLRPackage: false, openLibs: true);
}
