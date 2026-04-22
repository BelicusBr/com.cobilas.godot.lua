# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [2.1.0] - (21/04/2026)

### Changed
- Updated dependency `Cobilas.Core.Net4x` from version `2.7.2` to `2.12.0`.
- Updated dependency `Cobilas.Godot.Utility` from version `7.4.2` to `8.0.1`.
- Refactored project file (`com.cobilas.godot.lua.csproj`):
  - Moved NuGet packaging properties (`Title`, `Authors`, `PackageId`, `RepositoryUrl`, `RepositoryType`, `NeutralLanguage`, `PackageTags`, `PackageReadmeFile`, `PackageLicenseFile`, `PackageOutputPath`, `PackageRequireLicenseAcceptance`, `GeneratePackageOnBuild`) into a dedicated `<PropertyGroup>`.
  - Set `GeneratePackageOnBuild` to only run in `Release` configuration.
  - Moved `WarningLevel` and `CheckForOverflowUnderflow` to the main property group (applies to all configurations).
  - Added `project-git-funcs.ps1` to the project as a content file.
- Bumped package version from `2.0.3` to `2.1.0`.

---

## [2.0.3] - (23/11/2025)

### Fixed
- **Script Refresh**: Fixed `LuaScriptFile.RefreshBuffer` method to properly update the script container when file changes are detected
- **Buffer Management**: Added explicit buffer refresh call to ensure script content is correctly reloaded from the underlying archive stream

### Changed
- **Version Update**: Bumped package version from 2.0.2 to 2.0.3

## [2.0.2] - (23/11/2025)

### Changed
- **Dependency Update**: Upgraded `Cobilas.Godot.Utility` dependency from version 7.3.3 to 7.4.2
- **Package Configuration**: Added `<PrivateAssets>analyzers</PrivateAssets>` to utility package reference for better dependency management

### Removed
- **Test Project**: Removed direct `Cobilas.Godot.Utility` package reference from test project, as it's now inherited through project reference

### Technical Notes
- Version bump from 2.0.1 to 2.0.2 to reflect dependency updates
- Test project now relies on transitive dependency resolution through the main project reference
- Private assets configuration prevents analyzer dependencies from flowing to consuming projects

## [2.0.1] - (20/11/2025)
### Changed
The package version has been changed from [2.0.0] to [2.0.1].
There have been changes to the package's license.

## [2.0.0] - (20/11/2025)
### Added
- Comprehensive XML documentation comments for all major classes including:
  - `CLRRef<T>` - Type-safe wrapper for Lua table to CLR object conversion
  - `LuaException` - Custom exception class for Lua script execution errors
  - `LuaField` - Field representation with type conversion capabilities
  - `LuaFunc` - Lua function wrapper with invocation support
  - `LuaScript` - Base class for Lua script handling in Godot Engine
  - `LuaScriptContainer` - In-memory Lua script container
  - `LuaScriptContainerBuilder` - Builder pattern for constructing Lua scripts
  - `LuaScriptFile` - File-based Lua script with auto-refresh capabilities
  - `LuaTypeCode` - Unified type system for Lua values
- Added third-party licensing credits to LICENSE.md including:
  - KopiLua string library components
  - Visual Studio Code debugger code from Microsoft
  - Eclipse project icons for Remote Debugger
  - MoonSharp icon copyright

### Changed
- Updated package version to 2.0.0 in README.md and project files
- Improved code documentation with detailed parameter and return value descriptions
- Enhanced method signatures with `readonly` modifiers where appropriate
- Updated package metadata to require license acceptance

## [2.0.0-ch.2] - (18/11/2025)
### Fixed
- Corrected XML documentation inheritance tags in `CLRRef<T>` and `LuaField` classes
- Removed unused `_init` flag from `CustomConverters` class
- Fixed method signatures in `LuaField` to use `readonly` modifiers
- Corrected `IConvert` method calls in `LuaField` to remove unnecessary culture parameter
- Improved license file formatting and structure

### Changed
- Marked disposal methods with proper `inheritdoc` tags in `LuaFunc` and `LuaScript` classes
- Enhanced code formatting and consistency across all files
- Updated project configuration and build settings

