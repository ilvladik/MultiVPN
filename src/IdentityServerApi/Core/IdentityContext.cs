using IdentityServerApi.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IdentityServerApi.Core
{
    internal class IdentityContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            Guid guidRoleAdmin = Guid.NewGuid();
            Guid guidUserAdmin = Guid.NewGuid();
            modelBuilder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid>()
                { 
                    Id = guidRoleAdmin,
                    Name = "Admin", 
                    NormalizedName = "ADMIN"
                },
                new IdentityRole<Guid>()
                {
                    Id = Guid.NewGuid(),
                    Name = "User",
                    NormalizedName = "USER"
                });

            var hasher = new PasswordHasher<User>();

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = guidUserAdmin,
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "vlad.ilyin.2003@gmail.com",
                    NormalizedEmail = "VLAD.ILYIN.2003@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(default, "ADMIN"),
                    EmailConfirmed = true
                }
            );


            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    RoleId = guidRoleAdmin,
                    UserId = guidUserAdmin
                }
            );
        }
    }
}
