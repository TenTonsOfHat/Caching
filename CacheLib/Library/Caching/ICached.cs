using ZiggyCreatures.Caching.Fusion;

namespace Library.Caching;

public interface ICachable
{
}

public interface ICached
{
    
}

public interface ICached<TCacheType> where TCacheType : class, ICachable
{
    public ValueTask<TCacheType> Retrieve();
}

public class Cached<TCacheType>(ICacheFactory<TCacheType> factory, IFusionCache cache)
    : ICached<TCacheType> where TCacheType : class, ICachable
{
    private readonly SemaphoreSlim _asyncLock = new(1);

    public async ValueTask<TCacheType> Retrieve()
    {
        await _asyncLock.WaitAsync();
        try
        {
            return await _lazy.Value;
        }
        finally
        {
            _asyncLock.Release();
        }
    }

    public async ValueTask Clear()
    {
        await cache.RemoveAsync(factory.Key);
        await ResetLazyIfInitialized();

        async Task ResetLazyIfInitialized()
        {
            if (!_lazy.IsValueCreated)
                return;

            await _asyncLock.WaitAsync();
            try
            {
                if (!_lazy.IsValueCreated)
                    return;
                _lazy = InitLazy(factory, cache);
            }
            finally
            {
                _asyncLock.Release();
            }
        }
    }

    private Lazy<ValueTask<TCacheType>> _lazy = InitLazy(factory, cache);

    private static Lazy<ValueTask<TCacheType>> InitLazy(ICacheFactory<TCacheType> factory, IFusionCache cache)
    {
        return new(
            () => cache.GetOrSetAsync<TCacheType>(
                factory.Key,
                async token => await factory.CreateAsync(),
                factory.GetExpiration()
            ),
            LazyThreadSafetyMode.ExecutionAndPublication
        );
    }
}