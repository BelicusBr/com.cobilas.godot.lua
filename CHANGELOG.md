# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.2.6] - (29/10/2025)

### Changed
- **Version Update**: Bumped package version from 1.2.5 to 1.2.6
- **Dependencies**: Updated `Cobilas.Godot.Utility` from version 7.1.0 to 7.1.1

## [1.2.5] (29/10/2025)

### Changed
- **Version Update**: Bumped package version from 1.2.4 to 1.2.5
- **Build Configuration**: 
  - Simplified package output path from `C:\local.nuget\$(Configuration)` to `C:\local.nuget`
- **Dependencies**: Updated to latest versions:
  - `Cobilas.Core.Net4x` from 2.7.1 to 2.7.2
  - `Cobilas.Godot.Utility` from 7.0.1 to 7.0.2
- **Project Structure**: 
  - Updated .gitignore to simplified structure focusing on essential C# project folders and files
  - Removed Godot-specific ignores in favor of minimal project structure

## [1.2.4] (29/10/2025)

### Changed
- **Project SDK**: Migrated from `Godot.NET.Sdk/3.3.0` to standard `Microsoft.NET.Sdk`
- **Target Frameworks**: Removed `netstandard2.1` (now targeting `net472` and `netstandard2.0` only)
- **Package References**:
  - Updated `NLua` from version 1.7.5 to 1.7.6
  - Updated `Cobilas.Core.Net4x` from version 2.6.0 to 2.7.1
  - Updated `Cobilas.Godot.Utility` from version 6.2.3 to 7.0.1
  - Removed `<PrivateAssets>` configuration from NLua reference
- **Property Structure**: Consolidated all properties into a single `<PropertyGroup>` section
- **Version Management**: Replaced `<PackageVersion>` with `<Version>` element (1.2.3 → 1.2.4)

### Added
- **Assembly Configuration**: 
  - `<AssemblyName>com.cobilas.godot.lua</AssemblyName>`
  - `<BaseOutputPath>bin\$(Configuration)</BaseOutputPath>`
- **Package Metadata**:
  - Package description for NuGet
  - Symbol package generation (`<IncludeSymbols>` and `<SymbolPackageFormat>`)
- **Build Configuration**:
  - Enhanced warning configuration with additional suppressions
  - Overflow/underflow checking for both Debug and Release builds
  - Standardized warning level (4) for both configurations

### Build System
- **Output Paths**: Modified package output to include configuration: `C:\local.nuget\$(Configuration)`
- **Godot References**: Added explicit references to GodotSharp and GodotSharpEditor assemblies
- **Documentation**: Maintained XML documentation generation
- **Package Generation**: Made package generation unconditional (previously Release-only)

### Removed
- **Godot-Specific SDK**: No longer using Godot.NET.Sdk
- **Conditional Constants**: Removed `DefineConstants` for DEBUG, TOOLS, TRACE, and GODOT_EDITOR
- **Framework Support**: Dropped netstandard2.1 target framework

### Technical Notes
- This change aligns the project structure with standard .NET SDK conventions
- Improved compatibility with modern .NET tooling and CI/CD pipelines
- Maintains full functionality while modernizing the build configuration

## [1.2.3] (20/10/2025)

### Fixed
- **ObjectToLuaTable Converter Discovery**: Fixed type detection logic in `GetConverters()` method
  - Replaced custom `CompareTypeAndSubType<T>()` method with standard `IsSubclassOf()` approach
  - Improved reliability of converter type discovery across different .NET environments
  - Resolved potential issues with converter registration in certain runtime scenarios

### Serialization System
- **Enhanced Type Scanning**: More robust detection of custom serialization converters
- **Improved Reflection Logic**: Standardized type checking using built-in .NET methods
- **Better Cross-Platform Compatibility**: Ensures consistent behavior across different target frameworks (net472, netstandard2.0, netstandard2.1)

### Technical Improvements
- **Code Standardization**: Replaced custom type comparison with framework-standard approach
- **Maintenance Reduction**: Eliminated dependency on custom type utility methods for converter discovery
- **Performance**: Potentially improved performance through use of built-in type checking mechanisms

### Impact
- No breaking changes to public API
- Existing serialization converters continue to work without modification
- Improved reliability in automated converter discovery during runtime

## [1.2.2] (20/10/2025)

### Fixed
- **NuGet Package Dependency Configuration**: Fixed NLua package reference by adding `<PrivateAssets>build;analyzers</PrivateAssets>` configuration
  - Prevents build and analyzer assets from being transitively included in consuming projects
  - Reduces package footprint and avoids potential conflicts in downstream projects
  - Improves clean dependency management for package consumers

### Build & Packaging
- **Enhanced Package Cleanliness**: NLua dependency now properly marked as development dependency for build-time only
- **Reduced Transitive Dependencies**: Build assets and analyzers from NLua won't propagate to consuming projects
- **Improved Compatibility**: Better alignment with NuGet best practices for dependency management

### Technical Details
- The `<PrivateAssets>` element ensures that build-related assets from NLua are not included when this package is consumed by other projects
- This change doesn't affect runtime functionality but improves the package's consumer experience
- Maintains full compatibility with existing Godot Engine projects and Lua integration features

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