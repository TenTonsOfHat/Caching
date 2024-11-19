using Library.Caching;

namespace Library.CachedItems;

public class Weather2(ICollection<Weather2Forecast> forecasts) : ICachable
{
    public ICollection<Weather2Forecast> Forecasts { get; init; } = forecasts;
}

public record Weather2Forecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


public class Weather2CacheFactory : ICacheFactory<Weather2>, ITransientService
{
    public Task<Weather2> CreateAsync()
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var results = Enumerable.Range(1, 5).Select(index =>
                new Weather2Forecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();

        return Task.FromResult(new Weather2(results));
    }


}