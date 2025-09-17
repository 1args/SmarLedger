using HandlebarsDotNet;
using SmartLedger.Modules.Reports.Contracts.Common;
using SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Abstractions;

namespace SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs;

/// <summary>
/// Service for compiling report templates.
/// </summary>
public sealed class ReportTemplateProvider(
    IHandlebars handlebars) : IReportTemplateProvider
{
    /// <inheritdoc/>
    public async Task<string> CompileTemplateAsync(object model, CancellationToken cancellationToken)
    {
        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), ReportPaths.TemplatePath);
        var templateContent = await File.ReadAllTextAsync(templatePath, cancellationToken);
        var template = handlebars.Compile(templateContent);

        return template(model);
    }
}