using KeraLua;

namespace Cobilas.GodotEngine.GDLua.Interfaces;
/// <summary>Defines configuration settings for a Lua container in the Godot engine.</summary>
/// <remarks>
/// This interface provides the basic configuration options required
/// for initializing and managing Lua environments within Godot.
/// </remarks>
public interface ILuaContainerConfg {
    /// <summary>Gets a value indicating whether standard Lua libraries should be loaded.</summary>
    /// <value>true to load standard Lua libraries; otherwise, false.</value>
    public bool OpenLibs { get; }
    /// <summary>Gets the current Lua state instance.</summary>
    /// <value>The <see cref="Lua"/> state object, or null if not initialized.</value>
    public Lua? LuaState { get; }
    /// <summary>Gets a value indicating whether the CLR package should be available in Lua.</summary>
    /// <value>true to enable CLR package access from Lua; otherwise, false.</value>
    public bool UseCLRPackage { get; }
}