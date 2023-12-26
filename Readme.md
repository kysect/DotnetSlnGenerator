# Kysect.DotnetSolutionGenerator

DotnetSolutionGenerator is a nuget that provide API for building dotnet solution. Main use case is creating sample solution in tests for other nugets.

This code samples:

```csharp
new DotnetSolutionBuilder("MySolution")
    .AddProject(new DotnetProjectBuilder("Project", "<Project></Project>"))
    .AddFile(new SolutionFileInfo(["Directory.Build.props"], "<Project></Project>"))
    .Save(_fileSystem, _rootPath);

```


will create this file structure:

```
- ./
    - Project/
        - Project.csproj
    - MySolution.sln
    - Directory.Build.props
```