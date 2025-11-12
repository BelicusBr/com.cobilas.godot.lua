using System;

namespace Cobilas.GodotEngine.GDLua;
public enum LuaTypeCode : byte {
	/// <inheritdoc cref="TypeCode.Empty"/>
	Empty = 0,
	/// <inheritdoc cref="TypeCode.Object"/>
	Object = 1,
	/// <inheritdoc cref="TypeCode.DBNull"/>
	DBNull = 2,
	/// <inheritdoc cref="TypeCode.Boolean"/>
	Boolean = 3,
	/// <inheritdoc cref="TypeCode.Char"/>
	Char = 4,
	/// <inheritdoc cref="TypeCode.SByte"/>
	SByte = 5,
	/// <inheritdoc cref="TypeCode.Byte"/>
	Byte = 6,
	/// <inheritdoc cref="TypeCode.Int16"/>
	Int16 = 7,
	/// <inheritdoc cref="TypeCode.UInt16"/>
	UInt16 = 8,
	/// <inheritdoc cref="TypeCode.Int32"/>
	Int32 = 9,
	/// <inheritdoc cref="TypeCode.UInt32"/>
	UInt32 = 10,
	/// <inheritdoc cref="TypeCode.Int64"/>
	Int64 = 11,
	/// <inheritdoc cref="TypeCode.UInt64"/>
	UInt64 = 12,
	/// <inheritdoc cref="TypeCode.Single"/>
	Single = 13,
	/// <inheritdoc cref="TypeCode.Double"/>
	Double = 14,
	/// <inheritdoc cref="TypeCode.Decimal"/>
	Decimal = 15,
	/// <inheritdoc cref="TypeCode.DateTime"/>
	DateTime = 16,
	/// <inheritdoc cref="TypeCode.String"/>
	String = 18,
	Table = 19
}
