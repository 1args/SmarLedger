using HandlebarsDotNet;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;
using SmartLedger.Modules.Reports.Contracts.Helpers;

namespace SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports;

/// <summary>
/// Service for compiling report templates.
/// </summary>
public sealed class ReportTemplateService : IReportTemplateService
{
    /// <inheritdoc/>
    public async Task<Func<object, string>> CompileTemplateAsync(string templateName, CancellationToken cancellationToken)
    {
        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", $"{templateName}.hbs");
        var templateContent = await File.ReadAllTextAsync(templatePath, cancellationToken);

        var handlebars = Handlebars.Create();
        CustomHandlebarsHelper.RegisterHelpers(handlebars);

        var compiled = handlebars.Compile(templateContent);

        return model =>
        {
            using var writer = new StringWriter();
            compiled(writer, model);
            return writer.ToString();
        };
    }
}