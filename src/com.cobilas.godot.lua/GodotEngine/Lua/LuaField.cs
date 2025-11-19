using System;
using System.Globalization;
using MoonSharp.Interpreter;
using System.Collections.Generic;
using Cobilas.GodotEngine.Lua.Exceptions;

namespace Cobilas.GodotEngine.Lua;

public struct LuaField : IConvertible, IDisposable {

	public object? Value { get; private set; }
	public string FieldName { get; private set; }
	public readonly LuaTypeCode LuaTypeCode => GetLuaTypeCode(Value);
	public readonly Type FieldType => Value is null ? NullObject.Null.GetType() : Value.GetType();

	internal LuaField(string fieldName, object? value) {
		Value = value;
		FieldName = fieldName;
	}

	public readonly void ToObject<T>(ref T value) {
		if (!CustomConverters.TryGetValue(typeof(T), out ObjectToLuaTable table)) return;
		value = (T)table.ToObject(value, (Table?)this);
	}
	/// <inheritdoc/>
	public void Dispose() {
		Value = null;
		FieldName = string.Empty;
	}
	/// <inheritdoc/>
	readonly TypeCode IConvertible.GetTypeCode()
		=> Value switch {
			null => TypeCode.Empty,
			string => TypeCode.String,
			char => TypeCode.Char,
			bool => TypeCode.Boolean,
			byte => TypeCode.Byte,
			sbyte => TypeCode.SByte,
			short => TypeCode.Int16,
			ushort => TypeCode.UInt16,
			int => TypeCode.Int32,
			uint => TypeCode.UInt32,
			long => TypeCode.Int64,
			ulong => TypeCode.UInt64,
			float => TypeCode.Single,
			double => TypeCode.Double,
			decimal => TypeCode.Decimal,
			DateTime => TypeCode.DateTime,
			_ => TypeCode.Object
		};
	/// <inheritdoc/>
	readonly bool IConvertible.ToBoolean(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToBoolean(provider),
			_ => false
		};
	/// <inheritdoc/>
	readonly char IConvertible.ToChar(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToChar(provider),
			_ => char.MinValue
		};
	/// <inheritdoc/>
	readonly sbyte IConvertible.ToSByte(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToSByte(provider),
			_ => 0
		};
	/// <inheritdoc/>
	readonly byte IConvertible.ToByte(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToByte(provider),
			_ => 0
		};
	/// <inheritdoc/>
	readonly short IConvertible.ToInt16(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToInt16(provider),
			_ => 0
		};
	/// <inheritdoc/>
	readonly ushort IConvertible.ToUInt16(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToUInt16(provider),
			_ => 0
		};
	/// <inheritdoc/>
	readonly int IConvertible.ToInt32(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToInt32(provider),
			_ => 0
		};
	/// <inheritdoc/>
	readonly uint IConvertible.ToUInt32(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToUInt32(provider),
			_ => 0U
		};
	/// <inheritdoc/>
	readonly long IConvertible.ToInt64(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToInt64(provider),
			_ => 0L
		};
	/// <inheritdoc/>
	readonly ulong IConvertible.ToUInt64(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToUInt64(provider),
			_ => 0UL
		};
	/// <inheritdoc/>
	readonly float IConvertible.ToSingle(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToSingle(provider),
			_ => 0f
		};
	/// <inheritdoc/>
	readonly double IConvertible.ToDouble(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToDouble(provider),
			_ => 0d
		};
	/// <inheritdoc/>
	readonly decimal IConvertible.ToDecimal(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToDecimal(provider),
			_ => 0m
		};
	/// <inheritdoc/>
	readonly DateTime IConvertible.ToDateTime(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToDateTime(provider),
			_ => DateTime.MinValue
		};
	/// <inheritdoc/>
	readonly string IConvertible.ToString(IFormatProvider provider) 
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IFormattable ifp => ifp.ToString(null, provider),
			IConvertible icb => icb.ToString(provider),
			_ => Value.ToString()
		};
	/// <inheritdoc/>
	readonly object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		=> Convert.ChangeType(
			Value ?? throw new ArgumentNullException(nameof(Value)),
			conversionType, provider
		);

	public static LuaTypeCode GetLuaTypeCode(object? value)
		=> value switch {
			null => LuaTypeCode.Empty,
			string => LuaTypeCode.String,
			char => LuaTypeCode.Char,
			bool => LuaTypeCode.Boolean,
			byte => LuaTypeCode.Byte,
			sbyte => LuaTypeCode.SByte,
			short => LuaTypeCode.Int16,
			ushort => LuaTypeCode.UInt16,
			int => LuaTypeCode.Int32,
			uint => LuaTypeCode.UInt32,
			long => LuaTypeCode.Int64,
			ulong => LuaTypeCode.UInt64,
			float => LuaTypeCode.Single,
			double => LuaTypeCode.Double,
			decimal => LuaTypeCode.Decimal,
			DateTime => LuaTypeCode.DateTime,
			Table => LuaTypeCode.Table,
			Closure => LuaTypeCode.Function,
			UserData => LuaTypeCode.UserData,
			CallbackFunction => LuaTypeCode.ClrFunction,
			TailCallData => LuaTypeCode.TailCallRequest,
			Coroutine => LuaTypeCode.Thread,
			YieldRequest => LuaTypeCode.YieldRequest,
			_ => LuaTypeCode.Object
		};

	public static LuaTypeCode GetLuaTypeCode(LuaField field) => GetLuaTypeCode(field.Value);

	public static explicit operator LuaField((string, object?) value) => new(value.Item1, value.Item2);
	public static explicit operator LuaField(Tuple<string, object?> value) => new(value.Item1, value.Item2);
	public static explicit operator LuaField(KeyValuePair<string, object?> value) => new(value.Key, value.Value);

	public static explicit operator string(LuaField field) => Convert.ToString(field, CultureInfo.InvariantCulture);
	public static explicit operator char(LuaField field) => Convert.ToChar(field, CultureInfo.InvariantCulture);
	public static explicit operator bool(LuaField field) => Convert.ToBoolean(field, CultureInfo.InvariantCulture);
	public static explicit operator byte(LuaField field) => Convert.ToByte(field, CultureInfo.InvariantCulture);
	public static explicit operator sbyte(LuaField field) => Convert.ToSByte(field, CultureInfo.InvariantCulture);
	public static explicit operator short(LuaField field) => Convert.ToInt16(field, CultureInfo.InvariantCulture);
	public static explicit operator ushort(LuaField field) => Convert.ToUInt16(field, CultureInfo.InvariantCulture);
	public static explicit operator int(LuaField field) => Convert.ToInt32(field, CultureInfo.InvariantCulture);
	public static explicit operator uint(LuaField field) => Convert.ToUInt32(field, CultureInfo.InvariantCulture);
	public static explicit operator long(LuaField field) => Convert.ToInt64(field, CultureInfo.InvariantCulture);
	public static explicit operator ulong(LuaField field) => Convert.ToUInt64(field, CultureInfo.InvariantCulture);
	public static explicit operator float(LuaField field) => Convert.ToSingle(field, CultureInfo.InvariantCulture);
	public static explicit operator double(LuaField field) => Convert.ToDouble(field, CultureInfo.InvariantCulture);
	public static explicit operator decimal(LuaField field) => Convert.ToDecimal(field, CultureInfo.InvariantCulture);
	public static explicit operator DateTime(LuaField field) => Convert.ToDateTime(field, CultureInfo.InvariantCulture);

	public static explicit operator LuaTypeCode(LuaField field) => field.LuaTypeCode;
	public static explicit operator Table(LuaField field) => IConvert<Table>(field);
	public static explicit operator Closure(LuaField field) => IConvert<Closure>(field);
	public static explicit operator CallbackFunction(LuaField field) => IConvert<CallbackFunction>(field);
	public static explicit operator Coroutine(LuaField field) => IConvert<Coroutine>(field);
	public static explicit operator TailCallData(LuaField field) => IConvert<TailCallData>(field);
	public static explicit operator UserData(LuaField field) => IConvert<UserData>(field);
	public static explicit operator YieldRequest(LuaField field) => IConvert<YieldRequest>(field);

	private static object IConvert(LuaField field)
		=> field.LuaTypeCode switch {
			LuaTypeCode.Empty => throw new ArgumentNullException(nameof(field), "The input value is empty!"),
			LuaTypeCode.Table => (Table)field.Value!,
			LuaTypeCode.Function => (Closure)field.Value!,
			LuaTypeCode.UserData => (UserData)field.Value!,
			LuaTypeCode.ClrFunction => (CallbackFunction)field.Value!,
			LuaTypeCode.TailCallRequest => (TailCallData)field.Value!,
			LuaTypeCode.Thread => (Coroutine)field.Value!,
			LuaTypeCode.YieldRequest => (YieldRequest)field.Value!,
			_ => throw new LuaException($"It is not possible to explicitly convert the type '{field.FieldType}'; use the '{nameof(LuaField)}.{nameof(ToObject)}' method for conversion if the object has a custom '{nameof(ObjectToLuaTable)}' converter!")
		};
	private static TypeValue IConvert<TypeValue>(LuaField field)
		=> (TypeValue)IConvert(field);
}