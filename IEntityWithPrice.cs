using Kros.Framework.Mediator;
using Kros.Framework.Modules;

public interface IEntityWithPrice : IEntity
{
    decimal TotalPrice { get; set; }

    IEnumerable<IEntityPriceItem> Items { get; }
}

public interface IEntityPriceItem
{
    decimal UnitPrice { get; }

    decimal Amount { get; }

    decimal TotalPrice { get; set; }
}

public class EntityWithPriceHandler<TEntity> :
    DomainEventHandlerBase<EntityCreatingEvent<TEntity>>,
    IDomainEventHandler<EntityUpdatingEvent<TEntity>>
    where TEntity : class, IEntityWithPrice
{
    public override Task HandleAsync(EntityCreatingEvent<TEntity> domainEvent, CancellationToken cancellationToken)
    {
        Recalculate(domainEvent.Entity);
        return Task.CompletedTask;
    }

    public Task HandleAsync(EntityUpdatingEvent<TEntity> domainEvent, CancellationToken cancellationToken = default)
    {
        Recalculate(domainEvent.Entity);
        return Task.CompletedTask;
    }

    private void Recalculate(TEntity entity)
    {
        decimal totalPrice = 0;

        foreach (var item in entity.Items)
        {
            item.TotalPrice = item.UnitPrice * item.Amount;
            totalPrice += item.TotalPrice;
        }

        entity.TotalPrice = totalPrice;
    }
}

public static class DefaultRegistratorExtension
{
    public static DefaultRegistrator AddPriceRecalculationExtension(this DefaultRegistrator registrator)
        => registrator.AddExtension<IEntityWithPrice, EntityWithPriceHandler<IEntityWithPrice>>();
}
