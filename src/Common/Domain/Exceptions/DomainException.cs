using System;

namespace SmartLedger.Common.Domain.Exceptions;

/// <summary>
/// Represents a domain-level exception that occurs when an operation fails.
/// </summary>
public class DomainException(string propertyName, string message) 
    : Exception($"An error occurred while performing an operation with the '{propertyName}' property " +
                $"with the following context: {message}.")
{
    /// <summary>
    /// Name of the property that caused the exception.
    /// </summary>
    public string PropertyName => propertyName;
}