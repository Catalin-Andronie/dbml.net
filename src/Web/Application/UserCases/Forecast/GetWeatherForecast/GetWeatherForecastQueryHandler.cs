using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DbmlNet.Web.Application.UserCases.Forecast.GetWeatherForecast;

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
        throw new NotImplementedException();
    }
}
