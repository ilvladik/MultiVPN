using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutlineServerApi.Core.Models;

namespace OutlineServerApi.Core.Configurations
{
    internal class ServerConfiguration : IEntityTypeConfiguration<Server>
    {
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name)
                .IsRequired();
            builder.Property(s => s.IsAvailable)
                .IsRequired();
            builder.HasOne(s => s.Country)
                .WithMany()
                .HasForeignKey(s => s.CountryId)
                .IsRequired();
            builder
                .HasMany(s => s.Keys)
                .WithOne(s => s.Server)
                .HasForeignKey(k => k.ServerId)
                .IsRequired();
            
            builder.Property(s => s.Hostname)
                .IsRequired();
            builder.Property(s => s.Port)
                .IsRequired();
            builder.Property(s => s.ApiPrefix)
                .IsRequired();
        }
    }
}
