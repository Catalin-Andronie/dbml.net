namespace DbmlNet.Web.Application.UserCases.Forecast.GetWeatherForecast;

/// <summary>
/// Represents a query to get weather forecasts.
/// </summary>
public sealed class GetWeatherForecastQuery
{
    public GetWeatherForecastQuery(int numberOfDays)
    {
        NumberOfDays = numberOfDays;
    }

    /// <summary>
    /// Gets the number of days.
    /// </summary>
    public int NumberOfDays { get; }
}
