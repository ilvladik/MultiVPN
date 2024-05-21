using IdentityServerApi.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServerApi.Core.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Value)
                .IsRequired();
            builder.Property(r => r.ExpiresIn)
                .IsRequired();
            builder.HasOne(r => r.User)
                .WithOne()
                .HasForeignKey<RefreshToken>(r => r.UserId);
        }
    }
}
