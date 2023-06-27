using Kros.Framework.Modules;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kros.EShop;

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
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Number)
            .IsRequired(false);
    }
}

public sealed class OrderItemDbConfiguration : EntityGuidTypeConfiguration<OrderItem>
{
}

public sealed class ProductDbConfiguration : EntityGuidTypeConfiguration<Product>
{
}
