using Godot;
using System;
using MoonSharp.Interpreter;
using Cobilas.GodotEngine.Lua;

namespace godot.test {
	[LuaSerializable(typeof(Vector2))]
	public sealed class TableToVec2 : ObjectToLuaTable {
		public override void ToLuaTable(object obj, Table table) {
			Vector2 vector = (Vector2)obj;
			table["x"] = vector.x;
			table["y"] = vector.y;
		}

		public override object ToObject(object obj, Table table) {
			Vector2 vector = (Vector2)obj;
			vector.x = Convert.ToSingle(table["x"]);
			vector.y = Convert.ToSingle(table["y"]);
			return vector;
		}
	}
}
