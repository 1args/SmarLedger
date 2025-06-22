namespace SmartLedger.Common.Contracts.Abstractions;

/// <summary>
/// Indicates the command without returning a result.
/// </summary>
public interface ICommand;

/// <summary>
/// Indicates the command with the expected result.
/// </summary>
/// <typeparam name="TResponse">Type of command result.</typeparam>
public interface ICommand<TResponse>; 