using Microsoft.Extensions.Logging;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;

namespace SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports;

/// <summary>
/// Service for generating PDF documents from HTML content.
/// </summary>
/// <param name="logger"></param>
public sealed class PdfGenerator(
    ILogger<PdfGenerator> logger) : IPdfGenerator
{
    /// <inheritdoc/>
    public async Task<Stream> GeneratePdfAsync(string htmlContent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Generating PDF from HTML content");

        try
        {
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = ["--no-sandbox", "--disable-setuid-sandbox"]
            });

            await using var page = await browser.NewPageAsync();

            await page.SetContentAsync(htmlContent, new NavigationOptions
            {
                WaitUntil = [WaitUntilNavigation.Load, WaitUntilNavigation.Networkidle0]
            });

            var pdfOptions = new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                PreferCSSPageSize = true,
                MarginOptions = new MarginOptions
                {
                    Top = "40px",
                    Right = "20px",
                    Bottom = "20px",
                    Left = "20px"
                }
            };

            var pdfStream = await page.PdfStreamAsync(pdfOptions);
            pdfStream.Position = 0;

            return pdfStream;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to generate PDF");
            throw new InvalidOperationException("Failed to generate PDF.", ex);
        }
    }
}