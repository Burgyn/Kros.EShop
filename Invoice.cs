using Kros.Framework.Modules;

public class Invoice : Entity<Guid>
{
    public decimal TotalPrice { get; set; }

    public string Number { get; set; }

    public DateTime IssueDate { get; set; }

    public List<InvoiceItem> Items { get; set; }
}
