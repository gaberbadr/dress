using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class SettingsConfiguration : IEntityTypeConfiguration<Settings>
    {
        public void Configure(EntityTypeBuilder<Settings> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.StoreName)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasQueryFilter(s => !s.IsDeleted);
        }
    }
}
