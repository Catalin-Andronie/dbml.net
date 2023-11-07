using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DbmlNet.Web.Application.Common;

namespace DbmlNet.Web.Application.UserCases.Forecast.GetWeatherForecast;

#pragma warning disable CA1822 // Mark members as static

public sealed class GetWeatherForecastQueryHandler
{
    /// <summary>
    /// Get weather forecasts.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The weather forecasts.</returns>
    public async Task<IEnumerable<WeatherForecast>> HandleAsync(
        GetWeatherForecastQuery command,
        CancellationToken cancellationToken = default)
    {
        await ValidateAsync(command).ConfigureAwait(false);

        throw new NotImplementedException();
    }

    private static Task ValidateAsync(GetWeatherForecastQuery command)
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