## [2.0.0-ch.1] - (17/11/2025)
### Added
- Initial pre-release version with core Lua integration functionality
- Basic Godot Engine integration components
- Foundation for Lua script execution and management

### Notes
This release represents a major milestone in the Cobilas Godot Lua integration package, 
providing comprehensive documentation and improved code quality for production use. 
The package now offers full IntelliSense support through extensive XML documentation and 
maintains backward compatibility with existing functionality.

## [1.3.1] - (06/11/2025)

### Added
- **Build System**: Added Cobilas.Godot.Lua.props file for centralized package version management
- **MSBuild Properties**: Implemented GDLuaPackageVersion and GDLuaPackFolde properties for build configuration

### Changed
- **Version Management**: Centralized version control using MSBuild properties instead of hardcoded values
- **Build Configuration**: Updated Cobilas.Godot.Lua.targets to use imported properties from props file
- **Project Structure**: Removed obsolete obss.txt file and cleaned up project configuration

### Technical Notes
- The new props file allows for consistent version management across build targets and project configuration
- Package folder paths are now dynamically resolved using NuGetPackageRoot variable

## [1.3.0] - (06/11/2025)

### Added
- **Lua Function Support**: Introduced LuaFunc struct for safe Lua function invocation and resource management
- **Function Access**: Added GetLuaFunc method to ILuaFile interface and implementations
- **Table Conversion**: Enhanced LuaField with IsLuaTable property and LuaTableTo method for type conversion
- **Function Wrapper**: New LuaFunc struct provides proper disposal and exception handling for Lua functions

### Features
- **Safe Invocation**: LuaFunc provides multiple Invoke methods with proper return value handling
- **Resource Management**: Implements IDisposable for proper cleanup of underlying Lua functions
- **Type Safety**: Improved type conversion capabilities for Lua table data

### Changed
- **Version Update**: Bumped package version from 1.2.9 to 1.3.0
- **Code Documentation**: Enhanced XML documentation for better IntelliSense support
- **Interface Consistency**: Unified method signatures across LuaFile and LuaContainer implementations

## [1.2.9] - (06/11/2025)

### Added
- **Cross-Platform Support**: Added Lua DLLs for all major platforms (Windows, Linux, macOS, Android, iOS)
- **Build System**: Implemented Cobilas.Godot.Lua.targets for automatic library deployment
- **Godot Integration**: Added com.cobilas.godot.lua.gdnlib configuration file for Godot plugin system
- **MSBuild Integration**: Created custom build target to copy Lua libraries and configuration files

### Features
- **Automatic Deployment**: Build system automatically copies platform-specific Lua DLLs to output directory
- **Plugin Configuration**: Proper Godot native plugin configuration for all target platforms
- **Dependency Management**: Updated Cobilas.Godot.Utility dependency from 7.2.2 to 7.3.2

### Fixed
- **Stream Access**: Corrected stream type usage in LuaFile constructor to use GDStream for proper file handling

### Technical Notes
- The build target automatically copies Lua libraries for all supported platforms during build process
- The .gdnlib file provides proper configuration for Godot's native plugin system across different operating systems
- Platform-specific libraries include: Windows (x86, x64, ARM, ARM64), Linux (x64, ARM64), macOS, Android (ARM, ARM64, x86, x64), and iOS

## [1.2.8] - (31/10/2025)

### Changed
- **Version Update**: Bumped package version from 1.2.7 to 1.2.8
- **Dependency Update**: Upgraded Cobilas.Godot.Utility from version 7.1.1 to 7.2.2
- **Documentation**: Updated README.md to reflect new version 1.2.8 in installation examples

## [1.2.7] - (30/10/2025)

### Changed
- **Version Update**: Bumped package version from 1.2.6 to 1.2.7
- **Dependency Configuration**: Added PrivateAssets configuration to NLua package reference to exclude build and analyzers from consuming projects

### Technical Notes
- The PrivateAssets configuration for NLua helps prevent build assets and analyzers from being transitively included in projects that consume this package, 
reducing potential conflicts and improving build performance

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