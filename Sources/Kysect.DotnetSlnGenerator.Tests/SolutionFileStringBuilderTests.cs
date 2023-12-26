using FluentAssertions;
using Kysect.DotnetSlnGenerator.Builders;

namespace Kysect.DotnetSlnGenerator.Tests;

public class SolutionFileStringBuilderTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Build_WithoutProject_ReturnExpectedString()
    {
        var solutionFileStringBuilder = new SolutionFileStringBuilder();
        var expected = """
                       Microsoft Visual Studio Solution File, Format Version 12.00
                       # Visual Studio Version 17
                       VisualStudioVersion = 17.9.34310.174
                       MinimumVisualStudioVersion = 10.0.40219.1
                       Global
                       	GlobalSection(SolutionProperties) = preSolution
                       		HideSolutionNode = FALSE
                       	EndGlobalSection
                       EndGlobal
                       """;

        var actual = solutionFileStringBuilder.Build();

        actual.Should().Be(expected);
    }

    [Test]
    public void Build_WithOneProject_ReturnExpectedString()
    {
        var solutionFileStringBuilder = new SolutionFileStringBuilder();

        string projectName = "Project";
        string projectPath = "Project/Project.csproj";
        var actual = solutionFileStringBuilder
            .AddProject(projectName, projectPath)
            .Build();

        actual.Should().Contain($"= \"{projectName}\", \"{projectPath}\",");
    }
}