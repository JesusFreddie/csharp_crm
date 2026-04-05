using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration;

public class LeadConfiguration : IEntityTypeConfiguration<Lead>
{
    public void Configure(EntityTypeBuilder<Lead> builder)
    {
        builder.ToTable("leads");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(l => l.Name)
            .HasColumnName("name")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(l => l.Description)
            .HasColumnName("description")
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(l => l.IsArchived)
            .HasColumnName("is_archived")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(l => l.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(l => l.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(l => l.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        // Configure DealAmount as owned entity (value object)
        builder.OwnsOne(l => l.Amount, amount =>
        {
            amount.Property(a => a.Amount)
                .HasColumnName("deal_amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            amount.Property(a => a.Mode)
                .HasColumnName("pricing_mode")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();
        });

        // Indexes
        builder.HasIndex(l => l.Name)
            .HasDatabaseName("ix_leads_name");

        builder.HasIndex(l => l.IsArchived)
            .HasDatabaseName("ix_leads_is_archived");

        builder.HasIndex(l => l.IsDeleted)
            .HasDatabaseName("ix_leads_is_deleted");

        builder.HasIndex(l => l.CreatedAt)
            .HasDatabaseName("ix_leads_created_at");
    }
}
