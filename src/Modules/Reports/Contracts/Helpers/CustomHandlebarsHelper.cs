using HandlebarsDotNet;
using System.Globalization;

namespace SmartLedger.Modules.Reports.Contracts.Helpers;

/// <summary>
/// Provides custom Handlebars helpers for report generation.
/// </summary>
public static class CustomHandlebarsHelper
{
    /// <summary>
    /// Registers a set of custom helper functions with the specified Handlebars instance.
    /// </summary>
    /// <param name="handlebars">Handlebars instance to which the helper functions will be registered.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="handlebars"/> is null.</exception>
    public static void RegisterHelpers(IHandlebars handlebars)
    {
        if (handlebars is null)
        {
            throw new ArgumentNullException(nameof(handlebars), "Handlebars instance cannot be null.");
        }

        RegisterToFixedHelper(handlebars);
        RegisterFormatDateHelper(handlebars);
        RegisterFormatDateShortHelper(handlebars);
    }

    /// <summary>
    /// Registers the "toFixed" helper, enabling formatting of numeric values to two decimal places within templates.
    /// </summary>
    private static void RegisterToFixedHelper(IHandlebars handlebars)
    {
        handlebars.RegisterHelper("toFixed", (writer, context, parameters) =>
        {
            if (parameters.Length == 0 || parameters[0] == null)
            {
                writer.WriteSafeString(string.Empty);
                return;
            }

            if (decimal.TryParse(parameters[0].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
            {
                writer.WriteSafeString(number.ToString("F2", CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteSafeString(parameters[0].ToString());
            }
        });
    }

    /// <summary>
    /// Registers the "formatDate" helper, enabling templates to format date values as "yyyy-MM-dd HH:mm:ss".
    /// </summary>
    private static void RegisterFormatDateHelper(IHandlebars handlebars)
    {
        handlebars.RegisterHelper("formatDate", (writer, context, parameters) =>
        {
            if (parameters.Length == 0 || parameters[0] == null)
            {
                writer.WriteSafeString(string.Empty);
                return;
            }

            if (parameters[0] is DateTime date)
            {
                writer.WriteSafeString(date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            }
            else if (parameters[0] is string dateString && DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                writer.WriteSafeString(parsedDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteSafeString(string.Empty);
            }
        });
    }

    /// <summary>
    /// Registers the 'formatDateShort' helper, eenabling templates to format date values as shory string in 'yyyy-MM-dd' format.
    /// </summary>
    private static void RegisterFormatDateShortHelper(IHandlebars handlebars)
    {
        handlebars.RegisterHelper("formatDateShort", (writer, context, parameters) =>
        {
            if (parameters.Length == 0 || parameters[0] == null)
            {
                writer.WriteSafeString(string.Empty);
                return;
            }

            if (parameters[0] is DateTime date)
            {
                writer.WriteSafeString(date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            }
            else if (parameters[0] is string dateString && DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                writer.WriteSafeString(parsedDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteSafeString(string.Empty);
            }
        });
    }
}