using FluentAssertions;
using Kysect.DotnetSlnGenerator.Builders;
using Kysect.DotnetSlnGenerator.Models;
using System.IO.Abstractions.TestingHelpers;

namespace Kysect.DotnetSlnGenerator.Tests;

public class DotnetSolutionBuilderTests
{
    private MockFileSystem _fileSystem;
    private string _rootPath;

    [SetUp]
    public void Setup()
    {
        _fileSystem = new MockFileSystem();
        _rootPath = _fileSystem.Path.GetFullPath(".");
    }

    [Test]
    public void Save_ForEmptySolution_CreateSolutionFile()
    {
        string solutionName = "MySolution";

        new DotnetSolutionBuilder(solutionName)
            .Save(_fileSystem, _rootPath);

        var expectedSolutionPath = _fileSystem.Path.Combine(_rootPath, $"{solutionName}.sln");
        _fileSystem.File.Exists(expectedSolutionPath).Should().BeTrue();
    }

    [Test]
    public void Save_AfterAddingFile_FileShouldExists()
    {
        string solutionName = "MySolution";
        string directoryBuildProps = "Directory.Build.props";
        string content = "<Project></Project>";

        new DotnetSolutionBuilder(solutionName)
            .AddFile(new SolutionFileInfo([directoryBuildProps], content))
            .Save(_fileSystem, _rootPath);

        var expectedSolutionPath = _fileSystem.Path.Combine(_rootPath, $"{solutionName}.sln");
        var expectedDirectoryBuildPropsPath = _fileSystem.Path.Combine(_rootPath, directoryBuildProps);
        _fileSystem.File.Exists(expectedSolutionPath).Should().BeTrue();
        _fileSystem.File.Exists(expectedDirectoryBuildPropsPath).Should().BeTrue();
        _fileSystem.File.ReadAllText(expectedDirectoryBuildPropsPath).Should().BeEquivalentTo(content);
    }

    [Test]
    public void Save_AfterAddingProject_ProjectFileShouldExists()
    {
        string solutionName = "MySolution";
        string projectName = "Project";
        string content = "<Project></Project>";

        new DotnetSolutionBuilder(solutionName)
            .AddProject(new DotnetProjectBuilder(projectName, content))
            .Save(_fileSystem, _rootPath);

        var expectedSolutionPath = _fileSystem.Path.Combine(_rootPath, $"{solutionName}.sln");
        var expectedProjectPath = _fileSystem.Path.Combine(_rootPath, "Project", $"{projectName}.csproj");

        _fileSystem.File.Exists(expectedSolutionPath).Should().BeTrue();
        _fileSystem.File.Exists(expectedProjectPath).Should().BeTrue();
        _fileSystem.File.ReadAllText(expectedProjectPath).Should().BeEquivalentTo(content);
    }
}