using System;

namespace SmartLedger.Common.Domain.Exceptions;

/// <summary>
/// Represents a domain-level exception that occurs when an operation fails.
/// </summary>
public class DomainException(string propertyName, string message) 
    : Exception(message)
{
    /// <summary>
    /// Name of the property that caused the exception.
    /// </summary>
    public string PropertyName => propertyName;
}