using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace DbmlNet.Web.Application.UserCases.Forecast.GetWeatherForecast;

public sealed class GetWeatherForecastQueryHandler
{
    private readonly string[] _summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

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
        await new GetWeatherForecastQueryValidator().ValidateAsync(command).ConfigureAwait(false);

        ArgumentNullException.ThrowIfNull(command);

        return Enumerable
            .Range(1, command.NumberOfDays)
            .Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = RandomNumberGenerator.GetInt32(-20, 55),
                Summary = _summaries[RandomNumberGenerator.GetInt32(_summaries.Length)]
            }).ToArray();
    }
}
