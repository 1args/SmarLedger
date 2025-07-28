using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartLedger.Common.Hosts.Features.Abstractions;

namespace SmartLedger.Common.Hosts.Features;

/// <summary>
/// Registry for application features.
/// </summary>
public class FeaturesRegistry
{
    /// <summary>List of registered application features.</summary>
    private readonly List<IAppFeature> _features = [];

    /// <summary>
    /// Registers a feature in the application.
    /// </summary>
    /// <param name="assembly">Assembly.</param>
    /// <returns>New <see cref="FeaturesRegistry"/> instance.</returns>
    public FeaturesRegistry RegisterFeaturesFromAssembly(Assembly assembly)
    {
        var featureTypes = assembly.GetTypes()
            .Where(t => typeof(IAppFeature).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var featureType in featureTypes)
        {
            if (Activator.CreateInstance(featureType) is IAppFeature feature)
            {
                _features.Add(feature);
            }
        }
        return this;
    }

    /// <summary>
    /// Applies all registered features to the application.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration</param>
    public void ApplyFeatures(IServiceCollection services, IConfiguration configuration)
    {
        foreach (var feature in _features)
        {
            feature.UseFeature(services,  configuration);
        }
    }
}