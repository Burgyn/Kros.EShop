using Kros.Framework.Modules;

public class Address : Entity<Guid>
{
    public string Country { get; set; }
    public string Town { get; set; }
    public string PostalCode { get; set; }
    public string Street { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}
