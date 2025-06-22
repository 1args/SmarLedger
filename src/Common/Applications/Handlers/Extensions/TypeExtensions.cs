namespace SmartLedger.Common.Applications.Handlers.Extensions;

/// <summary>
/// Extension for working with handler registration.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Determines whether the specified type is a concrete class (i.e., not abstract and not an interface).
    /// </summary>
    /// <param name="type"><see cref="Type"/> to determine.</param>
    /// <returns><c>true</c> if the type is concrete; otherwise, <c>false</c>.</returns>
    public static bool IsConcrete(this Type type) =>
        type is { IsAbstract: false, IsInterface: false };

    /// <summary>
    /// Determines whether the specified type is an open generic type.
    /// </summary>
    /// <param name="type"><see cref="Type"/> to determine.</param>
    /// <returns>
    /// <c>true</c> if the type is a generic type definition or contains unassigned generic type parameters; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsOpenGenericType(this Type type) =>
        type.IsGenericTypeDefinition || type.ContainsGenericParameters;
}