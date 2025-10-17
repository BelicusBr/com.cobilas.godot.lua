using System;

namespace Cobilas.GodotEngine.GDLua.Interfaces;

public interface ILuaFile
{
    LuaField GetField(string pathField);
    LuaField GetField<T>(string pathField);
    void GetFieldToObject<T>(string pathField, ref T value);
    void SetField(string pathField, object value);
    object[] InvokeFunction(string methodName, params object[] args);
}
