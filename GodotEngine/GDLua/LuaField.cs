using System;
using MoonSharp.Interpreter;
using System.Collections.Generic;

namespace Cobilas.GodotEngine.GDLua;
public struct LuaField : IConvertible, IDisposable {

	public object? Value { get; private set; }
	public string FieldName { get; private set; }
	public readonly LuaTypeCode LuaTypeCode => GetLuaTypeCode(Value);

	internal LuaField(string fieldName, object? value) {
		Value = value;
		FieldName = fieldName;
	}

	public void Dispose() {
		Value = null;
		FieldName = string.Empty;
	}

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

	readonly bool IConvertible.ToBoolean(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToBoolean(provider),
			_ => false
		};

	readonly char IConvertible.ToChar(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToChar(provider),
			_ => char.MinValue
		};

	readonly sbyte IConvertible.ToSByte(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToSByte(provider),
			_ => 0
		};

	readonly byte IConvertible.ToByte(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToByte(provider),
			_ => 0
		};

	readonly short IConvertible.ToInt16(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToInt16(provider),
			_ => 0
		};

	readonly ushort IConvertible.ToUInt16(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToUInt16(provider),
			_ => 0
		};

	readonly int IConvertible.ToInt32(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToInt32(provider),
			_ => 0
		};

	readonly uint IConvertible.ToUInt32(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToUInt32(provider),
			_ => 0U
		};

	readonly long IConvertible.ToInt64(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToInt64(provider),
			_ => 0L
		};

	readonly ulong IConvertible.ToUInt64(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToUInt64(provider),
			_ => 0UL
		};

	readonly float IConvertible.ToSingle(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToSingle(provider),
			_ => 0f
		};

	readonly double IConvertible.ToDouble(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToDouble(provider),
			_ => 0d
		};

	readonly decimal IConvertible.ToDecimal(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToDecimal(provider),
			_ => 0m
		};

	readonly DateTime IConvertible.ToDateTime(IFormatProvider provider)
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IConvertible icb => icb.ToDateTime(provider),
			_ => DateTime.MinValue
		};

	readonly string IConvertible.ToString(IFormatProvider provider) 
		=> Value switch {
			null => throw new ArgumentNullException(nameof(Value)),
			IFormattable ifp => ifp.ToString(null, provider),
			IConvertible icb => icb.ToString(provider),
			_ => Value.ToString()
		};

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
			_ => LuaTypeCode.Object
		};
	
	public static explicit operator LuaField((string, object?) value) => new(value.Item1, value.Item2);
	public static explicit operator LuaField(Tuple<string, object?> value) => new(value.Item1, value.Item2);
	public static explicit operator LuaField(KeyValuePair<string, object?> value) => new(value.Key, value.Value);

	public static explicit operator string(LuaField field) => Convert.ToString(field);
	public static explicit operator char(LuaField field) => Convert.ToChar(field);
	public static explicit operator bool(LuaField field) => Convert.ToBoolean(field);
	public static explicit operator byte(LuaField field) => Convert.ToByte(field);
	public static explicit operator sbyte(LuaField field) => Convert.ToSByte(field);
	public static explicit operator short(LuaField field) => Convert.ToInt16(field);
	public static explicit operator ushort(LuaField field) => Convert.ToUInt16(field);
	public static explicit operator int(LuaField field) => Convert.ToInt32(field);
	public static explicit operator uint(LuaField field) => Convert.ToUInt32(field);
	public static explicit operator long(LuaField field) => Convert.ToInt64(field);
	public static explicit operator ulong(LuaField field) => Convert.ToUInt64(field);
	public static explicit operator float(LuaField field) => Convert.ToSingle(field);
	public static explicit operator double(LuaField field) => Convert.ToDouble(field);
	public static explicit operator decimal(LuaField field) => Convert.ToDecimal(field);
	public static explicit operator DateTime(LuaField field) => Convert.ToDateTime(field);

	public static explicit operator LuaTypeCode(LuaField field) => field.LuaTypeCode;
	public static explicit operator Table?(LuaField field) {
		if (field.LuaTypeCode != LuaTypeCode.Table)
			throw new InvalidCastException($"LuaField is not of type {nameof(Table)}!");
		return field.Value as Table;
	}
}