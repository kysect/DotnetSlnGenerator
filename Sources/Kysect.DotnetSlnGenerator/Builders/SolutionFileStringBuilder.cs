using System.Text;

namespace Kysect.DotnetSlnGenerator.Builders;

public class SolutionFileStringBuilder
{
    private readonly StringBuilder _builder;

    public SolutionFileStringBuilder()
    {
        var header = """
                     Microsoft Visual Studio Solution File, Format Version 12.00
                     # Visual Studio Version 17
                     VisualStudioVersion = 17.9.34310.174
                     MinimumVisualStudioVersion = 10.0.40219.1
                     """;

        _builder = new StringBuilder();
        _builder.AppendLine(header);
    }

    public SolutionFileStringBuilder AddProject(string projectName, string projectPath)
    {
        string projectDefinition = $$"""
                                     Project("{{{Guid.Empty}}}") = "{{projectName}}", "{{projectPath}}", "{{{Guid.Empty}}}"
                                     EndProject
                                     """;

        _builder.AppendLine(projectDefinition);
        return this;
    }

    public string Build()
    {
        var footer = """
                     Global
                     	GlobalSection(SolutionProperties) = preSolution
                     		HideSolutionNode = FALSE
                     	EndGlobalSection
                     EndGlobal
                     """;

        _builder.Append(footer);
        return _builder.ToString();
    }
}