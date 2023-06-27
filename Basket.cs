using Kros.Framework.Core;
using Kros.Framework.Modules;
using Microsoft.Extensions.Caching.Distributed;

public class Basket : Entity<Guid>
{
    public IReadOnlyCollection<BasketItem> Items { get; set; }

    public decimal TotalPrice { get; set; }
}

public class BasketItem
{
    public Product Product { get; set; }
    public decimal UnitPrice { get; set; }
    public int Amount { get; set; }
    public decimal TotalPrice { get; set; }
}

// registruj memory cache
public class BasketRepository :
    IReadOnlyRepository<Basket, Guid>,
    IWriteOnlyRepository<Basket, Guid>
{
    private readonly IDistributedCache _distributedCache;

    public BasketRepository(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task AddItemAsync(TenantId tenantId, Basket item, CancellationToken cancellationToken = default)
    {
        await _distributedCache.SetAsync(tenantId, item, new() { SlidingExpiration = TimeSpan.FromDays(2) },
            token: cancellationToken);
    }

    public Task BatchAddItemsAsync(TenantId tenantId, IEnumerable<Basket> items, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteItemAsync(TenantId tenantId, Guid id, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(tenantId, token: cancellationToken);
        return true;
    }

    public async Task<Basket?> GetItemAsync(TenantId tenantId, Guid id, CancellationToken cancellationToken = default)
    {
        return await _distributedCache.GetAsync<Basket?>(tenantId, token: cancellationToken);
    }

    public Task<IQueryable<Basket>> GetItemsAsync(TenantId tenantId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasTenantDataAsync(TenantId tenantId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateItemAsync(TenantId tenantId, Guid id, Basket item, CancellationToken cancellationToken = default)
    {
        await _distributedCache.SetAsync(tenantId, item, new() { SlidingExpiration = TimeSpan.FromDays(2) },
            token: cancellationToken);

        return true;
    }
}
