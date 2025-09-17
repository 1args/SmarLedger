namespace SmartLedger.Modules.Reports.Domain.Enums;

/// <summary>
/// Status of the report generation process.
/// </summary>
public enum ReportStatus
{
    /// <summary>Generation is in progress.</summary>
    InProgress,

    /// <summary>Report is ready for download.</summary>
    Ready,

    /// <summary>Report generation has failed.</summary>
    Failed
}