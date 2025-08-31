using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Proporciona métodos de extensión para configurar automáticamente las propiedades DateTime y DateTime? 
/// de todas las entidades del modelo para que sean tratadas como UTC al leer desde la base de datos.
/// </summary>
public static class UtcDateTimeExtensions
{
    /// <summary>
    /// Aplica la conversión de tipo a todas las propiedades DateTime y DateTime? del modelo,
    /// asegurando que al leer desde la base de datos, el tipo DateTimeKind sea UTC.
    /// </summary>
    /// <param name="modelBuilder">El ModelBuilder de Entity Framework Core.</param>
    public static void ApplyUtcDateTimeKind(this ModelBuilder modelBuilder)
    {
        // Conversor para propiedades DateTime (no nullable)
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v,
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        // Conversor para propiedades DateTime? (nullable)
        var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);

        // Itera sobre todas las entidades y sus propiedades
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                // Aplica el conversor a propiedades DateTime
                if (property.ClrType == typeof(DateTime))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(property.Name)
                        .HasConversion(dateTimeConverter);
                }

                // Aplica el conversor a propiedades DateTime? (nullable)
                if (property.ClrType == typeof(DateTime?))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(property.Name)
                        .HasConversion(nullableDateTimeConverter);
                }
            }
        }
    }
}
