using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.DotnetSlnGenerator.Tools;
using System.IO.Abstractions;
using Kysect.DotnetSlnGenerator.Models;

namespace Kysect.DotnetSlnGenerator.Builders;

public class DotnetProjectBuilder
{
    private readonly string _projectFileContent;
    private readonly List<SolutionFileInfo> _files;

    public string ProjectName { get; }

    public DotnetProjectBuilder(string projectName, string projectFileContent)
    {
        ProjectName = projectName;
        _projectFileContent = projectFileContent;

        _files = new List<SolutionFileInfo>();
    }

    public DotnetProjectBuilder AddEmptyFile(params string[] path)
    {
        _files.Add(new SolutionFileInfo(path, string.Empty));
        return this;
    }

    public DotnetProjectBuilder AddFile(SolutionFileInfo solutionFileInfo)
    {
        _files.Add(solutionFileInfo);
        return this;
    }

    public void Save(IFileSystem fileSystem, string rootPath)
    {
        fileSystem.ThrowIfNull();

        fileSystem.EnsureDirectoryExists(fileSystem.Path.Combine(rootPath, ProjectName));
        string csprojPath = fileSystem.Path.Combine(rootPath, ProjectName, $"{ProjectName}.csproj");
        fileSystem.File.WriteAllText(csprojPath, _projectFileContent);

        foreach (var solutionFileInfo in _files)
        {
            string[] fileFullPathParts = [rootPath, ProjectName, .. solutionFileInfo.Path];
            string fileFullPath = fileSystem.Path.Combine(fileFullPathParts);
            IFileInfo fileInfo = fileSystem.FileInfo.New(fileFullPath);
            fileSystem.EnsureContainingDirectoryExists(fileInfo);

            fileSystem.File.WriteAllText(fileFullPath, solutionFileInfo.Content);
        }
    }
}