using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);


            builder.Property(p => p.Description)
                .HasMaxLength(2000);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsMany(p => p.Features, f =>
            {
                f.WithOwner().HasForeignKey("ProductId");
                f.Property(pf => pf.Name).IsRequired().HasMaxLength(100);
                f.Property(pf => pf.Value).IsRequired().HasMaxLength(200);
                f.ToTable("ProductFeatures");
            });

            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
