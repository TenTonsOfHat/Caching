using Library.CachedItems;
using Library.Caching;

namespace Library.Repository;

public class CacheRepository(ICached<Weather> weather, ICached<Weather2> weather2) : ICacheRepository
{
    public ICached<Weather> Weather { get; } = weather;
    public ICached<Weather2> Weather2 { get; } = weather2;
}

public interface ICacheRepository : ITransientService
{
    ICached<Weather> Weather { get; }
    ICached<Weather2> Weather2 { get; }
}