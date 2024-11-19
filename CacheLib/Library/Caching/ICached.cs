using ZiggyCreatures.Caching.Fusion;

namespace Library.Caching;

public interface ICachable
{
}

public interface ICached<TCacheType> where TCacheType : class, ICachable
{
    public ValueTask<TCacheType> Retrieve();
}

public class Cached<TCacheType>(ICacheFactory<TCacheType> factory, IFusionCache cache)
    : ICached<TCacheType> where TCacheType : class, ICachable
{
    public ValueTask<TCacheType> Retrieve() => _lazy.Value;
    
    private readonly Lazy<ValueTask<TCacheType>> _lazy = new(
        () => cache.GetOrSetAsync<TCacheType>(
            factory.GetKey(), 
            async token => await factory.CreateAsync(), 
            factory.GetExpiration()
        ),
        LazyThreadSafetyMode.ExecutionAndPublication
    );
}