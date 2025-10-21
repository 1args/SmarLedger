using SmartLedger.Common.Applications.Handlers.Extensions;
using SmartLedger.Common.Infrastructures.DataAccess.Extensions;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;
using SmartLedger.Modules.Reports.Applications.Handlers.Contexts.Reports.Commands.GenerateReport;
using SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs;
using SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Abstractions;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read;

namespace SmartLedger.Hosts.Public.Extensions.Modules;

/// <summary>
/// Extensions for registering the Reports module components.
/// </summary>
public static class ReportsModuleExtensions
{
    /// <summary>
    /// Registers the Reports module components.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddReportsModule(this IServiceCollection services)
    {
        services
            .AddInfrastructures()
            .AddApplications();

        return services;
    }

    /// <summary>
    /// Registers infrastructure components.
    /// </summary>
    private static IServiceCollection AddInfrastructures(this IServiceCollection services)
    {
        services
            .AddDataAccess<ReportsDbContext, ReportsDbContextConfigurator>()
            .AddScoped<IReportStorage, ReportStorage>()
            .AddScoped<IReportTemplateProvider, ReportTemplateProvider>()
            .AddScoped<IPdfGenerator, PdfGenerator>()
            .AddScoped<IReportFactory, ReportFactory>()
            .AddScoped<IReportPublisher, ReportPublisher>();

        return services;
    }

    /// <summary>
    /// Registers applications components.
    /// </summary>
    private static IServiceCollection AddApplications(this IServiceCollection services)
    {
        services
            .AddScoped<IReportsService, ReportsService>();

        services
            .AddHandlersFromAssembly(typeof(GenerateReportCommand).Assembly);

        return services;
    }
}
