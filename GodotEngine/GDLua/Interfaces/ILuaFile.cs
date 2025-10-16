using System;

namespace Cobilas.GodotEngine.GDLua.Interfaces;

public interface ILuaFile
{
    LuaField GetField(string pathField);
    void SetField(string pathField, object value);
    object[] InvokeFunction(string methodName, params object[] args);
}
