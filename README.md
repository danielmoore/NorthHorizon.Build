# NorthHorizon Build

### Installation

In the Nuget package manager console,

    Install-Package NorthHorizon.Build

## Tasks

### HashFiles

The HashFiles task allows you to create a file at build-time with the hash of your compiled code. This is useful for verifying the integrity of your assemblies.

#### Usage

First, create a mustache template file that uses the hash output. The template context consists of:

- `Algorithm` (The name of the hash method used, e.g. MD5)
- `Hash`
    - `HexString`
    - `Base64String`
    - `CommaDelim` A list of comma-separated byte values of the hash. Suitable for wrapping in an array initializer.

Then, modify your .csproj files to reference the NorthHorizon.Build.Tasks assembly and invoke the HashFiles task with the appropriate parameters.

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="12.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    <!-- Import the task assembly -->
    <UsingTask 
        AssemblyFile="..\packages\NorthHorizon.Build-1.0.0.0\lib\net-40\NorthHorizon.Build.Tasks.dll"
        TaskName="NorthHorizon.Build.Tasks.HashFiles" />

    <!-- ... -->

    <!-- Invoke the HashFiles task in the BeforeBuild target
         so we can add some information to the assembly -->
    <Target Name="BeforeBuild">
        <!-- Identify your mustache template to be used, the list of files 
            to be hashed, and the output location for the rendered template. -->
        <HashFiles 
            TemplatePath="SourceHashes.cs.mustache" 
            FilePaths="@(Compile)"
            OutPath="$(IntermediateOutputPath)\SourceHashes.cs" />

        <!-- Add the output file to the build -->
        <ItemGroup>
            <Compile Include="$(IntermediateOutputPath)\SourceHashes.cs" />
        </ItemGroup>
    </Target>
</Project>
```