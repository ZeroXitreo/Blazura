# Blazura

Blazura is an early-stage, component-driven UI library for Blazor. It offers a growing set of essential UI components and utilities to help you build web applications quickly and consistently. While customization options are currently limited, future releases will focus on expanding flexibility and theming capabilities.

[![Build, Test, & Pack](https://github.com/ZeroXitreo/Blazura/actions/workflows/pack.yml/badge.svg)](https://github.com/ZeroXitreo/Blazura/actions/workflows/pack.yml)

## Prerequisites

Before you begin, ensure you have the following installed:

- [.NET SDK 9.0 or later](https://dotnet.microsoft.com/download)
- [Node.js and npm](https://nodejs.org/) (for SCSS/JS bundling)

## Getting started

<!-- TODO: Create a getting started section -->

### Add Blazura to your project

1. **Install the NuGet packages**:

   ```sh
   dotnet add package Blazura
   dotnet add package Microsoft.Extensions.FileProviders.Embedded --version 9.0.6
   dotnet add package Microsoft.TypeScript.MSBuild
   ```

   https://www.nuget.org/packages/Blazura/

2. **Enable embedded resources**:

   Add the following to your `.csproj` file to ensure Blazura can find your static assets as embedded resources:

   ```xml
   <PropertyGroup>
     <GenerateEmbeddedFilesManifest>true</GenerateEmbeddedFilesManifest>
   </PropertyGroup>

   <ItemGroup>
     <EmbeddedResource Include="**\*.js" />
     <EmbeddedResource Include="**\*.scss" />
     <EmbeddedResource Remove="wwwroot\**\*.js" />
   </ItemGroup>
   ```

3. **Copy files from this project to yours**

   Some files is needed to be copied over to your own project in order to gain access to SCSS variables and other TypeScript functionality.

   Files to copy:

   - Styles/Root.scss
   - Scripts/IClassBringer.ts
   - tsconfig.json

   Also make a new file `Scripts/ClassBringer.d.ts` and add the following:

   ```typescript
   declare class ClassBringer {
     public static register(bringer: new () => IClassBringer): void;
   }
   ```

4. **Register services**

   In `Program.cs`, add:

   ```csharp
   builder.Services.AddBlazuraServices();
   builder.Services.AddBlazuraOptimizer();
   ```

5. **Start using components**

   Import the Blazura namespace in your `_Imports.razor`:

   ```razor
   @using Blazura.Components
   ```

   Now you can use Blazura components in your Blazor pages.

6. **Add BundleComplete to your App.razor**

   Add the `BundleComplete` component to your `App.razor` file to ensure all bundled assets are loaded:

   ```razor
   <head>
       <BundleComplete />
   </head>
   ```

## Features

- **Rich Component Library**: Includes modals, drawers, snackbars, buttons, input controls, layout utilities, and more.
- **Customizable Styles**: Built with SCSS for easy theming and customization.
- **Blazor-First**: Designed specifically for Blazor, leveraging its component model and data binding.
- **Optimized Bundling**: Integrates with WebOptimizer for efficient asset bundling and minification.

## Contributing

Contributions are welcome! Please open issues or submit pull requests for bug fixes, new features, or improvements.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details. However, re-uploading, rebranding, or redistributing this project in a way that obscures or removes original authorship is against the intended use of the license and may result in takedown requests.

Please maintain attribution and do not misrepresent ownership of this software.
