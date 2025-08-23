using HandlebarsDotNet;

namespace SmartLedger.Modules.Reports.Contracts.Helpers;

/// <summary>
/// Provides custom Handlebars helpers for report generation.
/// </summary>
public static class HandlebarsHelper
{
    /// <summary>
    /// Registers custom Handlebars helpers.
    /// </summary>
    /// <param name="handlebars">Handlebars instance.</param>
    public static void RegisterHelpers(this IHandlebars handlebars)
    {
       handlebars.RegisterFixedValueHelper();
       handlebars.RegisterFormatDateHelper(); 
       handlebars.RegisterFormatDateShortHelper();
    }

    /// <summary>
    /// Registers a helper to format numbers to two decimal places.
    /// </summary>
    private static void RegisterFixedValueHelper(this IHandlebars handlebars)
    {
        Handlebars.RegisterHelper("toFixed", (writer, context, parameters) =>
        {
            if (parameters.Length == 0) return;

            if (decimal.TryParse(parameters[0]?.ToString(), out var number))
            {
                writer.WriteSafeString(number.ToString("F2"));
            }
            else
            {
                writer.WriteSafeString(parameters[0]?.ToString() ?? "");
            }
        });
    }

    /// <summary>
    /// Registers a helper to format dates to "yyyy-MM-dd HH:mm:ss".
    /// </summary>
    private static void RegisterFormatDateHelper(this IHandlebars handlebars)
    {
        Handlebars.RegisterHelper("formatDate", (writer, context, parameters) =>
        {
            switch (parameters.Length)
            {
                case >= 1 when parameters[0] is DateTime date:
                    writer.WriteSafeString(date.ToString("yyyy-MM-dd HH:mm:ss"));
                    break;
                case >= 1 when parameters[0] is string dateString &&
                               DateTime.TryParse(dateString, out var parsedDate):
                    writer.WriteSafeString(parsedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    break;
            }
        });
    }

    /// <summary>
    /// Registers a helper to format dates to "yyyy-MM-dd".
    /// </summary>
    private static void RegisterFormatDateShortHelper(this IHandlebars handlebars)
    {
        Handlebars.RegisterHelper("formatDateShort", (writer, context, parameters) =>
        {
            if (parameters.Length >= 1 && parameters[0] is DateTime date)
            {
                writer.WriteSafeString(date.ToString("yyyy-MM-dd"));
            }
        });
    }
}