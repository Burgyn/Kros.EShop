using Kros.Framework.Mediator;
using Kros.Framework.Modules;

namespace Kros.EShop;

public class OrderStatusChangeHandler : DomainEventHandlerBase<OrderStatusChangedMessage>
{
    private readonly IWriteOnlyService<Invoice, Invoice, Invoice, Guid> _invoiceService;
    private readonly IMapper _mapper;

    public OrderStatusChangeHandler(
        IWriteOnlyService<Invoice, Invoice, Invoice, Guid> invoiceService,
        IMapper mapper)
    {
        _invoiceService = invoiceService;
        _mapper = mapper;
    }

    public override async Task HandleAsync(OrderStatusChangedMessage domainEvent, CancellationToken cancellationToken)
    {
        await CreateInvoice(domainEvent, cancellationToken);
        // actualize product amount
    }

    private async Task CreateInvoice(OrderStatusChangedMessage domainEvent, CancellationToken cancellationToken)
    {
        var invoice = _mapper.Map<Invoice>(domainEvent.Order);
        if (domainEvent.Order.Status == OrderStatus.Completed)
        {
            await _invoiceService.AddItemAsync(domainEvent.Order.TenantId, invoice, cancellationToken);
        }
    }
}
