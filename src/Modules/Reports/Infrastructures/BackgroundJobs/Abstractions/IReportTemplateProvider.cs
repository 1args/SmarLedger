namespace SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Abstractions;

/// <summary>
/// Interface for compiling report templates.
/// </summary>
public interface IReportTemplateProvider
{
    /// <summary>
    /// Compiles a report template.
    /// </summary>
    /// <param name="templateName">Template name.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Function that can be used to generate the report content</returns>
    Task<string> CompileTemplateAsync(object model, CancellationToken cancellationToken);
}