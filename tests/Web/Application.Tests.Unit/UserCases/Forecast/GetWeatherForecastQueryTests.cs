using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DbmlNet.Tests.Core;
using DbmlNet.Web.Application.Common;
using DbmlNet.Web.Application.UserCases.Forecast.GetWeatherForecast;

using Xunit;

namespace DbmlNet.Web.Application.Tests.Unit.UserCases.Forecast;

#pragma warning disable VSTHRD200 // Use "Async" suffix in names of methods that return an awaitable type

public sealed class GetWeatherForecastQueryTests
{
    [Fact]
    public async Task GetWeatherForecastQuery_Should_RequireMinimumFields()
    {
        GetWeatherForecastQuery query = new GetWeatherForecastQuery(numberOfDays: 0);
        GetWeatherForecastQueryHandler handler = new GetWeatherForecastQueryHandler();

        ApplicationValidationException ex =
            await Assert
                .ThrowsAsync<ApplicationValidationException>(() => handler.HandleAsync(query))
                .ConfigureAwait(true);

        Assert.Equal("One or more validation failures have occurred.", ex.Message);
    }

    [Fact]
    public async Task GetWeatherForecastQuery_Should_RequireNumberOfDays()
    {
        int numberOfDays = DataGenerator.GetRandomNumber(max: 0);
        GetWeatherForecastQuery query = new GetWeatherForecastQuery(numberOfDays);
        GetWeatherForecastQueryHandler handler = new GetWeatherForecastQueryHandler();
        ApplicationValidationException ex =
            await Assert
                .ThrowsAsync<ApplicationValidationException>(() => handler.HandleAsync(query))
                .ConfigureAwait(true);

        KeyValuePair<string, string[]> error = Assert.Single(ex.Errors);
        Assert.Equal("NumberOfDays", error.Key);
        string errorMessage = Assert.Single(error.Value);
        Assert.Equal("The number of days cannot be less than 1.", errorMessage);
    }

    [Fact]
    public async Task GetWeatherForecastQuery_Should_ReturnExpectedForecast()
    {
        int numberOfDays = DataGenerator.GetRandomNumber(min: 1, max: 20);
        GetWeatherForecastQuery query = new GetWeatherForecastQuery(numberOfDays);
        GetWeatherForecastQueryHandler handler = new GetWeatherForecastQueryHandler();

        WeatherForecast[] forecasts =
            (await handler.HandleAsync(query).ConfigureAwait(true)).ToArray();

        Assert.NotEmpty(forecasts);
        Assert.True(numberOfDays == forecasts.Length, $"Expected number of forecasts ({forecasts.Length}) to match the requested number of days ({numberOfDays}).");
        for (int i = 0; i < forecasts.Length; i++)
        {
            WeatherForecast forecast = forecasts[i];
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now.AddDays(i + 1)), forecast.Date);
            Assert.NotNull(forecast.Summary);
            Assert.Equal(forecast.TemperatureF, 32 + (int)(forecast.TemperatureC / 0.5556));
        }
    }
}
