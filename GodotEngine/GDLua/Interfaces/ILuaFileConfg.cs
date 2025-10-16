namespace Cobilas.GodotEngine.GDLua.Interfaces;

public interface ILuaFileConfg : ILuaContainerConfg
{
    public string? FilePath { get; }
    public bool RefreshBuffer { get; }
}
