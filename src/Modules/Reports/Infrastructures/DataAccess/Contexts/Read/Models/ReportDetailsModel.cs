using SmartLedger.Modules.Reports.Domain.Enums;

namespace SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read.Models;

/// <summary>
/// Represents a read model for a report.
/// </summary>
public sealed class ReportDetailsModel
{
    /// <summary>Report ID.</summary>
    public Guid Id { get; set; }

    /// <summary>User ID.</summary>
    public Guid UserId { get; set; }

    /// <summary>Report type.</summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>Date and time when account was generated.</summary>
    public DateTime GeneratedAt { get; set; }

    /// <summary>Report path in file storage.</summary>
    public string Path { get; set; } = string.Empty;
}