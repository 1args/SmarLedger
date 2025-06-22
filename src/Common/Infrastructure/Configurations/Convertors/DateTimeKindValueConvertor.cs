using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SmartLedger.Common.Infrastructure.Configurations.Convertors;

/// <summary>
/// Converts <see cref="DateTime"/> values to a specific <see cref="DateTimeKind"/> (Utc, Local, or Unspecified)
/// and ensures proper conversion during database read/write operations.
/// </summary>
/// <param name="dateTimeKind">Target DateTimeKind to use during conversion.</param>
/// <param name="mappingHints">Optional mapping hints for the conversion.</param>
public sealed class DateTimeKindValueConvertor(DateTimeKind dateTimeKind, ConverterMappingHints? mappingHints = null)
    : ValueConverter<DateTime, DateTime>(
        v => v.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(v, dateTimeKind).ToUniversalTime()
            : v.ToUniversalTime(),
        v => DateTime.SpecifyKind(v, dateTimeKind), mappingHints)
{
    /// <summary>
    /// A predefined UTC <see cref="DateTimeKindValueConvertor"/>.
    /// </summary>
    public static readonly DateTimeKindValueConvertor Utc = new(DateTimeKind.Utc);

    /// <summary>
    /// A predefined Local <see cref="DateTimeKindValueConvertor"/>.
    /// </summary>
    public static readonly DateTimeKindValueConvertor Local = new(DateTimeKind.Local);

    /// <summary>
    /// A predefined Unspecified <see cref="DateTimeKindValueConvertor"/>.
    /// </summary>
    public static readonly DateTimeKindValueConvertor Unspecified = new(DateTimeKind.Unspecified);
}