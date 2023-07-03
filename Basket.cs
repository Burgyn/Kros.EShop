using Kros.Framework.Modules;

public class Basket : Entity<Guid>
{
    public List<BasketItem> Items { get; set; }

    public decimal TotalPrice { get; set; }
}
