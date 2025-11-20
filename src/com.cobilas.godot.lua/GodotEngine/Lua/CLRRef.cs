using System;
using MoonSharp.Interpreter;
using System.Collections.Generic;

namespace Cobilas.GodotEngine.Lua;
/// <summary>Represents a wrapper for converting between Lua tables and CLR objects of a specified type.</summary>
/// <typeparam name="T">The type of CLR object to convert to/from.</typeparam>
public struct CLRRef<T> : IFormattable {
	private Table? inValue;

	private static readonly Dictionary<Type, object> converters = [];
	/// <summary>Converts the wrapped Lua table to a CLR object of type T.</summary>
	/// <returns>The converted CLR object, or default if conversion fails.</returns>
	public readonly T? ToObject() {
		if (CustomConverters.TryGetValue(typeof(T), out var table))
			return (T)table.ToObject(typeof(T).Activator(), inValue);
		return default;
	}
	/// <inheritdoc/>
	public override readonly string ToString() {
		T? result = ToObject();
		return result is null ? string.Empty : result.ToString();
	}
	/// <inheritdoc/>
	public readonly string ToString(string format, IFormatProvider formatProvider) {
		T? result = ToObject();
		return result switch {
			null => string.Empty,
			IFormattable iftb => iftb.ToString(format, formatProvider),
			_ => result.ToString()
		};
	}
	/// <summary>Creates a new <seealso cref="CLRRef{T}"/> instance with the specified Lua table.</summary>
	/// <param name="inValue">The Lua table to wrap.</param>
	/// <returns>A new <seealso cref="CLRRef{T}"/> instance containing the specified table.</returns>
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
	/// <summary>Explicitly converts a <seealso cref="LuaField"/> to a <seealso cref="CLRRef{T}"/>.</summary>
	/// <param name="field">The <seealso cref="LuaField"/> to convert.</param>
	public static explicit operator CLRRef<T>(LuaField field) => Create((Table?)field);
	/// <summary>Implicitly converts a <seealso cref="CLRRef{T}"/> to its underlying CLR object.</summary>
	/// <param name="obj">The <seealso cref="CLRRef{T}"/> to convert.</param>
	public static implicit operator T?(CLRRef<T> obj) => obj.ToObject();
}