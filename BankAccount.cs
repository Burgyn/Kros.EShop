using Kros.Framework.Modules;

public class BankAccount : Entity<Guid>
{
    public string Iban { get; set; }

    public string Name { get; set; }

    public string Bic { get; set; }

    public string BankName { get; set; }

    public string Currency { get; set; }
}
