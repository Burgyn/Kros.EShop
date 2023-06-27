using FluentValidation;
using FluentValidation.Results;
using Kros.EShop;
using Kros.Framework;
using Kros.Framework.Core;
using Kros.Framework.Mediator;
using Kros.Framework.Modules;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public class Order : AuditedEntity<Guid>, IEntityWithPrice
{
    public string Number { get; set; }

    public Address? ShippingAddress { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? EstimatedDeliveryDate { get; set; }

    public string? PaymentMethod { get; set; }

    public List<OrderItem> Items { get; set; }

    public OrderStatus? Status { get; set; }

    public decimal TotalPrice { get; set; }

    IEnumerable<IEntityPriceItem> IEntityWithPrice.Items => Items;
}

public enum OrderStatus
{
    New,
    InProgress,
    Completed
}

public class OrderItem : Entity<Guid>, IEntityPriceItem
{
    public Guid ProductId { get; set; }
    public decimal Amount { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public interface IOrderWriteService : IWriteOnlyService<Order>
{
    Task<Guid> CreateFromBasketAsync(TenantId tenantId, Guid baskedId, CancellationToken cancellationToken);

    Task<bool> ChangeStatusAsync(TenantId tenantId, Guid orderId, OrderStatus status, CancellationToken cancellationToken);
}

public class OrderWriteService : WriteOnlyService<Order>, IOrderWriteService
{
    private readonly IReadOnlyRepository<Basket, Guid> _basketRepository;

    public OrderWriteService(
        IServiceLocator serviceLocator,
        IReadOnlyRepository<Basket, Guid> basketRepository)
        : base(serviceLocator)
    {
        _basketRepository = basketRepository;
    }

    public async Task<Guid> CreateFromBasketAsync(TenantId tenantId, Guid baskedId, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetItemAsync(tenantId, baskedId, cancellationToken);

        // if basket doesnot exist, throw exception ...

        //map basket to order
        var order = new Order
        {
            OrderDate = DateTime.Now,
            EstimatedDeliveryDate = DateTime.Now.AddDays(5),
            Items = basket!.Items.Select(item => new OrderItem
            {
                ProductId = item.Product.Id,
                Amount = item.Amount,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.TotalPrice
            }).ToList(),
            Status = OrderStatus.New,
            TotalPrice = basket.TotalPrice
        };

        var result = await AddItemAsync(tenantId, order, cancellationToken);
        return result.Match(
            success => success.Value,
            error => throw new InvalidOperationException());
    }

    public async Task<bool> ChangeStatusAsync(
        TenantId tenantId,
        Guid orderId,
        OrderStatus status,
        CancellationToken cancellationToken)
    {
        var order = await ReadOnlyRepository.GetItemAsync(tenantId, orderId, cancellationToken);

        if (order == null)
        {
            return false;
        }
        order.Status = status;
        var ret = await Repository.UpdateItemAsync(tenantId, orderId, order, cancellationToken);

        if (ret)
        {
            await Publisher.PublishAsync(new OrderStatusChangedMessage(order), cancellationToken);
        }

        return ret;
    }
}

public record OrderStatusChangedMessage(Order Order) : IDomainEvent;

public class OrderApiBuilder : ReadOnlyApiBuilder<Order>
{
    public override void OnApiBuilding(ApiBuilderContext<Order, Order, Order, Order, Guid> context)
    {
        base.OnApiBuilding(context);

        context.WithPost()
            .WithDelete();

        context.WithEndpoints(builder =>
        {
            builder.MapPost("/create", async (
                TenantId tenantId,
                [FromBody] Guid basketId,
                [FromServices] IOrderWriteService orderService,
                CancellationToken cancellation) =>
            {
                var id = await orderService.CreateFromBasketAsync(tenantId, basketId, cancellation);
                return TypedResults.CreatedAtRoute(id, "GetOrderById", new { tenantId, id});
            });

            builder.MapPut("/complete/{id}", Complete);
        });
    }

    private static async Task<Results<NoContent, NotFound>> Complete(
        TenantId tenantId,
        Guid id,
        IOrderWriteService orderService,
        CancellationToken cancellation)
    {
        var result = await orderService.ChangeStatusAsync(tenantId, id, OrderStatus.Completed, cancellation);
        return result
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}

public class OrderModule : IModuleContext<Order>
{
    public void OnModuleBuilding(ModuleBuilder<Order, Order, Order, Order, Guid> context)
    {
        context.Services.AddScoped<IOrderWriteService, OrderWriteService>();
        context
            .WithWriteOnlyService<OrderWriteService>()
            .WithApiBuilder<OrderApiBuilder>()
            .WithEventHandler<OrderEventHandler>()
            //.WithEventHandler<OrderStatusChangeHandler>()
            .WithValidator<OrderValidator>();
    }
}

public class OrderEventHandler
    : DomainEventHandlerBase<EntityCreatingEvent<Order>>,
    IDomainEventHandler<EntityCreatedEvent<Order>>
{
    public override Task HandleAsync(EntityCreatingEvent<Order> domainEvent, CancellationToken cancellationToken)
    {
        domainEvent.Entity.Number ??= $"O-{DateTime.Now: yymmdd}-{DateTime.Now.TimeOfDay.Ticks}";
        return Task.CompletedTask;
    }

    public Task HandleAsync(EntityCreatedEvent<Order> domainEvent, CancellationToken cancellationToken = default)
    {
        // ToDo: send email, ...
        return Task.CompletedTask;
    }
}

// nezabudni registrovat
public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(x => x.EstimatedDeliveryDate).GreaterThanOrEqualTo(x => x.OrderDate);
    }

    public override ValidationResult Validate(ValidationContext<Order> context) => base.Validate(context);
}
