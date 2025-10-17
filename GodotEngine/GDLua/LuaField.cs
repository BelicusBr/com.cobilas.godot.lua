using NLua;
using System;

namespace Cobilas.GodotEngine.GDLua;

public struct LuaField(string fieldName, object value) : IConvertible, IDisposable {
    private bool disposed;
    private object? _value = value;
    private string? _fieldName = fieldName;

    public readonly string FieldName => _fieldName ?? throw new ObjectDisposedException(nameof(LuaField));
    public readonly object Value => _value ?? throw new ObjectDisposedException(nameof(LuaField));

    public void Dispose() {
        if (disposed) throw new ObjectDisposedException(nameof(LuaField));
        disposed = true;
        _fieldName = null;
        _value = null;
    }
    
    readonly TypeCode IConvertible.GetTypeCode()
        => _value switch {
            bool => TypeCode.Boolean,
            string => TypeCode.String,
            char => TypeCode.Char,
            byte => TypeCode.Byte,
            ushort => TypeCode.UInt16,
            uint => TypeCode.UInt32,
            ulong => TypeCode.UInt64,
            short => TypeCode.Int16,
            int => TypeCode.Int32,
            long => TypeCode.Int64,
            float => TypeCode.Single,
            double => TypeCode.Double,
            decimal => TypeCode.Decimal,
            DateTime => TypeCode.DateTime,
            null => TypeCode.Empty,
            _ => TypeCode.Object,
        };
    readonly bool IConvertible.ToBoolean(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Boolean ?
            bool.Parse(ToString(this, provider)) :
            throw CastException(nameof(Boolean));
    readonly byte IConvertible.ToByte(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Byte ?
            byte.Parse(Value.ToString(), provider) :
            throw CastException(nameof(Byte));
    readonly char IConvertible.ToChar(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Char ?
            char.Parse(ToString(this, provider)) :
            throw CastException(nameof(Char));
    readonly DateTime IConvertible.ToDateTime(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.DateTime ?
            DateTime.Parse(Value.ToString(), provider) :
            throw CastException(nameof(DateTime));
    readonly decimal IConvertible.ToDecimal(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Decimal ?
            decimal.Parse(Value.ToString(), provider) :
            throw CastException(nameof(Decimal));
    readonly double IConvertible.ToDouble(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Double ?
            double.Parse(Value.ToString(), provider) :
            throw CastException(nameof(Double));
    readonly short IConvertible.ToInt16(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Int16 ?
            short.Parse(Value.ToString(), provider) :
            throw CastException(nameof(Int16));
    readonly int IConvertible.ToInt32(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Int32 ?
            int.Parse(Value.ToString(), provider) :
            throw CastException(nameof(Int32));
    readonly long IConvertible.ToInt64(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Int64 ?
            long.Parse(Value.ToString(), provider) :
            throw CastException(nameof(Int64));
    readonly sbyte IConvertible.ToSByte(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.SByte ?
            sbyte.Parse(Value.ToString(), provider) :
            throw CastException(nameof(SByte));
    readonly float IConvertible.ToSingle(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Single ?
            float.Parse(Value.ToString(), provider) :
            throw CastException(nameof(Single));
    readonly string IConvertible.ToString(IFormatProvider provider) => string.Format(provider, "{0}", _value);
    readonly object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Value, conversionType, provider);
    readonly ushort IConvertible.ToUInt16(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.UInt16 ?
            ushort.Parse(Value.ToString(), provider) :
            throw CastException(nameof(UInt16));
    readonly uint IConvertible.ToUInt32(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.UInt32 ?
            uint.Parse(Value.ToString(), provider) :
            throw CastException(nameof(UInt32));
    readonly ulong IConvertible.ToUInt64(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.UInt64 ?
            ulong.Parse(Value.ToString(), provider) :
            throw CastException(nameof(UInt64));

    private static InvalidCastException CastException(string typeName) => new($"Cannot convert LuaField to {typeName}!");
    private static TypeCode GetTypeCode(LuaField field) => ((IConvertible)field).GetTypeCode();
    private static string ToString(LuaField field, IFormatProvider provider) => ((IConvertible)field).ToString(provider);

    public static explicit operator LuaTable(LuaField f) => f._value as LuaTable ?? throw new InvalidCastException($"{nameof(LuaField)} is null");
    public static explicit operator TypeCode(LuaField f) => Convert.GetTypeCode(f);
    public static explicit operator string(LuaField f) => Convert.ToString(f);
    public static explicit operator char(LuaField f) => Convert.ToChar(f);
    public static explicit operator float(LuaField f) => Convert.ToSingle(f);
    public static explicit operator double(LuaField f) => Convert.ToDouble(f);
    public static explicit operator decimal(LuaField f) => Convert.ToDecimal(f);
    public static explicit operator byte(LuaField f) => Convert.ToByte(f);
    public static explicit operator ushort(LuaField f) => Convert.ToUInt16(f);
    public static explicit operator uint(LuaField f) => Convert.ToUInt32(f);
    public static explicit operator ulong(LuaField f) => Convert.ToUInt64(f);
    public static explicit operator sbyte(LuaField f) => Convert.ToSByte(f);
    public static explicit operator short(LuaField f) => Convert.ToInt16(f);
    public static explicit operator int(LuaField f) => Convert.ToInt32(f);
    public static explicit operator long(LuaField f) => Convert.ToInt64(f);
}
