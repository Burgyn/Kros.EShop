using Kros.Framework.Modules;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore.Metadata.Builders;
=======
>>>>>>> 1a354376180820a7389fdb0ecd42eaf3f1003b1d

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
<<<<<<< HEAD


public sealed class AddressDbConfiguration : EntityGuidTypeConfiguration<Address>
{
}


public sealed class BankAccountDbConfiguration : EntityGuidTypeConfiguration<BankAccount>
{
    public override void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Iban)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}

public sealed class InvoiceDbConfiguration : EntityGuidTypeConfiguration<Invoice>
{
}

public sealed class InvoiceItemDbConfiguration : EntityGuidTypeConfiguration<InvoiceItem>
{
}

public sealed class OrderDbConfiguration : EntityGuidTypeConfiguration<Order>
{
}

public sealed class OrderItemDbConfiguration : EntityGuidTypeConfiguration<OrderItem>
{
}

public sealed class ProductDbConfiguration : EntityGuidTypeConfiguration<Product>
{
}
=======
>>>>>>> 1a354376180820a7389fdb0ecd42eaf3f1003b1d
