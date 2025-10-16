using NLua;
using System;
using System.IO;
using NLua.Exceptions;
using Cobilas.GodotEngine.Utility.IO;
using Cobilas.GodotEngine.GDLua.Interfaces;

namespace Cobilas.GodotEngine.GDLua;

public sealed class LuaFile : IDisposable, ILuaFile {
    private readonly Lua lua;
    private readonly Archive luaFile;
    private readonly Folder? luaFolder;
    private readonly LuaFileConfg confg;

    public LuaFile(LuaFileConfg confg) {
        this.confg = confg;
        lua = confg.LuaState is not null ? new(confg.LuaState) : new(confg.OpenLibs);
        if (confg.UseCLRPackage)
            lua.LoadCLRPackage();

        if (confg.FilePath is null) throw new ArgumentNullException(nameof(confg.FilePath));
        if ((luaFolder = Folder.Create(GodotPath.GetDirectoryName(confg.FilePath))) == Folder.Null)
            throw new DirectoryNotFoundException(GodotPath.GetDirectoryName(confg.FilePath));
        if ((luaFile = luaFolder.GetArchive(GodotPath.GetFileName(confg.FilePath))) == Archive.Null)
            throw new FileNotFoundException(confg.FilePath);

        _ = lua.DoString(luaFile.Read());
    }
    
    public LuaFile(string filePath, bool refreshBuffer = false) : 
        this(new LuaFileConfg(filePath, null, refreshBuffer, openLibs:true)) { }
    
    public LuaField GetField(string pathField) {
        if (confg.RefreshBuffer) {
            luaFile.RefreshBuffer();
            _ = lua.DoString(luaFile.Read());
        }
        object value = lua[pathField];
        if (value is LuaFunction) throw new LuaException($"{pathField} is {nameof(LuaFunction)}");
        return new(pathField, lua[pathField]);
    }
    
    public void SetField(string pathField, object value) {
        if (confg.RefreshBuffer) {
            luaFile.RefreshBuffer();
            _ = lua.DoString(luaFile.Read());
        }
        lua.SetObjectToPath(pathField, value);
    }

    public object[] InvokeFunction(string methodName, params object[] args) {
        if (confg.RefreshBuffer) {
            luaFile.RefreshBuffer();
            _ = lua.DoString(luaFile.Read());
        }
        return lua[methodName] is LuaFunction lf ? lf.Call(args) : throw new LuaException($"{methodName} is {nameof(LuaField)}");
    }

    public void Dispose() {
        ((IDisposable)lua).Dispose();
        ((IDisposable?)luaFolder)?.Dispose();
    }
}
