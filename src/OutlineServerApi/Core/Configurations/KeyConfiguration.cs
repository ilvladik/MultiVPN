using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutlineServerApi.Core.Models;

namespace OutlineServerApi.Core.Configurations
{
    internal class KeyConfiguration : IEntityTypeConfiguration<Key>
    {
        public void Configure(EntityTypeBuilder<Key> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(k => k.Name)
                .IsRequired();
            builder.Property(k => k.CreatedByUser)
                .IsRequired();

            builder.HasOne(k => k.Server)
                .WithMany(s => s.Keys)
                .HasForeignKey(k => k.ServerId);
            
            builder.Property(k => k.InternalId)
                .IsRequired();
            builder.Property(k => k.Password)
                .IsRequired();
            builder.Property(k => k.Port)
                .IsRequired();
            builder.Property(k => k.Method)
                .IsRequired();
        }
    }
}
