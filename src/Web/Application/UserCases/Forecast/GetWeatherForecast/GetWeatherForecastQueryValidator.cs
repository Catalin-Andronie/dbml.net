using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DbmlNet.Web.Application.Common;

namespace DbmlNet.Web.Application.UserCases.Forecast.GetWeatherForecast;

#pragma warning disable CA1822 // Mark members as static

/// <summary>
/// Represents a validator for <see cref="GetWeatherForecastQuery"/>.
/// </summary>
public sealed class GetWeatherForecastQueryValidator
{
    /// <summary>
    /// Validates the specified command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ApplicationValidationException">The validation failed.</exception>
    public Task ValidateAsync(GetWeatherForecastQuery command)
    {
        ArgumentNullException.ThrowIfNull(command);

        List<ValidationError> failures = new List<ValidationError>();

        if (command.NumberOfDays < 1)
        {
            failures.Add(new ValidationError
            {
                PropertyName = nameof(command.NumberOfDays),
                ErrorMessage = "The number of days cannot be less than 1."
            });
        }

        if (failures.Count != 0)
        {
            throw new ApplicationValidationException(failures);
        }

        return Task.CompletedTask;
    }
}
