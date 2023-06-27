using Kros.Framework.Modules;
using Microsoft.EntityFrameworkCore;

namespace Kros.EShop;

public class EShopDbContext : DbContextBase,
    IDbContext<Product>,
    IDbContext<Order>,
    IDbContext<BankAccount>,
    IDbContext<Address>,
    IDbContext<Invoice>
{
    public EShopDbContext(DbContextOptions<EShopDbContext> options) : base(options)
    {
    }

    public EShopDbContext()
    {
    }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<Invoice> Invoices => Set<Invoice>();

    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();

    public DbSet<Address> Addresses => Set<Address>();

    DbSet<Product> IDbContext<Product>.GetDbSet() => Products;

    DbSet<Order> IDbContext<Order>.GetDbSet() => Orders;

    DbSet<BankAccount> IDbContext<BankAccount>.GetDbSet() => BankAccounts;

    DbSet<Address> IDbContext<Address>.GetDbSet() => Addresses;

    DbSet<Invoice> IDbContext<Invoice>.GetDbSet() => Invoices;
}
