using System;
using MoonSharp.Interpreter;
using System.Collections.Generic;

namespace Cobilas.GodotEngine.GDLua;

public struct LuaFieldToObject<T> {
	private Table? inValue;

	private static readonly Dictionary<Type, object> converters = [];
	
	public T? ToObject() {
		if (ObjectToLuaTable.TryGetValue(typeof(T), out var table))
			return (T)table.ToObject(typeof(T).Activator(), inValue);
		return default;
	}

	public static LuaFieldToObject<T> Create(Table? inValue) {
		LuaFieldToObject<T> result;
		if (converters.TryGetValue(typeof(T), out object value)) {
			result = (LuaFieldToObject<T>)value;
			result.inValue = inValue;
			return result;
		}
		converters.Add(typeof(T), result = new());
		result.inValue = inValue;
		return result;
	}

	public static explicit operator LuaFieldToObject<T>(LuaField field) => Create((Table?)field);
	public static implicit operator T?(LuaFieldToObject<T> obj) => obj.ToObject();
}
