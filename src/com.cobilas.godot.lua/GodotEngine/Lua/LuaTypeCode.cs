using MoonSharp.Interpreter;
using System;

namespace Cobilas.GodotEngine.Lua;
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
	/// <inheritdoc cref="DataType.Table"/>
	Table = 19,
	/// <inheritdoc cref="DataType.Function"/>
	Function = 20,
	/// <inheritdoc cref="DataType.UserData"/>
	UserData = 21,
	/// <inheritdoc cref="DataType.ClrFunction"/>
	ClrFunction = 22,
	/// <inheritdoc cref="DataType.TailCallRequest"/>
	TailCallRequest = 23,
	/// <inheritdoc cref="DataType.Thread"/>
	Thread = 24,
	/// <inheritdoc cref="DataType.YieldRequest"/>
	YieldRequest = 25
}
