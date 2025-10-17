using NLua;
using System;
using System.Text;
using NLua.Exceptions;
using System.Collections.Generic;
using Cobilas.GodotEngine.GDLua.Interfaces;

namespace Cobilas.GodotEngine.GDLua;

public sealed class LuaContainer : IDisposable, ILuaFile {
    private bool disposed;
    private readonly Lua lua;
    private readonly StringBuilder builder;

    public string Builder => builder.ToString();

    public LuaContainer(LuaContainerConfg confg) {
        lua = confg.LuaState is not null ? new(confg.LuaState) : new(confg.OpenLibs);
        if (confg.UseCLRPackage)
            lua.LoadCLRPackage();
        builder = new();
    }

    public LuaContainer InitField(string pathField, object value) {
        ObjectDisposed(disposed);
        builder.AppendFormat("{0} = {1}\r\n", pathField, value);
        return this;
    }

    public LuaContainer InitLoaclField(string pathField, object value) {
        ObjectDisposed(disposed);
        builder.AppendFormat("local {0} = {1}\r\n", pathField, value);
        return this;
    }
    
    public LuaContainer InitTable(LuaTableItem tables) {
        ObjectDisposed(disposed);
        builder.Append(tables.ToString());
        return this;
    }

    public LuaContainer InitFunction(string funcName, string funcBody, params string[] funcArgs) {
        ObjectDisposed(disposed);
        builder.AppendFormat("function {0}({1})\r\n{2}\r\nend\r\n", funcName, string.Join(", ", funcArgs), funcBody);
        return this;
    }

    public LuaContainer InitCLRPackage(string import) {
        ObjectDisposed(disposed);
        builder.AppendFormat("import('{0}')\r\n", import);
        return this;
    }
    
    public LuaContainer DoString(string value) {
        ObjectDisposed(disposed);
        builder.AppendLine(value);
        return this;
    }
    
    public LuaContainer ClearBuffer() {
        ObjectDisposed(disposed);
        builder.Clear();
        return this;
    }
    
    public LuaContainer FlushToLua() {
        ObjectDisposed(disposed);
        _ = lua.DoString(builder.ToString());
        return this;
    }
    
    public void SetField(string pathField, object value) {
        ObjectDisposed(disposed);
        if (ObjectToLuaTable.TryGetValue(value.GetType(), out ObjectToLuaTable func))
            func.ToLuaTable(value, lua.GetTable(pathField));
        else lua.SetObjectToPath(pathField, value);
    }

    public LuaField GetField(string pathField) {
        ObjectDisposed(disposed);
        object value = lua[pathField];
        if (value is LuaFunction) throw new LuaException($"{pathField} is {nameof(LuaFunction)}");
        return new(pathField, lua[pathField]);
    }

    public object[] InvokeFunction(string methodName, params object[] args) {
        ObjectDisposed(disposed);
        return lua[methodName] is LuaFunction lf ? lf.Call(args) : throw new LuaException($"{methodName} is {nameof(LuaField)}");
    }

    public void Dispose() {
        ObjectDisposed(disposed);
        disposed = true;
        ((IDisposable)lua).Dispose();
        builder.Clear();
        builder.Capacity = 0;
    }

    private static void ObjectDisposed(bool disposed) {
        if (disposed) throw new ObjectDisposedException(nameof(LuaContainer));
    }

    private static void InitTable(StringBuilder builder, string name, int tab, params KeyValuePair<string, object>[] pairs) {
        builder.AppendFormat("{0}{1} = {{\r\n", string.Empty.PadLeft(tab, '\t'), name);
        for (int I = 0; I < pairs.Length; I++) {
            if (pairs[I].Value is KeyValuePair<string, object>[] list)
                InitTable(builder, pairs[I].Key, tab + 1, list);
            else builder.AppendFormat("{0}{1} = {2}{3}\r\n",
                string.Empty.PadLeft(tab + 1, '\t'),
                pairs[I].Key, pairs[I].Value,
                I < pairs.Length - 1 ? "," : string.Empty);
        }
        builder.AppendFormat("{0}}}{1}", string.Empty.PadLeft(tab, '\t'), tab != 0 ? "," : string.Empty).AppendLine();
    }

    public LuaField GetField<T>(string pathField)
    {
        throw new NotImplementedException();
    }

    public void GetFieldToObject<T>(string pathField, ref T value)
    {
        throw new NotImplementedException();
    }
}
