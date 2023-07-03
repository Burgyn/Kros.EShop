using Kros.Framework.Mediator;
using Kros.Framework.Modules;

namespace Kros.EShop;

public class OrderEventHandler : DomainEventHandlerBase<EntityCreatingEvent<Order>>,
    IDomainEventHandler<EntityCreatedEvent<Order>>
{
    public override Task HandleAsync(EntityCreatingEvent<Order> domainEvent, CancellationToken cancellationToken)
    {
        domainEvent.Entity.Number ??= $"O-{DateTime.Now:yymmdd}-{DateTime.Now.TimeOfDay.Ticks}";
        return Task.CompletedTask;
    }

    public Task HandleAsync(EntityCreatedEvent<Order> domainEvent, CancellationToken cancellationToken = default)
    {
        // send email
        return Task.CompletedTask;
    }
}
