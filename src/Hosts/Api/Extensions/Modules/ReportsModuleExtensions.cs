using SmartLedger.Common.Applications.Handlers.Extensions;
using SmartLedger.Common.Infrastructures.DataAccess.Extensions;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;
using SmartLedger.Modules.Reports.Applications.Handlers.Contexts.Reports.Commands.GenerateReport;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read;

namespace SmartLedger.Hosts.Api.Extensions.Modules;

/// <summary>
/// Extensions for registering the Reports module.
/// </summary>
public static class ReportsModuleExtensions
{
    /// <summary>
    /// Registers the Reports module.
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
            .AddDataAccess<ReportsReadDbContext, ReportsReadDbContextConfigurator>();

        return services;
    }

    /// <summary>
    /// Registers applications components.
    /// </summary>
    private static IServiceCollection AddApplications(this IServiceCollection services)
    {
        services
            .AddScoped<IReportsService, ReportsService>()
            .AddScoped<IReportStorageService, ReportStorageService>()
            .AddScoped<IReportTemplateService, ReportTemplateService>()
            .AddScoped<IPdfGenerator, PdfGenerator>();

        services
            .AddHandlersFromAssembly(typeof(GenerateReportCommand).Assembly);

        return services;
    }
}
