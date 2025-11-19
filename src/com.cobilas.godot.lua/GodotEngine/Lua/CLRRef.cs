using System;
using MoonSharp.Interpreter;
using System.Collections.Generic;

namespace Cobilas.GodotEngine.Lua;

public struct CLRRef<T> : IFormattable {
	private Table? inValue;

	private static readonly Dictionary<Type, object> converters = [];
	
	public T? ToObject() {
		if (CustomConverters.TryGetValue(typeof(T), out var table))
			return (T)table.ToObject(typeof(T).Activator(), inValue);
		return default;
	}
	
	public override string ToString() {
		T? result = ToObject();
		return result is null ? string.Empty : result.ToString();
	}
	/// <inheritdoc/>
	public string ToString(string format, IFormatProvider formatProvider) {
		T? result = ToObject();
		return result switch {
			null => string.Empty,
			IFormattable iftb => iftb.ToString(format, formatProvider),
			_ => result.ToString()
		};
	}

	public static CLRRef<T> Create(Table? inValue) {
		CLRRef<T> result;
		if (converters.TryGetValue(typeof(T), out object value)) {
			result = (CLRRef<T>)value;
			result.inValue = inValue;
			return result;
		}
		converters.Add(typeof(T), result = new());
		result.inValue = inValue;
		return result;
	}

	public static explicit operator CLRRef<T>(LuaField field) => Create((Table?)field);
	public static implicit operator T?(CLRRef<T> obj) => obj.ToObject();
}
