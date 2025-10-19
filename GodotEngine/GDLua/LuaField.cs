using NLua;
using System;
using System.Linq;

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
    /// <summary>Specifies the data type of the Lua field value.</summary>
    public enum LuaFieldType : byte {
        /// <summary>Represents a null or undefined value.</summary>
        Nil = 0,
        /// <summary>Represents a 32-bit integer value.</summary>
        Integer = 1,
        /// <summary>Represents a 64-bit integer value.</summary>
        LongInteger = 2,
        /// <summary>Represents a floating-point number value.</summary>
        FloatingPoint = 3,
        /// <summary>Represents a boolean value.</summary>
        Boolean = 4,
        /// <summary>Represents a text string value.</summary>
        Text = 5,
        /// <summary>Represents a complex object value.</summary>
        Object = 6
    }
    private bool disposed;
    private object? _value = value;
    private string? _fieldName = fieldName;
    /// <summary>Gets the name of the Lua field</summary>
    /// <value>The name of the field.</value>
    /// <exception cref="ObjectDisposedException">Thrown when the LuaField has been disposed.</exception>
    public readonly string? FieldName => disposed ? throw new ObjectDisposedException(nameof(LuaField)) : _fieldName;
    /// <summary>Gets the value of the Lua field</summary>
    /// <value>The field value as an object.</value>
    /// <exception cref="ObjectDisposedException">Thrown when the LuaField has been disposed.</exception>
    public readonly object? Value => disposed ? throw new ObjectDisposedException(nameof(LuaField)) : _value;
    /// <summary>Gets the data type of the Lua field value.</summary>
    /// <value>The <see cref="LuaFieldType"/> representing the value's data type.</value>
    public readonly LuaFieldType ValueType => GetLuaFieldType(_value);
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
    readonly bool IConvertible.ToBoolean(IFormatProvider provider) => (bool)IConvert(_value, provider);
    /// <inheritdoc/>
    readonly char IConvertible.ToChar(IFormatProvider provider) => ((string)IConvert(_value, provider)).First();
    /// <inheritdoc/>
    readonly DateTime IConvertible.ToDateTime(IFormatProvider provider)
        => DateTime.Parse((string)IConvert(_value, null), provider);
    /// <inheritdoc/>
    readonly string IConvertible.ToString(IFormatProvider provider) => (string)IConvert(_value, provider);
    /// <inheritdoc/>
    readonly byte IConvertible.ToByte(IFormatProvider provider)
        => IConvert(_value, provider) switch {
            double _double => (byte)_double,
            long _long => (byte)_long,
            int _int => (byte)_int,
            _ => throw CastException(nameof(UInt64))
        };
    /// <inheritdoc/>
    readonly decimal IConvertible.ToDecimal(IFormatProvider provider)
        => IConvert(_value, provider) switch {
            double _double => (decimal)_double,
            long _long => _long,
            int _int => _int,
            _ => throw CastException(nameof(UInt64))
        };
    /// <inheritdoc/>
    readonly double IConvertible.ToDouble(IFormatProvider provider)
        => IConvert(_value, provider) switch {
            double _double => _double,
            long _long => _long,
            int _int => _int,
            _ => throw CastException(nameof(UInt64))
        };
    /// <inheritdoc/>
    readonly short IConvertible.ToInt16(IFormatProvider provider)
        => IConvert(_value, provider) switch {
            double _double => (short)_double,
            long _long => (short)_long,
            int _int => (short)_int,
            _ => throw CastException(nameof(UInt64))
        };
    /// <inheritdoc/>
    readonly int IConvertible.ToInt32(IFormatProvider provider)
        => IConvert(_value, provider) switch {
            double _double => (int)_double,
            long _long => (int)_long,
            int _int => _int,
            _ => throw CastException(nameof(UInt64))
        };
    /// <inheritdoc/>
    readonly long IConvertible.ToInt64(IFormatProvider provider)
        => IConvert(_value, provider) switch {
            double _double => (long)_double,
            long _long => _long,
            int _int => _int,
            _ => throw CastException(nameof(UInt64))
        };
    /// <inheritdoc/>
    readonly sbyte IConvertible.ToSByte(IFormatProvider provider)
        => IConvert(_value, provider) switch {
            double _double => (sbyte)_double,
            long _long => (sbyte)_long,
            int _int => (sbyte)_int,
            _ => throw CastException(nameof(UInt64))
        };
    /// <inheritdoc/>
    readonly float IConvertible.ToSingle(IFormatProvider provider)
        => IConvert(_value, provider) switch {
            double _double => (float)_double,
            long _long => _long,
            int _int => _int,
            _ => throw CastException(nameof(UInt64))
        };
    /// <inheritdoc/>
    readonly object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(GetTryValue(_value), conversionType, provider);
    /// <inheritdoc/>
    readonly ushort IConvertible.ToUInt16(IFormatProvider provider)
        => IConvert(_value, provider) switch {
            double _double => (ushort)_double,
            long _long => (ushort)_long,
            int _int => (ushort)_int,
            _ => throw CastException(nameof(UInt64))
        };
    /// <inheritdoc/>
    readonly uint IConvertible.ToUInt32(IFormatProvider provider)
        => IConvert(_value, provider) switch {
            double _double => (uint)_double,
            long _long => (uint)_long,
            int _int => (uint)_int,
            _ => throw CastException(nameof(UInt64))
        };
    /// <inheritdoc/>
    readonly ulong IConvertible.ToUInt64(IFormatProvider provider)
        => IConvert(_value, provider) switch {
            double _double => (ulong)_double,
            long _long => (ulong)_long,
            int _int => (ulong)_int,
            _ => throw CastException(nameof(UInt64))
        };
    // Private methods not documented as requested
    private static object GetTryValue(object? value) => value ?? throw new ArgumentNullException(nameof(value));
    private static LuaFieldType GetLuaFieldType(object? value) 
        => value switch {
            null => LuaFieldType.Nil,
            bool => LuaFieldType.Boolean,
            sbyte or byte or short or ushort => LuaFieldType.Integer,
            int or uint => LuaFieldType.Integer,
            long or ulong => LuaFieldType.LongInteger,
            float or double or decimal => LuaFieldType.FloatingPoint,
            string => LuaFieldType.Text,
            _ => LuaFieldType.Object,
        };
    private static object IConvert(object? value, IFormatProvider? provider)
        => GetLuaFieldType(value) switch {
            LuaFieldType.Boolean => ((IConvertible)value!).ToBoolean(provider),
            LuaFieldType.Integer => ((IConvertible)value!).ToInt32(provider),
            LuaFieldType.LongInteger => ((IConvertible)value!).ToInt64(provider),
            LuaFieldType.FloatingPoint => ((IConvertible)value!).ToDouble(provider),
            LuaFieldType.Text => ((IConvertible)value!).ToString(provider),
            LuaFieldType.Nil => throw new InvalidCastException("Cannot convert null value of LuaField!"),
            _ => CastException(value!.GetType().FullName)
        };
    private static InvalidCastException CastException(string typeName) => new($"Cannot convert LuaField to {typeName}!");
    /// <summary>Converts a LuaField to a LuaTable</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>A LuaTable representation of the field value.</returns>
    /// <exception cref="InvalidCastException">Thrown when the field value is not a LuaTable.</exception>
    public static explicit operator LuaTable(LuaField f) => f._value as LuaTable ?? throw new InvalidCastException($"{nameof(LuaField)} is null");
    /// <summary>Converts a LuaField to its TypeCode</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>The TypeCode of the field value.</returns>
    public static explicit operator TypeCode(LuaField f) => Convert.GetTypeCode(f);
    /// <summary>Converts a LuaField to a boolean value</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>A boolean representation of the field value.</returns>
    public static explicit operator bool(LuaField f) => Convert.ToBoolean(f);
    /// <summary>Converts a LuaField to a DateTime value</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>A DateTime representation of the field value.</returns>
    public static explicit operator DateTime(LuaField f) => Convert.ToDateTime(f);
    /// <summary>Converts a LuaField to a LuaFieldType</summary>
    /// <param name="f">The LuaField to convert.</param>
    /// <returns>The LuaFieldType representing the field value's data type.</returns>
    public static explicit operator LuaFieldType(LuaField f) => LuaField.GetLuaFieldType(f._value);
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