using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using SmartLedger.Common.Infrastructure.Configurations.Convertors;

namespace SmartLedger.Common.Infrastructure.Configurations;

/// <summary>
/// Extension methods for <see cref="ModelBuilder"/> to configure default <see cref="DateTimeKind"/> for DateTime properties.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies a global value converter to all DateTime properties to ensure the specified <see cref="DateTimeKind"/>.
    /// </summary>
    /// <param name="modelBuilder">EF model builder.</param>
    /// <param name="dateTimeKind">DateTimeKind to enforce (Utc, Local, Unspecified).</param>
    public static void SetDefaultDateTimeKind(this ModelBuilder modelBuilder, DateTimeKind dateTimeKind)
    {
        var converter = dateTimeKind switch
        {
            DateTimeKind.Utc => new DateTimeKindValueConvertor(DateTimeKind.Utc),
            DateTimeKind.Local => new DateTimeKindValueConvertor(DateTimeKind.Local),
            DateTimeKind.Unspecified => new DateTimeKindValueConvertor(DateTimeKind.Unspecified),
            _ => throw new ArgumentOutOfRangeException(nameof(dateTimeKind), dateTimeKind,
                "Invalid 'DateTimeKind' specified.")
        };

        modelBuilder.UseTypeValueConverter<DateTime>(converter);
        modelBuilder.UseTypeValueConverter<DateTime?>(converter);
    }

    private static ModelBuilder UseTypeValueConverter<TType>(this ModelBuilder modelBuilder, ValueConverter valueConverter)
    {
        return modelBuilder.UseTypeValueConverter(typeof(TType), valueConverter);
    }

    private static ModelBuilder UseTypeValueConverter(this ModelBuilder modelBuilder, Type type, ValueConverter valueConverter)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties()
                .Where(t => t.PropertyType == type);

            foreach (var property in properties)
            {
                modelBuilder.Entity(entityType.Name)
                    .Property(property.Name)
                    .HasConversion(valueConverter);
            }
        }

        return modelBuilder;
    }
}