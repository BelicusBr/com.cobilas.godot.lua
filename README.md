# Cobilas.Godot.Lua
## Quick Start
📖 [API Documentation](https://belicusbr.github.io/com.cobilas.docs/com.cobilas.godot.lua.api/Cobilas.GodotEngine.GDLua.html)
### Executing Lua Code

```csharp
// Create a Lua container with default configuration
var config = LuaContainerConfg.Default;
using var lua = new LuaContainer(config);

// Build and execute Lua code
lua.DoString("print('Hello from Lua!')")
   .InitField("playerName", "John Doe")
   .InitFunction("greet", "print('Hello, ' .. playerName)", "playerName")
   .FlushToLua();

// Invoke the function
lua.InvokeFunction("greet");
```

### Working with Lua Files

```csharp
// Load and execute a Lua script file
var fileConfig = new LuaFileConfg("path/to/script.lua");
using var luaFile = new LuaFile(fileConfig);

// Access fields from the Lua script
var playerHealth = luaFile.GetField("player.health");
int healthValue = (int)playerHealth;

// Invoke functions defined in the script
var result = luaFile.InvokeFunction("calculateDamage", 10, 2.5f);
```

### Creating Lua Tables

```csharp
// Create complex Lua table structures
var playerTable = new LuaTableItem("player",
    new LuaTableValue("name", "Hero"),
    new LuaTableValue("health", 100),
    new LuaTableValue("level", 5),
    new LuaTableItem("inventory",
        new LuaTableValue("sword", 1),
        new LuaTableValue("potion", 3)
    )
);

var config = LuaContainerConfg.Default;
using var lua = new LuaContainer(config);
lua.InitTable(playerTable).FlushToLua();
```

## Core Components

### Configuration
- `LuaContainerConfg` - Configuration for in-memory Lua execution
- `LuaFileConfg` - Configuration for file-based Lua scripts

### Main Classes
- `LuaContainer` - Dynamic Lua code builder and executor
- `LuaFile` - File-based Lua script manager
- `LuaField` - Type-safe Lua field access with conversion capabilities
- `LuaTableItem` & `LuaTableValue` - Lua table structure builders

### Interfaces
- `ILuaFile` - Core operations for Lua file interaction
- `ILuaTable` & `ILuaTableItem` - Lua table element contracts

## Advanced Usage

### Custom Serialization

```csharp
[LuaSerializable(typeof(PlayerData))]
public class PlayerDataConverter : ObjectToLuaTable
{
    public override void ToLuaTable(object obj, NLua.LuaTable table)
    {
        var player = (PlayerData)obj;
        table["name"] = player.Name;
        table["level"] = player.Level;
    }
    
    public override object ToObject(object obj, NLua.LuaTable table)
    {
        return new PlayerData
        {
            Name = (string)table["name"],
            Level = (int)table["level"]
        };
    }
}

// Usage
var playerData = new PlayerData { Name = "Warrior", Level = 10 };
lua.SetField("player", playerData);
```

### CLR Integration

```csharp
var config = new LuaContainerConfg(useCLRPackage: true);
using var lua = new LuaContainer(config);

lua.InitCLRPackage("System.Math")
   .DoString("print('PI value: ' .. Math.PI)")
   .FlushToLua();
```

## API Documentation

Comprehensive XML documentation is included with the library. Key methods include:

- `GetField()` / `SetField()` - Access and modify Lua variables
- `InvokeFunction()` - Call Lua functions with parameters
- `LuaTableToObject<T>()` - Convert Lua tables to C# objects
- `InitFunction()` - Define Lua functions programmatically
- `FlushToLua()` - Execute accumulated Lua code

## Examples

Check the `Examples/` folder for complete usage examples:
- Basic Lua execution
- Game data configuration
- Save/load system implementation
- Custom object serialization

## The [Cobilas Godot Lua](https://www.nuget.org/packages/Cobilas.Godot.Lua/) is on nuget.org
To include the package, open the `.csproj` file and add it.
```xml
<ItemGroup>
	<PackageReference Include="Cobilas.Godot.Lua" Version="1.3.1" />
</ItemGroup>
```
Or use command line.
```
dotnet add package Cobilas.Godot.Lua --version 1.3.1
```