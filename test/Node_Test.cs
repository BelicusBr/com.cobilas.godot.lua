using Godot;
using System;
using MoonSharp.Interpreter;
using Cobilas.GodotEngine.Lua;
using Cobilas.GodotEngine.Utility;
using Cobilas.GodotEngine.Utility.IO;

public class Node_Test : Node2D
{

	[Export] private NodePath nodePath;
	private Label label;
	private LuaScriptFile file;

	public override void _Ready() {
		try
		{
			label = GetNode<Label>(nodePath);
			label.SelfModulate = Color.Color8(255, 255, 255);
			file = new LuaScriptFile(GodotPath.GlobalizePath("res://luatest.lua"));

			label.AppendLine($"<{nameof(LuaScriptFile)}>");
			label.AppendLine();

			file.SetField("value55", new Vector2(224f, 441f));
			file.SetFunction(nameof(func_test), (Func<float, float, float, float>)func_test);

			label.AppendFormat("value0:{0}\r\n", (short)file.GetField("value0"));
			label.AppendFormat("value1:{0}\r\n", (float)file.GetField("value1"));
			label.AppendFormat("value2:{0}\r\n", (long)file.GetField("value2"));
			label.AppendFormat("value3:{0}\r\n", (double)file.GetField("value3"));
			label.AppendFormat("value6:{0}\r\n", (string)file.GetField("value6"));
			label.AppendFormat("value7:{0}\r\n", (bool)file.GetField("value7"));
			label.AppendFormat("value8:{0}\r\n", (Table)file.GetField("value8"));
			label.AppendFormat("value55:{0}\r\n", (CLRRef<Vector2>)file.GetField("value55"));
			label.AppendFormat("value8:{0}\r\n", (CLRRef<Vector2>)file.GetField("value8"));
			label.AppendFormat("value9:{0}\r\n", (char)file.GetField("value9"));
			label.AppendFormat("{0}:{1}\r\n", nameof(func_test), file.GetField(nameof(func_test)).LuaTypeCode);
			label.AppendFormat("{0}2:{1}\r\n", nameof(func_test), file.GetField($"{nameof(func_test)}2").LuaTypeCode);

			label.AppendFormat("{0}.call:{1}\r\n", nameof(func_test), file.GetFunction(nameof(func_test)).Call(25, 77, 123));
			label.AppendFormat("{0}2.call:{1}\r\n", nameof(func_test), file.GetFunction($"{nameof(func_test)}2").Call(25, 77, 123));

			label.AppendLine();
			label.AppendLine($"<{nameof(LuaScriptContainer)}>");
			label.AppendLine();

			LuaScriptContainerBuilder builder = LuaScriptContainerBuilder.Create();
			builder.AddField("value0", "Ola mundo");
			builder.AddField("value1", 5584f);
			builder.AddField("value2", 'C');
			builder.AddField("value3", true);
			builder.Spacing();
			builder.AddLocalField("value4", "Ola mundo");
			builder.AddLocalField("value5", 5584f);
			builder.AddLocalField("value6", 'C');
			builder.AddLocalField("value7", true);
			builder.Spacing();
			builder.AddTable(new LuaTableItem(
					"value22",
					new LuaTableValue("x", 552f),
					new LuaTableValue("y", 125f)
				));
			builder.Spacing();
			builder.AddFunction("func_test",
				"\treturn var1 + var2 * var3",
				"var1", "var2", "var3"
				);

			LuaScriptContainer container = new LuaScriptContainer(builder);
			label.AppendFormat("value0:{0}\r\n", (string)container.GetField("value0"));
			label.AppendFormat("value1:{0}\r\n", (double)container.GetField("value1"));
			label.AppendFormat("value2:{0}\r\n", (char)container.GetField("value2"));
			label.AppendFormat("value3:{0}\r\n", (bool)container.GetField("value3"));
			label.AppendFormat("value22:{0}\r\n", (Table)container.GetField("value22"));
			label.AppendFormat("value22:{0}\r\n", (CLRRef<Vector2>)container.GetField("value22"));
			
			label.AppendFormat("{0}:{1}\r\n", nameof(func_test), container.GetField(nameof(func_test)).LuaTypeCode);
			label.AppendFormat("{0}:{1}\r\n", nameof(func_test), container.GetFunction(nameof(func_test)).Call(25, 77, 123));
		}
		catch (Exception ex)
		{
			if (label != null)
			{
				label.SelfModulate = Color.Color8(255, 0, 0);
				label.Append(ex.ToString());
			}
			DebugLog.ExceptionLog(ex);
		}
	}

	public float func_test(float var1, float var2, float var3)
	{
		return var1 + var2 * var3;
	}
}
