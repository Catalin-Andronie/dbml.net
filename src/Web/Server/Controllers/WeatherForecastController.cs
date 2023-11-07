using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DbmlNet.Web.Application.UserCases.Forecast.GetWeatherForecast;

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/v1/weather-forecast")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet(Name = "GetWeatherForecast")]
    public Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync(
        CancellationToken cancellationToken = default)
    {
        GetWeatherForecastQuery command = new GetWeatherForecastQuery(numberOfDays: 5);
        GetWeatherForecastQueryHandler handler = new GetWeatherForecastQueryHandler();
        return handler.HandleAsync(command, cancellationToken);
    }

    [HttpGet("{numberOfDays}", Name = "GetWeatherForecastByDays")]
    public Task<IEnumerable<WeatherForecast>> GetWeatherForecastByDaysAsync(
        int numberOfDays,
        CancellationToken cancellationToken = default)
    {
        GetWeatherForecastQuery command = new GetWeatherForecastQuery(numberOfDays);
        GetWeatherForecastQueryHandler handler = new GetWeatherForecastQueryHandler();
        return handler.HandleAsync(command, cancellationToken);
    }
}
