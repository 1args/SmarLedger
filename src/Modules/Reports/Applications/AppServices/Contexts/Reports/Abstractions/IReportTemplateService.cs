namespace SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;

/// <summary>
/// Interface for compiling report templates.
/// </summary>
public interface IReportTemplateService
{
    /// <summary>
    /// Compiles a report template.
    /// </summary>
    /// <param name="templateName">Template name.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Function that can be used to generate the report content</returns>
    Task<Func<object, string>> CompileTemplateAsync(string templateName, CancellationToken cancellationToken);
}