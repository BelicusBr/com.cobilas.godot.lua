using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace Cobilas.GodotEngine.Lua;

public sealed class LuaScriptContainerBuilder {
	private StringBuilder builder;

	private static LuaScriptContainerBuilder containerBuilder = new();

	private LuaScriptContainerBuilder() => builder = new();

	public LuaScriptContainerBuilder AddField(string? name, string? value) => AddField(false, name, value);
	public LuaScriptContainerBuilder AddField(string? name, double? value) => AddField(false, name, value);
	public LuaScriptContainerBuilder AddField(string? name, char? value) => AddField(false, name, value);
	public LuaScriptContainerBuilder AddField(string? name, bool? value) => AddField(false, name, value);

	public LuaScriptContainerBuilder AddLocalField(string? name, string? value) => AddField(true, name, value);
	public LuaScriptContainerBuilder AddLocalField(string? name, double? value) => AddField(true, name, value);
	public LuaScriptContainerBuilder AddLocalField(string? name, char? value) => AddField(true, name, value);
	public LuaScriptContainerBuilder AddLocalField(string? name, bool? value) => AddField(true, name, value);

	public LuaScriptContainerBuilder AddTable(LuaTableItem table) {
		builder.Append(table);
		return this;
	}

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

	public LuaScriptContainerBuilder Spacing() {
		builder.AppendLine();
		return this;
	}

	public LuaScriptContainerBuilder Clear() {
		builder.Clear();
		return this;
	}

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

	public static LuaScriptContainerBuilder Create() => containerBuilder.Clear();
}
