using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.DotnetSlnGenerator.Models;
using System.IO.Abstractions;

namespace Kysect.DotnetSlnGenerator.Builders;

public class DotnetSolutionBuilder
{
    private readonly string _solutionName;
    private readonly List<DotnetProjectBuilder> _projects;
    private readonly List<SolutionFileInfo> _files;

    public DotnetSolutionBuilder(string solutionName)
    {
        _solutionName = solutionName;

        _projects = new List<DotnetProjectBuilder>();
        _files = new List<SolutionFileInfo>();
    }

    public DotnetSolutionBuilder AddProject(DotnetProjectBuilder project)
    {
        _projects.Add(project);
        return this;
    }

    public DotnetSolutionBuilder AddFile(SolutionFileInfo solutionFileInfo)
    {
        _files.Add(solutionFileInfo);
        return this;
    }

    public void Save(IFileSystem fileSystem, string rootPath)
    {
        fileSystem.ThrowIfNull();

        string solutionFileContent = CreateSolutionFileContent(fileSystem);
        fileSystem.File.WriteAllText(fileSystem.Path.Combine(rootPath, $"{_solutionName}.sln"), solutionFileContent);

        foreach (var solutionFileInfo in _files)
        {
            var partialFilePath = fileSystem.Path.Combine(solutionFileInfo.Path.ToArray());
            fileSystem.File.WriteAllText(fileSystem.Path.Combine(rootPath, partialFilePath), solutionFileInfo.Content);
        }

        foreach (DotnetProjectBuilder projectBuilder in _projects)
            projectBuilder.Save(fileSystem, rootPath);
    }

    private string CreateSolutionFileContent(IFileSystem fileSystem)
    {
        fileSystem.ThrowIfNull();

        var solutionFileStringBuilder = new SolutionFileStringBuilder();

        foreach (DotnetProjectBuilder projectBuilder in _projects)
            solutionFileStringBuilder.AddProject(projectBuilder.ProjectName, fileSystem.Path.Combine(projectBuilder.ProjectName, $"{projectBuilder.ProjectName}.csproj"));

        return solutionFileStringBuilder.Build();
    }
}