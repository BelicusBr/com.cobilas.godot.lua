using System;
using System.Collections.Generic;

namespace Cobilas.GodotEngine.Lua;

internal static class CustomConverters {
	private static bool _init = false;
	private static readonly Dictionary<Type, ObjectToLuaTable> converters = GetConverters();
	/// <summary>Attempts to retrieve a converter for the specified type.</summary>
	/// <param name="type">The type to find a converter for.</param>
	/// <param name="value">When this method returns, contains the converter associated with the specified type, if found; otherwise, null.</param>
	/// <returns>true if a converter for the specified type was found; otherwise, false.</returns>
	internal static bool TryGetValue(Type type, out ObjectToLuaTable value) => converters.TryGetValue(type, out value);

	private static Dictionary<Type, ObjectToLuaTable> GetConverters() {
		Dictionary<Type, ObjectToLuaTable> result = [];
		foreach (Type item in TypeUtilitarian.GetTypes())
			if (item.IsSubclassOf(typeof(ObjectToLuaTable))) {
				ObjectToLuaTable obj2lt = item.Activator<ObjectToLuaTable>();
				LuaSerializableAttribute[] attributes = item.GetAttributes<LuaSerializableAttribute>();
				if (attributes is not null)
					foreach (LuaSerializableAttribute item2 in attributes)
						result.Add(item2.TypeTarget, obj2lt);
			}
		return result;
	}
}
