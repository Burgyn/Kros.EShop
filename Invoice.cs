using Kros.Framework.Modules;

public class Invoice : Entity<Guid>
{
    public decimal TotalPrice { get; set; }

    public string Number { get; set; }

    public DateTime IssueDate { get; set; }

    public List<InvoiceItem> Items { get; set; }
}
<<<<<<< HEAD
=======

public class InvoiceItem : Entity<Guid>
{
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public Invoice Invoice { get; set; }
}
>>>>>>> 1a354376180820a7389fdb0ecd42eaf3f1003b1d
