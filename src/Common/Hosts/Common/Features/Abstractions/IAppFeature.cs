using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SmartLedger.Common.Host.Features.Abstractions;

/// <summary>
/// Defines a feature of the application that can be used to extend its functionality.
/// </summary>
public interface IAppFeature
{
    /// <summary>
    /// Use this feature in the application.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration</param>
    public void UseFeature(IServiceCollection services, IConfiguration configuration);
}