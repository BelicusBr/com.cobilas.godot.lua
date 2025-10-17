namespace Cobilas.GodotEngine.GDLua.Interfaces;
/// <summary>Defines configuration settings for Lua file operations in the Godot engine.</summary>
/// <remarks>
/// Extends <see cref="ILuaContainerConfg"/> with file-specific configuration options
/// for loading and executing Lua scripts from files.
/// </remarks>
public interface ILuaFileConfg : ILuaContainerConfg {
    /// <summary>Gets the file path to the Lua script.</summary>
    /// <value>The path to the Lua file, or null if not specified.</value>
    public string? FilePath { get; }
    /// <summary>Gets a value indicating whether the file buffer should be refreshed on each access.</summary>
    /// <value>true to refresh the buffer on each access; otherwise, false.</value>
    public bool RefreshBuffer { get; }
}