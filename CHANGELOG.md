# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.2.1] (18/10/2025)

### Fixed
- **Fixed conversion methods in `LuaField`**: Corrected type conversion logic where values are now properly handled by the `IConvert` method
- **Fixed numeric type handling**: Numeric values now undergo additional processing and validation during conversion operations
- **Improved type safety**: Enhanced conversion reliability for various data types including integers, floating-point numbers, and strings

### Added
- **New explicit conversions in `LuaField`**:
  - `bool` conversion operator for direct boolean value extraction
  - `DateTime` conversion operator for date and time value handling
  - `LuaFieldType` conversion operator for accessing the field's type information
- **Enhanced type system**: Additional type conversion capabilities for better interoperability between Lua and C#
- **Extended conversion support**: More comprehensive coverage of C# primitive types and common data structures

### Technical Improvements
- Refactored internal conversion logic for better maintainability
- Added proper null value handling in conversion methods
- Improved exception messages for better debugging experience
- Enhanced type inference for mixed numeric operations

## [1.1.0] (17/10/2025)

### Added
- Initial project structure for Godot Engine Lua integration
- Core interfaces: `ILuaContainerConfg`, `ILuaFile`, `ILuaFileConfg`, `ILuaTable`, `ILuaTableItem`
- Main implementation classes: `LuaContainer`, `LuaFile`, `LuaField`
- Configuration structures: `LuaContainerConfg`, `LuaFileConfg`
- Lua table manipulation: `LuaTableItem`, `LuaTableValue`
- Serialization system: `LuaSerializableAttribute`, `ObjectToLuaTable`
- Support for CLR package integration in Lua environments
- Buffer management for dynamic Lua code execution
- Type conversion capabilities via `IConvertible` implementation in `LuaField`
- Comprehensive XML documentation for all public APIs

### Features
- Dynamic Lua code building and execution through `LuaContainer`
- File-based Lua script loading and execution via `LuaFile`
- Lua table to C# object conversion and vice versa
- Support for both global and local field initialization
- Function invocation from Lua scripts with parameter passing
- Configurable Lua library loading and CLR package usage
- Disposable pattern implementation for resource management
- Enumeration support for Lua table items

### Technical Details
- Built on KeraLua for Lua interoperability
- Integrated with Godot Engine ecosystem
- Support for both Lua 5.3 and 5.4 compatibility
- Structured configuration system for flexible Lua environment setup
- Comprehensive error handling with proper exception types

### First Release
- Initial stable release of Godot Engine Lua integration library
- Complete API surface as documented in XML documentation
- Ready for production use with Godot projects

---

*Note: This changelog tracks changes starting from the initial codebase. Previous development history is not available.*