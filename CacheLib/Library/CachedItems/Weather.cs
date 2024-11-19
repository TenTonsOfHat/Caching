using Library.Caching;

namespace Library.CachedItems;

public class Weather(ICollection<WeatherForecast> forecasts) : ICachable
{
    public ICollection<WeatherForecast> Forecasts { get; init; } = forecasts;
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


public class WeatherCacheFactory : ICacheFactory<Weather>, ITransientService
{
    public Task<Weather> CreateAsync()
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var results = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();

        return Task.FromResult(new Weather(results));
    }


}