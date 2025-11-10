using System;
using System.IO;
using System.Linq;
using MoonSharp.Interpreter;

namespace Cobilas.GodotEngine.GDLua;
public sealed class LuaScriptContainer {
	private Script script;

	private LuaScriptContainer() => script = new();

	public LuaScriptContainer(Stream code) : this() => _ = script.DoStream(code);

	public LuaScriptContainer(string code) : this() => _ = script.DoString(code);

	public void SetField(string? pathField, string? value) => ISetValue(pathField, value);
	public void SetField(string? pathField, bool value) => ISetValue(pathField, value);
	public void SetField(string? pathField, char value) => ISetValue(pathField, value.ToString());
	public void SetField(string? pathField, byte value) => ISetValue(pathField, value);
	public void SetField(string? pathField, sbyte value) => ISetValue(pathField, value);
	public void SetField(string? pathField, int value) => ISetValue(pathField, value);
	public void SetField(string? pathField, uint value) => ISetValue(pathField, value);
	public void SetField(string? pathField, short value) => ISetValue(pathField, value);
	public void SetField(string? pathField, ushort value) => ISetValue(pathField, value);
	public void SetField(string? pathField, long value) => ISetValue(pathField, value);
	public void SetField(string? pathField, ulong value) => ISetValue(pathField, value);
	public void SetField(string? pathField, float value) => ISetValue(pathField, value);
	public void SetField(string? pathField, double value) => ISetValue(pathField, value);
	public void SetField(string? pathField, Table? value) => ISetValue(pathField, value);
	public void SetField(string? pathField, params object[]? value) => ISetValue(pathField, value);

	public string GetFieldToString(string? pathField) => IGetValue<string>(pathField);
	public bool GetFieldToBool(string? pathField) => IGetValue<bool>(pathField);
	public char GetFieldToChar(string? pathField) => IGetValue<string>(pathField).ToCharArray().First();
	public byte GetFieldToByte(string? pathField) => (byte)IGetValue<double>(pathField);
	public sbyte GetFieldToSByte(string? pathField) => (sbyte)IGetValue<double>(pathField);
	public short GetFieldToShort(string? pathField) => (short)IGetValue<double>(pathField);
	public ushort GetFieldToUShort(string? pathField) => (ushort)IGetValue<double>(pathField);
	public int GetFieldToInt(string? pathField) => (int)IGetValue<double>(pathField);
	public uint GetFieldToUInt(string? pathField) => (uint)IGetValue<double>(pathField);
	public long GetFieldToLong(string? pathField) => (long)IGetValue<double>(pathField);
	public ulong GetFieldToULong(string? pathField) => (ulong)IGetValue<double>(pathField);
	public float GetFieldToFloat(string? pathField) => (float)IGetValue<double>(pathField);
	public double GetFieldToDouble(string? pathField) => IGetValue<double>(pathField);
	public Table GetFieldToTable(string? pathField) => IGetValue<Table>(pathField);
	public object[] GetFieldToTupla(string? pathField) {
		if (string.IsNullOrEmpty(pathField)) 
			throw new ArgumentNullException(nameof(pathField), "The argument can be null or empty!");
		return DynValueTuplaToObject(script.Globals.Get(pathField));
	}
	public object GetFieldToObject(string? pathField) {
		if (string.IsNullOrEmpty(pathField))
			throw new ArgumentNullException(nameof(pathField), "The argument can be null or empty!");
		DynValue dyn = script.Globals.Get(pathField);
		return dyn.Type switch {
			DataType.Boolean => dyn.Boolean,
			DataType.Number => DynValueNumberToObject(dyn),
			DataType.String => dyn.String,
			DataType.Table => dyn.Table,
			DataType.Tuple => DynValueTuplaToObject(dyn),
			_ => throw new InvalidCastException($"The type {dyn.Type} conversion is not supported by this method."),
		};
	}

	public object CallFunction(string? pathFunc) => CallFunction(pathFunc, []);

	public object CallFunction(string? pathFunc, params object[]? args) {
		if (string.IsNullOrEmpty(pathFunc)) throw new ArgumentNullException(nameof(pathFunc), "The argument can be null or empty!");
		return script.Call(script.Globals.Get(pathFunc), args ?? []).ToObject();
	}

	private void ISetValue(string? pathField, object? value) {
		if (string.IsNullOrEmpty(pathField)) throw new ArgumentNullException(nameof(pathField), "The argument can be null or empty!");
		else if (value is null) throw new ArgumentNullException(nameof(value));
		if (value is object[] objs) {
			DynValue[] values = new DynValue[objs.LongLength];
			for (long I = 0; I < values.LongLength; I++)
				values[I] = ObjectToDynValue(objs[I]);
			script.Globals.Set(pathField, DynValue.NewTuple(values));
		} else script.Globals.Set(pathField, ObjectToDynValue(value));
	}

	private T IGetValue<T>(string? pathField) {
		if (string.IsNullOrEmpty(pathField)) throw new ArgumentNullException(nameof(pathField), "The argument can be null or empty!");
		return script.Globals.Get(pathField).ToObject<T>();
	}

	public static object DynValueNumberToObject(DynValue? dyn) {
		if (dyn is null) throw new ArgumentNullException(nameof(dyn));
		else if (dyn.Type != DataType.Number)
			throw new InvalidCastException("The input DynValue is not of type DataType.Number!");
		double dbl = dyn.Number;
		if (dbl % 1d != 0 && (dbl - (dbl % 1d)) >= short.MinValue && (dbl - (dbl % 1d)) <= short.MaxValue)
			return (int)dbl;
		if (dbl % 1d != 0 && (dbl - (dbl % 1d)) >= int.MinValue && (dbl - (dbl % 1d)) <= int.MaxValue)
			return (int)dbl;
		if (dbl % 1d != 0 && (dbl - (dbl % 1d)) >= long.MinValue && (dbl - (dbl % 1d)) <= long.MaxValue)
			return (long)dbl;
		return dbl;
	}

	public static object[] DynValueTuplaToObject(DynValue? dyn) {
		if (dyn is null) throw new ArgumentNullException(nameof(dyn));
		else if (dyn.Type != DataType.Tuple)
			throw new InvalidCastException("The input DynValue is not of type DataType.Tuple!");
		DynValue[] tupla = dyn.Tuple;
		object[] result = new object[tupla.LongLength];
		for (long I = 0; I < result.LongLength; I++) {
			if (tupla[I].Type == DataType.Number)
				result[I] = DynValueNumberToObject(tupla[I]);
			else result[I] = tupla[I].ToObject();
		}
		return result;
	}

	public static DynValue ObjectToDynValue(object? value)
		=> value switch {
			null => throw new ArgumentNullException(nameof(value)),
			bool bl => DynValue.NewBoolean(bl),
			string stg => DynValue.NewString(stg),
			byte byt => DynValue.NewNumber(byt),
			sbyte sbyt => DynValue.NewNumber(sbyt),
			short sot => DynValue.NewNumber(sot),
			ushort usot => DynValue.NewNumber(usot),
			int it => DynValue.NewNumber(it),
			uint uit => DynValue.NewNumber(uit),
			long lg => DynValue.NewNumber(lg),
			ulong ulg => DynValue.NewNumber(ulg),
			float flt => DynValue.NewNumber(flt),
			double dbl => DynValue.NewNumber(dbl),
			Table tbl => DynValue.NewTable(tbl),
			_ => throw new InvalidCastException($"The input value type '{value.GetType()}' is not a primitive type or a Table!"),
		};
}