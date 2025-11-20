using System;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace Cobilas.GodotEngine.Lua;
/// <summary>Represents a base class for Lua script handling in Godot Engine.</summary>
public abstract class LuaScript : IDisposable {
	/// <summary>Gets or sets the script loader for Lua script resolution.</summary>
	/// <returns>The script loader instance.</returns>
	/// <value>The new script loader instance.</value>
	public abstract IScriptLoader ScriptLoader { get; set; }
	/// <summary>Gets or sets the debug input handler for Lua scripts.</summary>
	/// <returns>The debug input function.</returns>
	/// <value>The new debug input function.</value>
	public abstract Func<string, string> DebugInput { get; set; }
	/// <summary>Gets or sets the debug print handler for Lua scripts.</summary>
	/// <returns>The debug print action.</returns>
	/// <value>The new debug print action.</value>
	public abstract Action<string> DebugPrint { get; set; }
	/// <summary>Retrieves a field value from the Lua script.</summary>
	/// <param name="pathField">The path to the field in the Lua script.</param>
	/// <returns>A LuaField representing the requested field.</returns>
	public abstract LuaField GetField(string? pathField);
	/// <summary>Retrieves multiple field values from the Lua script.</summary>
	/// <param name="pathFields">The paths to the fields in the Lua script.</param>
	/// <returns>An array of LuaField objects representing the requested fields.</returns>
	public abstract LuaField[] GetTuplaField(string[]? pathFields);
	/// <summary>Retrieves a function from the Lua script.</summary>
	/// <param name="pathFunction">The path to the function in the Lua script.</param>
	/// <returns>A LuaFunc representing the requested function.</returns>
	public abstract LuaFunc GetFunction(string? pathFunction);
	/// <summary>Sets a field value in the Lua script.</summary>
	/// <param name="pathField">The path to the field in the Lua script.</param>
	/// <param name="value">The value to set.</param>
	public abstract void SetField(string? pathField, object? value);
	/// <summary>Sets a function in the Lua script.</summary>
	/// <param name="pathFunction">The path to the function in the Lua script.</param>
	/// <param name="value">The delegate to set as the function.</param>
	public abstract void SetFunction(string? pathFunction, Delegate? value);
	/// <summary>Sets multiple field values in the Lua script.</summary>
	/// <param name="pathFields">The paths to the fields in the Lua script.</param>
	/// <param name="value">The values to set.</param>
	public abstract void SetTuplaField(string[]? pathFields, object[]? value);
	/// <summary>Creates a new table in the Lua script.</summary>
	/// <param name="pathField">The path where the table should be created.</param>
	/// <returns>The newly created Lua table.</returns>
	public abstract Table CreateTable(string? pathField);
	/// <summary>Disposes resources used by the Lua script.</summary>
	/// <param name="disposing">True if called from <seealso cref="Dispose()"/>, false if from finalizer.</param>
	protected abstract void Dispose(bool disposing); 
	/// <inheritdoc/>
	public void Dispose() {
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
	/// <summary>Converts a <seealso cref="DynValue"/> to a CLR object.</summary>
	/// <param name="dyn">The <seealso cref="DynValue"/> to convert.</param>
	/// <returns>The converted CLR object.</returns>
	/// <exception cref="ArgumentNullException">Thrown when dyn is null.</exception>
	/// <exception cref="InvalidCastException">Thrown when the <seealso cref="DynValue"/> type is not supported.</exception>
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
	/// <summary>Converts a numeric <seealso cref="DynValue"/> to an appropriate CLR numeric type.</summary>
	/// <param name="dyn">The numeric <seealso cref="DynValue"/> to convert.</param>
	/// <returns>The converted numeric value as int, long, or double.</returns>
	/// <exception cref="ArgumentNullException">Thrown when dyn is null.</exception>
	/// <exception cref="InvalidCastException">Thrown when the <seealso cref="DynValue"/> is not of type Number.</exception>
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
	/// <summary>Converts a tuple <seealso cref="DynValue"/> to an array of CLR objects.</summary>
	/// <param name="dyn">The tuple <seealso cref="DynValue"/> to convert.</param>
	/// <returns>An array of converted CLR objects.</returns>
	/// <exception cref="ArgumentNullException">Thrown when dyn is null.</exception>
	/// <exception cref="InvalidCastException">Thrown when the <seealso cref="DynValue"/> is not of type Tuple.</exception>
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
	/// <summary>Converts a CLR object to a <seealso cref="DynValue"/>.</summary>
	/// <param name="script">The Lua script context.</param>
	/// <param name="value">The CLR object to convert.</param>
	/// <returns>The converted <seealso cref="DynValue"/>.</returns>
	/// <exception cref="ArgumentNullException">Thrown when value or script is null.</exception>
	/// <exception cref="InvalidCastException">Thrown when the object type cannot be converted.</exception>
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