using KeraLua;

namespace Cobilas.GodotEngine.GDLua.Interfaces;

public interface ILuaContainerConfg {
    public bool OpenLibs { get; }
    public Lua? LuaState { get; }
    public bool UseCLRPackage { get; }
}
