using System;

namespace SmartLedger.Common.Domain.Exceptions;

/// <summary>
/// Represents a domain-level validation exception that occurs when a specific property fails validation rules.
/// </summary>
/// <param name="propertyName">Name of the property that failed validation.</param>
/// <param name="message">Specific validation error message related to the property.</param>
public class DomainValidationException(string propertyName, string message)
    : Exception($"Validation failed for the property '{propertyName}' with the following context: {message}.")
{
    /// <summary>
    /// Name of the property that caused the validation exception.
    /// </summary>
    public string PropertyName => propertyName;
}