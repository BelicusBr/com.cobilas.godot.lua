using NLua;
using System;
using System.Text;
using System.Collections.Generic;
using Cobilas.GodotEngine.GDLua.Interfaces;

namespace Cobilas.GodotEngine.GDLua;

public sealed class LuaContainer : IDisposable, ILuaFile {
    private readonly Lua lua;
    private readonly StringBuilder builder;

    public StringBuilder Builder => builder;

    public LuaContainer(LuaContainerConfg confg) {
        lua = confg.LuaState is not null ? new(confg.LuaState) : new(confg.OpenLibs);
        if (confg.UseCLRPackage)
            lua.LoadCLRPackage();
        builder = new();
    }

    public LuaContainer SetField(string pathField, object value) {
        ((ILuaFile)this).SetField(pathField, value);
        return this;
    }

    public LuaContainer SetLoaclField(string pathField, object value) {
        builder.AppendFormat("local {0} = {1}\r\n", pathField, value);
        return this;
    }
    
    public LuaContainer SetTable(string pathField, params KeyValuePair<string, object>[] pairs) {
        SetTable(builder, pathField, 0, pairs);
        return this;
    }

    public LuaContainer SetFunction(string funcName, string funcBody, params string[] funcArgs) {
        builder.AppendFormat("function {0}({1})\r\n{2}\r\nend\r\n", funcName, string.Join(",", funcArgs), funcBody);
        return this;
    }

    public LuaContainer SetCLRPackage(string import) {
        builder.AppendFormat("import('{0}')\r\n", import);
        return this;
    }
    
    public LuaContainer DoString(string value) {
        builder.AppendLine(value);
        return this;
    }
    
    public LuaContainer ClearBuffer() {
        builder.Clear();
        return this;
    }
    
    public LuaContainer FlushToLua() {
        _ = lua.DoString(builder.ToString());
        return this;
    }

    public LuaField GetField(string pathField)
    {
        throw new NotImplementedException();
    }

    public object[] InvokeFunction(string methodName, params object[] args)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        ((IDisposable)lua).Dispose();
        builder.Clear();
        builder.Capacity = 0;
    }

    void ILuaFile.SetField(string pathField, object value)
        => builder.AppendFormat("{0} = {1}\r\n", pathField, value);

    private static void SetTable(StringBuilder builder, string name, int tab, params KeyValuePair<string, object>[] pairs) {
        builder.AppendFormat("{0}{1} = {{\r\n", string.Empty.PadLeft(tab, '\t'), name);
        for (int I = 0; I < pairs.Length; I++) {
            if (pairs[I].Value is KeyValuePair<string, object>[] list)
                SetTable(builder, pairs[I].Key, tab + 1, list);
            else builder.AppendFormat("{0}{1} = {2}{3}\r\n",
                string.Empty.PadLeft(tab + 1, '\t'),
                pairs[I].Key, pairs[I].Value,
                I < pairs.Length - 1 ? "," : string.Empty);
        }
        builder.AppendFormat("{0}}}{1}", string.Empty.PadLeft(tab, '\t'), tab != 0 ? "," : string.Empty).AppendLine();
    }
}
