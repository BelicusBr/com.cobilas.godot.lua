using NLua;
using System;

namespace Cobilas.GodotEngine.GDLua;

/// <summary>
/// Represents a Lua field with a name and value, providing type conversion capabilities
/// and disposable pattern for resource management.
/// </summary>
/// <remarks>
/// This struct implements <see cref="IConvertible"/> for seamless type conversions
/// and <see cref="IDisposable"/> for proper resource cleanup when working with Lua interop.
/// </remarks>
public struct LuaField(string fieldName, object? value) : IConvertible, IDisposable {
    private bool disposed;
    private object? _value = value;
    private string? _fieldName = fieldName;
    /// <summary>Gets the name of the Lua field</summary>
    /// <value>The name of the field.</value>
    /// <exception cref="ObjectDisposedException">Thrown when the LuaField has been disposed.</exception>
    public readonly string FieldName => _fieldName ?? throw new ObjectDisposedException(nameof(LuaField));
    /// <summary>Gets the value of the Lua field</summary>
    /// <value>The field value as an object.</value>
    /// <exception cref="ObjectDisposedException">Thrown when the LuaField has been disposed.</exception>
    public readonly object? Value => _value ?? throw new ObjectDisposedException(nameof(LuaField));
    /// <summary>Releases all resources used by the LuaField</summary>
    /// <exception cref="ObjectDisposedException">Thrown when the LuaField has already been disposed.</exception>
    public void Dispose() {
        if (disposed) throw new ObjectDisposedException(nameof(LuaField));
        disposed = true;
        _fieldName = null;
        _value = null;
    }
    /// <inheritdoc/>
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
    /// <inheritdoc/>
    readonly bool IConvertible.ToBoolean(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Boolean ?
            bool.Parse(ToString(this, provider)) :
            throw CastException(nameof(Boolean));
    /// <inheritdoc/>
    readonly byte IConvertible.ToByte(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Byte ?
            byte.Parse(GetTryValue(_value).ToString(), provider) :
            throw CastException(nameof(Byte));
    /// <inheritdoc/>
    readonly char IConvertible.ToChar(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Char ?
            char.Parse(ToString(this, provider)) :
            throw CastException(nameof(Char));
    /// <inheritdoc/>
    readonly DateTime IConvertible.ToDateTime(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.DateTime ?
            DateTime.Parse(GetTryValue(_value).ToString(), provider) :
            throw CastException(nameof(DateTime));
    /// <inheritdoc/>
    readonly decimal IConvertible.ToDecimal(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Decimal ?
            decimal.Parse(GetTryValue(_value).ToString(), provider) :
            throw CastException(nameof(Decimal));
    /// <inheritdoc/>
    readonly double IConvertible.ToDouble(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Double ?
            double.Parse(GetTryValue(_value).ToString(), provider) :
            throw CastException(nameof(Double));
    /// <inheritdoc/>
    readonly short IConvertible.ToInt16(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Int16 ?
            short.Parse(GetTryValue(_value).ToString(), provider) :
            throw CastException(nameof(Int16));
    /// <inheritdoc/>
    readonly int IConvertible.ToInt32(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Int32 ?
            int.Parse(GetTryValue(_value).ToString(), provider) :
            throw CastException(nameof(Int32));
    /// <inheritdoc/>
    readonly long IConvertible.ToInt64(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Int64 ?
            long.Parse(GetTryValue(_value).ToString(), provider) :
            throw CastException(nameof(Int64));
    /// <inheritdoc/>
    readonly sbyte IConvertible.ToSByte(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.SByte ?
            sbyte.Parse(GetTryValue(_value).ToString(), provider) :
            throw CastException(nameof(SByte));
    /// <inheritdoc/>
    readonly float IConvertible.ToSingle(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.Single ?
            float.Parse(GetTryValue(_value).ToString(), provider) :
            throw CastException(nameof(Single));
    /// <inheritdoc/>
    readonly string IConvertible.ToString(IFormatProvider provider) => string.Format(provider, "{0}", _value);
    /// <inheritdoc/>
    readonly object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(GetTryValue(_value), conversionType, provider);
    /// <inheritdoc/>
    readonly ushort IConvertible.ToUInt16(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.UInt16 ?
            ushort.Parse(GetTryValue(_value).ToString(), provider) :
            throw CastException(nameof(UInt16));
    /// <inheritdoc/>
    readonly uint IConvertible.ToUInt32(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.UInt32 ?
            uint.Parse(GetTryValue(_value).ToString(), provider) :
            throw CastException(nameof(UInt32));
    /// <inheritdoc/>
    readonly ulong IConvertible.ToUInt64(IFormatProvider provider)
        => GetTypeCode(this) == TypeCode.UInt64 ?
            ulong.Parse(GetTryValue(_value).ToString(), provider) :
            throw CastException(nameof(UInt64));
    // Private methods not documented as requested
    private static object GetTryValue(object? value)
        => value ?? throw new ArgumentNullException(nameof(value));
    private static InvalidCastException CastException(string typeName) => new($"Cannot convert LuaField to {typeName}!");
    private static TypeCode GetTypeCode(LuaField field) => ((IConvertible)field).GetTypeCode();
    private static string ToString(LuaField field, IFormatProvider provider) => ((IConvertible)field).ToString(provider);
    /// <summary>Converts a LuaField to a LuaTable</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>A LuaTable representation of the field value.</returns>
    /// <exception cref="InvalidCastException">Thrown when the field value is not a LuaTable.</exception>
    public static explicit operator LuaTable(LuaField f) => f._value as LuaTable ?? throw new InvalidCastException($"{nameof(LuaField)} is null");
    /// <summary>Converts a LuaField to its TypeCode</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>The TypeCode of the field value.</returns>
    public static explicit operator TypeCode(LuaField f) => Convert.GetTypeCode(f);
    /// <summary>Converts a LuaField to a string</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>A string representation of the field value.</returns>
    public static explicit operator string(LuaField f) => Convert.ToString(f);
    /// <summary>Converts a LuaField to a character</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>A character representation of the field value.</returns>
    public static explicit operator char(LuaField f) => Convert.ToChar(f);
    /// <summary>Converts a LuaField to a single-precision floating-point number</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>A float representation of the field value.</returns>
    public static explicit operator float(LuaField f) => Convert.ToSingle(f);
    /// <summary>Converts a LuaField to a double-precision floating-point number</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>A double representation of the field value.</returns>
    public static explicit operator double(LuaField f) => Convert.ToDouble(f);
    /// <summary>Converts a LuaField to a decimal number</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>A decimal representation of the field value.</returns>
    public static explicit operator decimal(LuaField f) => Convert.ToDecimal(f);
    /// <summary>Converts a LuaField to a byte</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>A byte representation of the field value.</returns>
    public static explicit operator byte(LuaField f) => Convert.ToByte(f);
    /// <summary>Converts a LuaField to an unsigned 16-bit integer</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>An unsigned 16-bit integer representation of the field value.</returns>
    public static explicit operator ushort(LuaField f) => Convert.ToUInt16(f);
    /// <summary>Converts a LuaField to an unsigned 32-bit integer</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>An unsigned 32-bit integer representation of the field value.</returns>
    public static explicit operator uint(LuaField f) => Convert.ToUInt32(f);
    /// <summary>Converts a LuaField to an unsigned 64-bit integer</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>An unsigned 64-bit integer representation of the field value.</returns>
    public static explicit operator ulong(LuaField f) => Convert.ToUInt64(f);
    /// <summary>Converts a LuaField to a signed byte</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>A signed byte representation of the field value.</returns>
    public static explicit operator sbyte(LuaField f) => Convert.ToSByte(f);
    /// <summary>Converts a LuaField to a signed 16-bit integer</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>A signed 16-bit integer representation of the field value.</returns>
    public static explicit operator short(LuaField f) => Convert.ToInt16(f);
    /// <summary>Converts a LuaField to a signed 32-bit integer</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>A signed 32-bit integer representation of the field value.</returns>
    public static explicit operator int(LuaField f) => Convert.ToInt32(f);
    /// <summary>Converts a LuaField to a signed 64-bit integer</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>A signed 64-bit integer representation of the field value.</returns>
    public static explicit operator long(LuaField f) => Convert.ToInt64(f);
}