using HandlebarsDotNet;

namespace SmartLedger.Modules.Reports.Contracts.Helpers;

/// <summary>
/// Provides custom Handlebars helpers for report generation.
/// </summary>
public static class CustomHandlebarsHelper
{
    public static void RegisterHelpers(IHandlebars handlebars)
    {
        Console.WriteLine("Registering Handlebars helpers...");
        RegisterFixedValueHelper(handlebars);
        RegisterFormatDateHelper(handlebars);
        RegisterFormatDateShortHelper(handlebars);
    }

    private static void RegisterFixedValueHelper(IHandlebars handlebars)
    {
        handlebars.RegisterHelper("toFixed", (writer, context, parameters) =>
        {
            if (parameters.Length == 0) return;
            if (decimal.TryParse(parameters[0]?.ToString(), out var number))
                writer.WriteSafeString(number.ToString("F2"));
            else
                writer.WriteSafeString(parameters[0]?.ToString() ?? "");
        });
    }

    private static void RegisterFormatDateHelper(IHandlebars handlebars)
    {
        handlebars.RegisterHelper("formatDate", (writer, context, parameters) =>
        {
            if (parameters.Length >= 1 && parameters[0] is DateTime date)
                writer.WriteSafeString(date.ToString("yyyy-MM-dd HH:mm:ss"));
            else if (parameters.Length >= 1 && parameters[0] is string s &&
                     DateTime.TryParse(s, out var parsed))
                writer.WriteSafeString(parsed.ToString("yyyy-MM-dd HH:mm:ss"));
        });
    }

    private static void RegisterFormatDateShortHelper(IHandlebars handlebars)
    {
        handlebars.RegisterHelper("formatDateShort", (writer, context, parameters) =>
        {
            if (parameters.Length >= 1 && parameters[0] is DateTime date)
                writer.WriteSafeString(date.ToString("yyyy-MM-dd"));
        });
    }
}
