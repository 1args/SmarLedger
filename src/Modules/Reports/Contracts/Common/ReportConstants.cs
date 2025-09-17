namespace SmartLedger.Modules.Reports.Contracts.Common;

/// <summary>
/// Provides constant values used for report generation.
/// </summary>
public static class ReportConstants
{
    /// <summary> Specifies the relative path to the default report template file used for generating reports.</summary>
    public const string TemplatePath = "Templates/report.template.hbs";

    /// <summary> Represents the format string used to generate report file paths for file storage. </summary>
    public const string ReportFilePathFormat = "report/{0}/{1}_{2}.pdf";
}