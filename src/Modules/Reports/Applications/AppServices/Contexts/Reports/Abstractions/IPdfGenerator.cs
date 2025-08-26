namespace SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;

/// <summary>
/// Interface for generating PDF documents from HTML content.
/// </summary>
public interface IPdfGenerator
{
    /// <summary>
    /// Generates a PDF document from the provided HTML content.
    /// </summary>
    /// <param name="htmlContent">HTML content.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Stream containing a report.</returns>
    Task<Stream> GeneratePdfAsync(string htmlContent, CancellationToken cancellationToken);
}