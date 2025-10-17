using KeraLua;
using Cobilas.GodotEngine.GDLua.Interfaces;

namespace Cobilas.GodotEngine.GDLua;
/// <summary>Represents configuration settings for Lua file operations in the Godot engine.</summary>
/// <remarks>
/// This structure provides configuration options for initializing and managing
/// Lua script files, including file paths, Lua state management, and package settings.
/// </remarks>
/// <param name="filePath">The path to the Lua script file.</param>
/// <param name="luaState">The existing Lua state instance to use, or null to create a new one.</param>
/// <param name="useCLRPackage">Whether to enable CLR package access from Lua scripts.</param>
/// <param name="refreshBuffer">Whether to refresh the file buffer on each access.</param>
/// <param name="openLibs">Whether to load standard Lua libraries.</param>
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
    /// <inheritdoc/>
    public bool OpenLibs => openLibs;
    /// <inheritdoc/>
    public Lua? LuaState => luaState;
    /// <inheritdoc/>
    public string? FilePath => filePath;
    /// <inheritdoc/>
    public bool RefreshBuffer => refreshBuffer;
    /// <inheritdoc/>
    public bool UseCLRPackage => useCLRPackage;
}