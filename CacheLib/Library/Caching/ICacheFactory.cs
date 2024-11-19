namespace Library.Caching;

public interface ICacheFactory<TCacheType> where TCacheType : class, ICachable
{
    Task<TCacheType> CreateAsync();

    string GetKey() => $"{typeof(TCacheType).Assembly}.{typeof(TCacheType).Name}";
    
    TimeSpan GetExpiration() => TimeSpan.FromSeconds(60);
}