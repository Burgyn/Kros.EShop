using Kros.EShop;
using Kros.Framework.Modules;

public class OrderItem : Entity<Guid>, IEntityPriceItem
{
    public Guid ProductId { get; set; }
    public decimal Amount { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
