namespace SmartLedger.Modules.Reports.Domain.Enums;

/// <summary>
/// Represents the type of report to be generated.
/// </summary>
public enum ReportType
{
    /// <summary>Generates a daily report.</summary>
    Daily,

    /// <summary>Generates a weekly report.</summary>
    Weekly,

    /// <summary>Generates a monthly report.</summary>
    Monthly,

    /// <summary>Generates a yearly report.</summary>
    Yearly,

    /// <summary>Generates a custom report based on user-defined parameters.</summary>
    Custom
}