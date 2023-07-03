using Kros.Framework.Modules;

public class InvoiceItem : Entity<Guid>
{
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public Invoice Invoice { get; set; }
}
