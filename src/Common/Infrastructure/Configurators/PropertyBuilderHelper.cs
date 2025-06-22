using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartLedger.Common.Infrastructure.Configurators;

/// <summary>
/// Contains helper methods for configuring entity property mappings.
/// </summary>
public static class PropertyBuilderHelper
{
    /// <summary>
    /// Maps a nullable Guid property to the "uuid" column.
    /// </summary>
    public static PropertyBuilder<Guid?> IsGuid(this PropertyBuilder<Guid?> propertyBuilder) =>
        propertyBuilder.HasColumnName("uuid");

    /// <summary>
    /// Maps a non-nullable Guid property to the "uuid" column and marks it as required.
    /// </summary>
    public static PropertyBuilder<Guid> IsGuid(this PropertyBuilder<Guid> propertyBuilder) =>
        propertyBuilder.HasColumnName("uuid").IsRequired();

    /// <summary>
    /// Sets a default SQL value of "now()" for DateTime properties.
    /// </summary>
    public static PropertyBuilder<DateTime> IsDateTime(this PropertyBuilder<DateTime> propertyBuilder) =>
        propertyBuilder.HasDefaultValueSql("now()");
}