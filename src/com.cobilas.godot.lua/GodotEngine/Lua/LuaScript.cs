using System;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace Cobilas.GodotEngine.Lua;

public abstract class LuaScript : IDisposable {

	public abstract IScriptLoader ScriptLoader { get; set; }
	public abstract Func<string, string> DebugInput { get; set; }
	public abstract Action<string> DebugPrint { get; set; }

	public abstract LuaField GetField(string? pathField);
	public abstract LuaField[] GetTuplaField(string[]? pathFields);
	public abstract LuaFunc GetFunction(string? pathFunction);

	public abstract void SetField(string? pathField, object? value);
	public abstract void SetFunction(string? pathFunction, Delegate? value);
	public abstract void SetTuplaField(string[]? pathFields, object[]? value);

	public abstract Table CreateTable(string? pathField);

	protected abstract void Dispose(bool disposing);
	/// <inheritdoc/>
	public void Dispose() {
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public static object? DynValueToObject(DynValue? dyn) {
		if (dyn is null) throw new ArgumentNullException(nameof(dyn));
		return dyn.Type switch {
			DataType.Boolean => dyn.Boolean,
			DataType.Number => DynValueNumberToObject(dyn),
			DataType.String => dyn.String,
			DataType.Table => dyn.Table,
			DataType.Tuple => DynValueTuplaToObject(dyn),
			DataType.Function => dyn.Function,
			DataType.UserData => dyn.UserData,
			DataType.ClrFunction => dyn.Callback,
			DataType.TailCallRequest => dyn.TailCallData,
			DataType.Thread => dyn.Coroutine,
			DataType.YieldRequest => dyn.YieldRequest,
			_ => throw new InvalidCastException($"The type {dyn.Type} conversion is not supported by this method."),
		};
	}

	public static object? DynValueNumberToObject(DynValue? dyn) {
		if (dyn is null) throw new ArgumentNullException(nameof(dyn));
		else if (dyn.Type != DataType.Number)
			throw new InvalidCastException("The input DynValue is not of type DataType.Number!");
		double value = dyn.Number;
		double module = value % 1d;
		double integral = value - module;
		if (module == 0 && integral >= short.MinValue && integral <= short.MaxValue)
			return (int)value;
		if (module == 0 && integral >= int.MinValue && integral <= int.MaxValue)
			return (int)value;
		if (module == 0 && integral >= long.MinValue && integral <= long.MaxValue)
			return (long)value;
		return value;
	}

	public static object?[] DynValueTuplaToObject(DynValue? dyn) {
		if (dyn is null) throw new ArgumentNullException(nameof(dyn));
		else if (dyn.Type != DataType.Tuple)
			throw new InvalidCastException("The input DynValue is not of type DataType.Tuple!");
		DynValue[] tupla = dyn.Tuple;
		object?[] result = new object[tupla.LongLength];
		for (long I = 0; I < result.LongLength; I++) {
			if (tupla[I].Type == DataType.Number)
				result[I] = DynValueNumberToObject(tupla[I]);
			else result[I] = tupla[I].ToObject();
		}
		return result;
	}

	public static DynValue? ObjectToDynValue(Script? script, object? value)
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
			Coroutine crt => DynValue.NewCoroutine(crt),
			CallbackFunction cbf => DynValue.NewCallback(cbf),
			Closure clr => DynValue.NewClosure(clr),
			TailCallData tcd => DynValue.NewTailCallReq(tcd),
			UserData udt => DynValue.NewUserData(udt),
			_ => ObjectToTable(script ?? throw new ArgumentNullException(nameof(script)), value)
		};

	private static DynValue ObjectToTable(Script script, object value) {
		if (CustomConverters.TryGetValue(value.GetType(), out ObjectToLuaTable table)) {
			Table result = new(script);
			table.ToLuaTable(value, result);
			return DynValue.NewTable(result);
		}
		throw new InvalidCastException($"The type '{value.GetType()}' cannot be converted to a CLR object!");
	}
}
