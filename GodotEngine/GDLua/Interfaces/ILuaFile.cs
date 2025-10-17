namespace Cobilas.GodotEngine.GDLua.Interfaces;

/// <summary>Defines operations for interacting with Lua files and their contents.</summary>
/// <remarks>Provides methods to read, write, and invoke elements within Lua script files.</remarks>
public interface ILuaFile {
    /// <summary>Retrieves a field from the Lua file by its path.</summary>
    /// <param name="pathField">The path to the field in the Lua file.</param>
    /// <returns>A <see cref="LuaField"/> containing the field data.</returns>
    LuaField GetField(string pathField);
    /// <summary>Converts a Lua table to an object of the specified type.</summary>
    /// <typeparam name="T">The target type to convert the Lua table to.</typeparam>
    /// <param name="pathField">The path to the Lua table in the file.</param>
    /// <returns>A <see cref="LuaField"/> containing the converted table data.</returns>
    LuaField LuaTableToObject<T>(string pathField);
    /// <summary>Converts a Lua table to an object and assigns it to the provided reference.</summary>
    /// <typeparam name="T">The type of the object to assign.</typeparam>
    /// <param name="pathField">The path to the Lua table in the file.</param>
    /// <param name="value">The reference variable to assign the converted table data to.</param>
    void LuaTableToObject<T>(string pathField, ref T value);
    /// <summary>Sets the value of a field in the Lua file.</summary>
    /// <param name="pathField">The path to the field in the Lua file.</param>
    /// <param name="value">The value to assign to the field.</param>
    void SetField(string pathField, object value);
    /// <summary>Invokes a function defined in the Lua file.</summary>
    /// <param name="methodName">The name of the function to invoke.</param>
    /// <param name="args">The arguments to pass to the function.</param>
    /// <returns>An array of objects containing the function's return values.</returns>
    object[] InvokeFunction(string methodName, params object[] args);
}