using System;
using System.Text;

namespace Cobilas.GodotEngine.Lua;
/// <summary>Provides a builder pattern for constructing Lua script containers with fields, tables, and functions.</summary>
/// <remarks>
/// This class simplifies the creation of Lua scripts by providing a fluent interface
/// for adding various types of fields, tables, and functions to a script container.
/// </remarks>
public sealed class LuaScriptContainerBuilder {
	private readonly StringBuilder builder;

	private static readonly LuaScriptContainerBuilder containerBuilder = new();

	private LuaScriptContainerBuilder() => builder = new();
	/// <summary>Adds a global string field to the Lua script.</summary>
	/// <param name="name">The name of the field.</param>
	/// <param name="value">The string value of the field.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	public LuaScriptContainerBuilder AddField(string? name, string? value) => AddField(false, name, value);
	/// <summary>Adds a global numeric field to the Lua script.</summary>
	/// <param name="name">The name of the field.</param>
	/// <param name="value">The numeric value of the field.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	public LuaScriptContainerBuilder AddField(string? name, double? value) => AddField(false, name, value);
	/// <summary>Adds a global character field to the Lua script.</summary>
	/// <param name="name">The name of the field.</param>
	/// <param name="value">The character value of the field.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	public LuaScriptContainerBuilder AddField(string? name, char? value) => AddField(false, name, value);
	/// <summary>Adds a global boolean field to the Lua script.</summary>
	/// <param name="name">The name of the field.</param>
	/// <param name="value">The boolean value of the field.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	public LuaScriptContainerBuilder AddField(string? name, bool? value) => AddField(false, name, value);
	/// <summary>Adds a local string field to the Lua script.</summary>
	/// <param name="name">The name of the field.</param>
	/// <param name="value">The string value of the field.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	public LuaScriptContainerBuilder AddLocalField(string? name, string? value) => AddField(true, name, value);
	/// <summary>Adds a local numeric field to the Lua script.</summary>
	/// <param name="name">The name of the field.</param>
	/// <param name="value">The numeric value of the field.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	public LuaScriptContainerBuilder AddLocalField(string? name, double? value) => AddField(true, name, value);
	/// <summary>Adds a local character field to the Lua script.</summary>
	/// <param name="name">The name of the field.</param>
	/// <param name="value">The character value of the field.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	public LuaScriptContainerBuilder AddLocalField(string? name, char? value) => AddField(true, name, value);
	/// <summary>Adds a local boolean field to the Lua script.</summary>
	/// <param name="name">The name of the field.</param>
	/// <param name="value">The boolean value of the field.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	public LuaScriptContainerBuilder AddLocalField(string? name, bool? value) => AddField(true, name, value);
	/// <summary>Adds a Lua table to the script container.</summary>
	/// <param name="table">The table item to add to the script.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	public LuaScriptContainerBuilder AddTable(LuaTableItem table) {
		builder.Append(table);
		return this;
	}
	/// <summary>Adds a function definition to the Lua script.</summary>
	/// <param name="name">The name of the function.</param>
	/// <param name="body">The body of the function as a string.</param>
	/// <param name="args">The parameter names for the function.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentNullException">Thrown when name is null or empty.</exception>
	public LuaScriptContainerBuilder AddFunction(string? name, string? body, params string[]? args) {
		if (name is null || string.IsNullOrEmpty(name))
			throw new ArgumentNullException(nameof(name), $"The input value '{nameof(name)}' is null or empty!");
		body ??= string.Empty;
		args ??= [];
		builder.AppendFormat("function {0}(", name)
			.AppendFormat("{0})\r\n", string.Join(", ", args))
			.AppendLine(body)
			.AppendLine("end");
		return this;
	}
	/// <summary>Adds a blank line for spacing in the generated script.</summary>
	/// <returns>The current builder instance for method chaining.</returns>
	public LuaScriptContainerBuilder Spacing() {
		builder.AppendLine();
		return this;
	}
	/// <summary>Clears the current script content.</summary>
	/// <returns>The current builder instance for method chaining.</returns>
	public LuaScriptContainerBuilder Clear() {
		builder.Clear();
		return this;
	}
	/// <summary>Returns the generated Lua script as a string.</summary>
	/// <returns>The complete Lua script as a string.</returns>
	public override string ToString() => builder.ToString();

	private LuaScriptContainerBuilder AddField(bool isLocal, string? name, object? value) {
		if (name is null || string.IsNullOrEmpty(name))
			throw new ArgumentNullException(nameof(name), $"The input value '{nameof(name)}' is null or empty!");
		else if (value is null) throw new ArgumentNullException(nameof(value));
		value = value switch {
			string stg => $"\"{stg}\"",
			char ch => $"'{ch}'",
			bool bl => bl.ToString().ToLower(),
			_ => value
		};
		builder.AppendFormat("{0}{1} = {2}\r\n", isLocal ? "local " : string.Empty, name, value);
		return this;
	}
	/// <summary>Creates a new instance of the <see cref="LuaScriptContainerBuilder"/> with cleared content.</summary>
	/// <returns>A new <see cref="LuaScriptContainerBuilder"/> instance ready for use.</returns>
	public static LuaScriptContainerBuilder Create() => containerBuilder.Clear();
}